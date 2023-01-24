Imports System.IO
Imports System.Text
Imports LibUsbDotNet
Imports LibUsbDotNet.Main

Public Class Fastboot
    Private Const USB_VID As Integer = &H18D1
    Private Const USB_PID As Integer = &HD00D
    Private Const HEADER_SIZE As Integer = 4
    Private Const BLOCK_SIZE As Integer = 512 * 1024 ' 512 KB

    Public Property LengthWritter As String = Nothing
    Public Property Timeout As Integer = 3000

    Private device As UsbDevice
    Private targetSerialNumber As String

    Public Enum Status
        Fail
        Okay
        Data
        Info
        Unknown
    End Enum

    Public Class Response
        Public Property Status As Status
        Public Property Payload As String
        Public Property RawData As Byte()

        Public Sub New(ByVal status As Status, ByVal payload As String)
            Me.Status = status
            Me.Payload = payload
        End Sub
    End Class

    Public Sub New(ByVal serial As String)
        targetSerialNumber = serial
    End Sub

    Public Sub New()
        targetSerialNumber = Nothing
    End Sub

    Private Function GetStatusFromString(ByVal header As String) As Status
        Select Case header
            Case "INFO"
                Return Status.Info
            Case "OKAY"
                Return Status.Okay
            Case "DATA"
                Return Status.Data
            Case "FAIL"
                Return Status.Fail
            Case Else
                Return Status.Unknown
        End Select
    End Function

    Public Function Wait()
        While True
            Dim allDevices = UsbDevice.AllDevices

            If allDevices.Any(Function(x) x.Vid = USB_VID And x.Pid = USB_PID) Then
                Return True
            End If

            If Main.SharedUI.counter = 0 Then
                Return False
            End If

            Main.SharedUI.Delay(1)
            Main.SharedUI.counter -= 1
            Main.SharedUI.LabelTimer.Invoke(CType(Sub() Main.SharedUI.LabelTimer.Text = Main.SharedUI.counter, Action))
        End While
        Return False
    End Function

    Public Sub Connect()
        Dim finder As UsbDeviceFinder

        If String.IsNullOrWhiteSpace(targetSerialNumber) Then
            finder = New UsbDeviceFinder(USB_VID, USB_PID)
        Else
            finder = New UsbDeviceFinder(USB_VID, USB_PID, targetSerialNumber)
        End If

        device = UsbDevice.OpenUsbDevice(finder)

        Do While device Is Nothing
            Main.SharedUI.Delay(3)
            device = UsbDevice.OpenUsbDevice(finder)
        Loop

        Dim wDev = TryCast(device, IUsbDevice)

        If TypeOf wDev Is IUsbDevice Then
            wDev.SetConfiguration(1)
            wDev.ClaimInterface(0)
        End If
    End Sub


    Public Sub Disconnect()
        device.Close()
    End Sub


    Public Function GetSerialNumber() As String
        Return device.Info.SerialString
    End Function


    Public Function Command(ByVal pCommand As Byte()) As Response
        Dim writeEndpoint = device.OpenEndpointWriter(WriteEndpointID.Ep01)
        Dim readEndpoint = device.OpenEndpointReader(ReadEndpointID.Ep01)

        Dim wrAct As Integer = pCommand.Length
        writeEndpoint.Write(pCommand, Timeout, wrAct)

        If wrAct <> pCommand.Length Then
            Throw New Exception($"Failed to write command! Transfered: {wrAct} of {pCommand.Length} bytes")
        End If

        Dim status As Status
        Dim response As StringBuilder = New StringBuilder()
        Dim buffer As Byte() = New Byte(63) {}
        Dim strBuffer As String = Nothing
        Dim rdAct As Integer = buffer.Length

        While True
            readEndpoint.Read(buffer, Timeout, rdAct)

            strBuffer = Encoding.ASCII.GetString(buffer)

            If strBuffer.Length < HEADER_SIZE Then
                status = Status.Unknown
            Else
                Dim header As String = New String(strBuffer.Take(HEADER_SIZE).ToArray())

                status = GetStatusFromString(header)
            End If

            response.Append(strBuffer.Skip(HEADER_SIZE).Take(rdAct - HEADER_SIZE).ToArray())

            response.Append(Microsoft.VisualBasic.Constants.vbLf)

            If status <> Status.Info Then
                Exit While
            End If
        End While

        Dim str As String = response.ToString().Replace(CStr(Microsoft.VisualBasic.Constants.vbCr), CStr(String.Empty)).Replace(Microsoft.VisualBasic.Constants.vbNullChar, String.Empty)

        Return New Response(status, str) With {
                    .RawData = Encoding.ASCII.GetBytes(strBuffer)
                }
    End Function


    Private Sub SendDataCommand(ByVal size As Long)
        If Command($"download:{size:X8}").Status <> Status.Data Then
            Throw New Exception($"Invalid response from device! (data size: {size})")
        End If
    End Sub


    Private Sub TransferBlock(ByVal stream As FileStream, ByVal writeEndpoint As UsbEndpointWriter, ByVal buffer As Byte(), ByVal size As Integer)
        Dim act As Integer = stream.Read(buffer, 0, size)
        writeEndpoint.Write(buffer, Timeout, act)
        If act <> size Then
            Throw New Exception($"Failed to transfer block (sent {act} of {size})")
        End If
    End Sub


    Public Sub UploadData(ByVal stream As FileStream)
        Dim writeEndpoint As UsbEndpointWriter = device.OpenEndpointWriter(WriteEndpointID.Ep01)
        Dim readEndpoint As UsbEndpointReader = device.OpenEndpointReader(ReadEndpointID.Ep01)

        Dim totallength As String = stream.Length / 10000
        Dim length As String = stream.Length
        Dim buffer As Byte() = New Byte(524287) {}

        SendDataCommand(length)

        Dim resultprogress As String
        Dim fileOffset As String = 0
        Dim totalprogress As Long = 0
        '100000000
        While length >= BLOCK_SIZE
            fileOffset += stream.Length
            totalprogress += length / 10000
            resultprogress = CInt(Math.Round((fileOffset / 100000000))) - CInt(Math.Round((totalprogress / 100000000)))
            Console.WriteLine("Result : " & resultprogress & " Total : " & totallength)
            If resultprogress < totallength Then
                Main.SharedUI.ProcessBar1(resultprogress, totallength)
            End If

            TransferBlock(stream, writeEndpoint, buffer, BLOCK_SIZE)
            length -= BLOCK_SIZE
        End While

        If length > 0 Then
            buffer = New Byte(length - 1) {}
            TransferBlock(stream, writeEndpoint, buffer, length)
        End If

        Dim resBuffer As Byte() = New Byte(63) {}

        readEndpoint.Read(resBuffer, Timeout, resBuffer.Length)

        Dim strBuffer As String = Encoding.ASCII.GetString(resBuffer)

        If strBuffer.Length < HEADER_SIZE Then
            Throw New Exception($"Invalid response from device: {strBuffer}")
        End If

        Dim header As String = New String(strBuffer.Take(HEADER_SIZE).ToArray())

        Dim status As Status = GetStatusFromString(header)

        If status <> Status.Okay Then
            Throw New Exception($"Invalid status: {strBuffer}")
        Else
            Main.SharedUI.ProcessBar1(100, 100)
        End If
    End Sub

    Public Sub UploadData(ByVal path As String)
        Using stream = New FileStream(path, FileMode.Open)
            UploadData(stream)
        End Using
    End Sub

    Public Shared Function GetDevices() As String()
        Dim dev As UsbDevice

        Dim devices = New List(Of String)()

        Dim allDevices = UsbDevice.AllDevices

        For Each usbRegistry As UsbRegistry In allDevices
            If usbRegistry.Vid <> USB_VID OrElse usbRegistry.Pid <> USB_PID Then
                Continue For
            End If

            If usbRegistry.Open(dev) Then
                devices.Add(dev.Info.SerialString)
                dev.Close()
            End If
        Next

        Return devices.ToArray()
    End Function

    Public Function Command(ByVal pCommand As String) As Response
        Return Command(Encoding.ASCII.GetBytes(pCommand))
    End Function
End Class