using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using Microsoft.VisualBasic;

namespace FastBootFlashingXiaomi
{

    public class Fastboot
    {
        private const int USB_VID = 0x18D1;
        private const int USB_PID = 0xD00D;
        private const int HEADER_SIZE = 4;
        private const int BLOCK_SIZE = 512 * 1024; // 512 KB

        public int Timeout { get; set; } = 3000;

        private UsbDevice device;
        private string targetSerialNumber;

        public enum Status
        {
            Fail,
            Okay,
            Data,
            Info,
            Unknown
        }

        public class Response
        {
            public Status Status { get; set; }
            public string Payload { get; set; }
            public byte[] RawData { get; set; }

            public Response(Status status, string payload)
            {
                Status = status;
                Payload = payload;
            }
        }

        public Fastboot(string serial)
        {
            targetSerialNumber = serial;
        }

        public Fastboot()
        {
            targetSerialNumber = null;
        }

        private Status GetStatusFromString(string header)
        {
            switch (header ?? "")
            {
                case "INFO":
                    {
                        return Status.Info;
                    }
                case "OKAY":
                    {
                        return Status.Okay;
                    }
                case "DATA":
                    {
                        return Status.Data;
                    }
                case "FAIL":
                    {
                        return Status.Fail;
                    }

                default:
                    {
                        return Status.Unknown;
                    }
            }
        }

        public object Wait()
        {
            while (true)
            {
                var allDevices = UsbDevice.AllDevices;

                if (allDevices.Any(x => x.Vid == USB_VID & x.Pid == USB_PID))
                {
                    return true;
                }

                if (Main.SharedUI.counter == 0)
                {
                    return false;
                }

                Main.SharedUI.Delay(1d);
                Main.SharedUI.counter -= 1;
                Main.SharedUI.LabelTimer.Invoke(new Action(() => Main.SharedUI.LabelTimer.Text = Main.SharedUI.counter.ToString()));
            }
            return false;
        }

        public void Connect()
        {
            UsbDeviceFinder finder;

            if (string.IsNullOrWhiteSpace(targetSerialNumber))
            {
                finder = new UsbDeviceFinder(USB_VID, USB_PID);
            }
            else
            {
                finder = new UsbDeviceFinder(USB_VID, USB_PID, targetSerialNumber);
            }

            device = UsbDevice.OpenUsbDevice(finder);

            while (device is null)
            {
                Main.SharedUI.Delay(3d);
                device = UsbDevice.OpenUsbDevice(finder);
            }

            IUsbDevice wDev = device as IUsbDevice;

            if (wDev is IUsbDevice)
            {
                wDev.SetConfiguration(1);
                wDev.ClaimInterface(0);
            }
        }


        public void Disconnect()
        {
            device.Close();
        }


        public string GetSerialNumber()
        {
            return device.Info.SerialString;
        }


        public Response Command(byte[] pCommand)
        {
            var writeEndpoint = device.OpenEndpointWriter(WriteEndpointID.Ep01);
            var readEndpoint = device.OpenEndpointReader(ReadEndpointID.Ep01);

            int wrAct = default;
            writeEndpoint.Write(pCommand, Timeout, out wrAct);

            if (wrAct != pCommand.Length)
            {
                throw new Exception($"Failed to write command! Transfered: {wrAct} of {pCommand.Length} bytes");
            }

            Status status;
            var response = new StringBuilder();
            var buffer = new byte[64];
            string strBuffer = null;
            int rdAct = default;

            while (true)
            {
                readEndpoint.Read(buffer, Timeout, out rdAct);

                strBuffer = Encoding.ASCII.GetString(buffer);

                if (strBuffer.Length < HEADER_SIZE)
                {
                    status = Status.Unknown;
                }
                else
                {
                    string header = new string(strBuffer.Take(HEADER_SIZE).ToArray());

                    status = GetStatusFromString(header);
                }

                response.Append(strBuffer.Skip(HEADER_SIZE).Take(rdAct - HEADER_SIZE).ToArray());

                response.Append(Constants.vbLf);

                if (status != Status.Info)
                {
                    break;
                }
            }

            string str = response.ToString().Replace(Constants.vbCr, string.Empty).Replace(Constants.vbNullChar, string.Empty);

            return new Response(status, str) { RawData = Encoding.ASCII.GetBytes(strBuffer) };
        }


        private void SendDataCommand(long size)
        {
            if (Command($"download:{size:X8}").Status != Status.Data)
            {
                throw new Exception($"Invalid response from device! (data size: {size})");
            }
        }


        private void TransferBlock(FileStream stream, UsbEndpointWriter writeEndpoint, byte[] buffer, int size)
        {
            stream.Read(buffer, 0, size);
            int act = default;
            writeEndpoint.Write(buffer, Timeout, out act);

            if (act != size)
            {
                throw new Exception($"Failed to transfer block (sent {act} of {size})");
            }
        }


        public void UploadData(FileStream stream)
        {
            var writeEndpoint = device.OpenEndpointWriter(WriteEndpointID.Ep01);
            var readEndpoint = device.OpenEndpointReader(ReadEndpointID.Ep01);

            long totallength = stream.Length;
            long length = stream.Length;
            var buffer = new byte[524288];

            SendDataCommand(length);

            var Progress = new ProgressBar();
            Progress.Minimum = 0;
            Progress.Maximum = 100;
            int resultprogress;
            long fileOffset = 0L;
            long totalprogress = 0L;

            while (length >= BLOCK_SIZE)
            {
                fileOffset += stream.Length;
                totalprogress += length;
                resultprogress = (int)Math.Round(Math.Round(fileOffset / 100d)) - (int)Math.Round(Math.Round(totalprogress / 100d));

                if (resultprogress < totallength)
                {
                    Main.SharedUI.ProcessBar1(resultprogress, totallength);
                }

                TransferBlock(stream, writeEndpoint, buffer, BLOCK_SIZE);
                length -= BLOCK_SIZE;
            }

            if (length > 0L)
            {
                buffer = new byte[(int)(length - 1L + 1)];
                TransferBlock(stream, writeEndpoint, buffer, (int)length);
            }

            var resBuffer = new byte[64];

            int argtransferLength = default;
            readEndpoint.Read(resBuffer, Timeout, out argtransferLength);

            string strBuffer = Encoding.ASCII.GetString(resBuffer);

            if (strBuffer.Length < HEADER_SIZE)
            {
                throw new Exception($"Invalid response from device: {strBuffer}");
            }

            string header = new string(strBuffer.Take(HEADER_SIZE).ToArray());

            var status = GetStatusFromString(header);

            if (status != Status.Okay)
            {
                throw new Exception($"Invalid status: {strBuffer}");
            }
            else
            {
                Main.SharedUI.ProcessBar1(100L, 100L);
            }
        }

        public void UploadData(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                UploadData(stream);
            }
        }

        public static string[] GetDevices()
        {
            UsbDevice dev;

            var devices = new List<string>();

            var allDevices = UsbDevice.AllDevices;

            foreach (UsbRegistry usbRegistry in allDevices)
            {
                if (usbRegistry.Vid != USB_VID || usbRegistry.Pid != USB_PID)
                {
                    continue;
                }

                if (usbRegistry.Open(out dev))
                {
                    devices.Add(dev.Info.SerialString);
                    dev.Close();
                }
            }

            return devices.ToArray();
        }

        public Response Command(string pCommand)
        {
            return Command(Encoding.ASCII.GetBytes(pCommand));
        }
    }
}