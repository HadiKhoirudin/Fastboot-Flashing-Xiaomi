using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace FastBootFlashingXiaomi
{
    public partial class Main
    {

        private BackgroundWorker _FastbootWorker;

        public virtual BackgroundWorker FastbootWorker
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _FastbootWorker;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _FastbootWorker = value;
            }
        }
        public static Fastboot Fastboot = new Fastboot();

        internal static Main SharedUI;
        public string WorkerTodo;
        public bool IsConnected;
        public string totalchecked;
        public string totaldo;
        public string TodoCommand;
        #region UI
        public Main()
        {
            FastbootWorker = new BackgroundWorker();
            Load += Main_Load;
            InitializeComponent();
            SharedUI = this;
        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            RichTextBox.Invoke(new Action(() =>
                {
                    RichTextBox.SelectionStart = RichTextBox.Text.Length;
                    RichTextBox.ScrollToCaret();
                }));
        }
        public void RichLogs(string msg, Color colour, bool isBold, bool NextLine = false)
        {
            RichTextBox.Invoke(new Action(() =>
                {
                    RichTextBox.SelectionStart = RichTextBox.Text.Length;
                    var selectionColor = RichTextBox.SelectionColor;
                    RichTextBox.SelectionColor = colour;
                    if (isBold)
                    {
                        RichTextBox.SelectionFont = new Font(RichTextBox.Font, FontStyle.Bold);
                    }
                    else
                    {
                        RichTextBox.SelectionFont = new Font(RichTextBox.Font, FontStyle.Regular);
                    }
                    RichTextBox.AppendText(msg);
                    RichTextBox.SelectionColor = selectionColor;
                    if (NextLine)
                    {
                        if (RichTextBox.TextLength > 0)
                        {
                            RichTextBox.AppendText(Constants.vbCrLf);
                        }
                    }
                }));
        }
        private void Main_Load(object sender, EventArgs e)
        {
            RichLogs("<+++++++++++  Fastboot Flashing Xiaomi  +++++++++++>", Color.White, true, true);
            RichLogs("► Software  " + Constants.vbTab + ": ", Color.White, true, false);
            RichLogs("Fastboot Tool", Color.White, true, true);
            RichLogs("► License  " + Constants.vbTab + ": ", Color.White, true, false);
            RichLogs("Maintainer", Color.White, true, true);
            RichLogs("  =============================================", Color.White, true, true);
            RichLogs("► Websites  " + Constants.vbTab + ":  https://facebook.com/f.hadikhoir/", Color.White, true, true);
            RichLogs("  =============================================", Color.White, true, true);
            RichLogs("", Color.White, true, true);
        }
        public void ProcessBar1(long Process, long total)
        {
            ProgressBar1.Invoke(new Action(() => ProgressBar1.Value = (int)Math.Round(Math.Round(Process * 100L / (double)total))));
        }
        #endregion


        #region Function
        public bool FastbootConnect()
        {
            RichLogs("Waiting devices to connect... ", Color.White, false, false);
            bool status = Conversions.ToBoolean(Fastboot.Wait());
            if (status)
            {
                Fastboot.Connect();
                ComboBoxDevices.Invoke(new Action(() => ComboBoxDevices.Text = Fastboot.GetSerialNumber()));
                return true;
            }
            else
            {
                ComboBoxDevices.Invoke(new Action(() => ComboBoxDevices.Text = ""));
                RichLogs("Devices Not Found!", Color.Red, false, true);
            }
            return false;
        }

        public void Delay(double dblSecs)
        {
            DateTime.Now.AddSeconds(0.0000115740740740741d);
            var dateTime = DateTime.Now.AddSeconds(0.0000115740740740741d);
            var dateTime1 = dateTime.AddSeconds(dblSecs);
            while (DateTime.Compare(DateTime.Now, dateTime1) <= 0)
                Application.DoEvents();
        }
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DataView.Rows.Count > 0)
            {
                if (CheckBox.Checked)
                {
                    foreach (DataGridViewRow item in DataView.Rows)
                        item.Cells[0].Value = true;
                }
                else
                {
                    foreach (DataGridViewRow item in DataView.Rows)
                        item.Cells[0].Value = false;
                }
            }
        }
        private void DataView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DataView.Rows.Count > 0)
            {
                if (e.ColumnIndex == 3)
                {
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(DataView.CurrentRow.Cells[1].Value, "flash", false)) || Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(DataView.CurrentRow.Cells[1].Value, "boot", false)))
                    {
                        var openFileDialog = new OpenFileDialog();
                        openFileDialog.Title = Conversions.ToString(Operators.AddObject("Select File Partition ", DataView.CurrentRow.Cells[2].Value));
                        openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                        openFileDialog.FileName = "*.*";
                        openFileDialog.Filter = "ALL FILE  (*.*)|*.*";
                        openFileDialog.FilterIndex = 2;
                        openFileDialog.RestoreDirectory = true;
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            DataView.CurrentRow.Cells[3].Value = openFileDialog.SafeFileName;
                            DataView.CurrentRow.Cells[4].Value = openFileDialog.FileName;
                        }
                    }
                    else
                    {
                        Interaction.MsgBox("Custom file for flash and boot command!");
                    }
                }
            }
        }
        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "File",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                FileName = "*.bat*",
                Filter = "bat file |*.bat* ",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                TextBoxLocation.Text = Path.Combine(new string[] { Path.GetDirectoryName(openFileDialog.FileName) });

                DataView.Rows.Clear();

                string str = string.Concat(new string[] { File.ReadAllText(openFileDialog.FileName) });

                string product = str.Substring(str.LastIndexOf("^product: *") + 1);
                product = product.Replace("product: *", "").Replace("\"", "").Replace(" || exit /B 1", "");

                var resultproduct = new TextBox();

                resultproduct.Text = product;

                var strs = new List<string>();
                var lines = resultproduct.Lines;
                int num = 0;

                while (num < lines.Length)
                {
                    string textlines = lines[num];
                    strs.Add(textlines);
                    num = num + 1;
                }

                product = strs[0].Replace(" ", "");

                LabelProductName.Text = product;
                if (str.Contains(")"))
                {
                    str = str.Substring(str.LastIndexOf(")") + 1);
                }

                using (var stringReader = new StringReader(str))
                {
                    while (stringReader.Peek() != -1)
                    {
                        string str1 = stringReader.ReadLine();
                        string command = "";
                        string partition = "";
                        string filename = "";
                        string path = TextBoxLocation.Text + @"\images";
                        if (str1.Contains("||"))
                        {
                            int l;
                            int p;
                            l = str1.Length;
                            p = str1.IndexOf("||") - 1;
                            str1 = str1.Remove(p, l - p);
                            str1 = str1.Replace("fastboot %* ", "").Replace(@"%~dp0images\", "").Replace("pause", "");
                        }
                        else
                        {
                            str1 = str1.Replace("fastboot %* ", "").Replace(@"%~dp0images\", "").Replace("pause", "");
                        }

                        if (!string.IsNullOrEmpty(str1))
                        {

                            Console.WriteLine(str1);

                            var strArrays = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            command = strArrays[0];

                            if (command != "getvar")
                            {
                                if (strArrays.Length == 2)
                                {
                                    partition = strArrays[1];
                                }

                                if (strArrays.Length == 3)
                                {
                                    partition = strArrays[1];
                                    filename = strArrays[2];
                                }

                                DataView.Invoke(new Action(() => DataView.Rows.Add(true, command, partition, filename, path)));
                            }

                        }
                    }
                }
            }

        }
        public void ReadFastbootDeviceInfo()
        {

            RichLogs("Reading Device Info : ... ", Color.White, false, true);
            RichLogs(Fastboot.Command("getvar:all").Payload, Color.White, false, true);

        }
        public void AllIsDone(object sender, RunWorkerCompletedEventArgs e)
        {
            RichLogs(" ", Color.Red, true, true);
            RichLogs("_______________________________________________", Color.WhiteSmoke, true, true);
            RichLogs("All Progress Completed ... ", Color.WhiteSmoke, false, true);
            if (IsConnected)
            {
                Fastboot.Disconnect();
            }
        }
        private void ButtonSTOP_Click(object sender, EventArgs e)
        {
            if (FastbootWorker.IsBusy)
            {
                FastbootWorker.CancelAsync();
            }
        }

        private void ButtonFlash_Click(object sender, EventArgs e)
        {
            if (!FastbootWorker.IsBusy)
            {
                var flag = default(bool);
                foreach (DataGridViewRow item in DataView.Rows)
                {
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(item.Cells[0].Value, true, false)))
                    {
                        flag = true;
                    }
                }

                if (flag)
                {
                    RichTextBox.Clear();
                    WorkerTodo = "flash";

                    TodoCommand = "";
                    totalchecked = 0.ToString();
                    string commands;
                    string args;
                    string filename;
                    foreach (DataGridViewRow item in DataView.Rows)
                    {
                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(item.Cells[DataView.Columns[0].Index].Value, true, false)))
                        {

                            totalchecked = (totalchecked + 1d).ToString();

                            commands = Conversions.ToString(item.Cells[DataView.Columns[1].Index].Value);
                            args = Conversions.ToString(item.Cells[DataView.Columns[2].Index].Value);
                            filename = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(item.Cells[DataView.Columns[4].Index].Value, @"\"), item.Cells[DataView.Columns[3].Index].Value));

                            if (string.IsNullOrEmpty(args))
                            {

                                TodoCommand = string.Concat(TodoCommand, commands + Constants.vbCrLf + "");
                            }

                            else if (string.IsNullOrEmpty(filename))
                            {

                                TodoCommand = string.Concat(TodoCommand, commands + " " + args + Constants.vbCrLf + "");
                            }

                            else
                            {

                                TodoCommand = string.Concat(TodoCommand, commands + " " + args + " " + filename + Constants.vbCrLf + "");

                            }

                        }
                    }

                    FastbootWorker = new BackgroundWorker();
                    FastbootWorker.WorkerSupportsCancellation = true;
                    FastbootWorker.DoWork += Worker;
                    FastbootWorker.RunWorkerCompleted += AllIsDone;
                    FastbootWorker.RunWorkerAsync();
                    FastbootWorker.Dispose();
                }
            }
            else
            {
                RichLogs(" ", Color.White, true, true);
                RichLogs("Fastboot Is Running", Color.WhiteSmoke, false, true);
            }
        }
        private void ButtonEDL_Click(object sender, EventArgs e)
        {
            if (!FastbootWorker.IsBusy)
            {
                RichTextBox.Clear();
                WorkerTodo = "EDL";
                FastbootWorker = new BackgroundWorker();
                FastbootWorker.WorkerSupportsCancellation = true;
                FastbootWorker.DoWork += Worker;
                FastbootWorker.RunWorkerCompleted += AllIsDone;
                FastbootWorker.RunWorkerAsync();
                FastbootWorker.Dispose();
            }
            else
            {
                RichLogs(" ", Color.White, true, true);
                RichLogs("Fastboot Is Running", Color.WhiteSmoke, false, true);
            }
        }

        private void ButtonReadInfo_Click(object sender, EventArgs e)
        {
            if (!FastbootWorker.IsBusy)
            {
                RichTextBox.Clear();
                WorkerTodo = "info";
                FastbootWorker = new BackgroundWorker();
                FastbootWorker.WorkerSupportsCancellation = true;
                FastbootWorker.DoWork += Worker;
                FastbootWorker.RunWorkerCompleted += AllIsDone;
                FastbootWorker.RunWorkerAsync();
                FastbootWorker.Dispose();
            }
            else
            {
                RichLogs(" ", Color.White, true, true);
                RichLogs("Fastboot Is Running", Color.WhiteSmoke, false, true);
            }
        }

        private void ButtonRebootSYS_Click(object sender, EventArgs e)
        {
            if (!FastbootWorker.IsBusy)
            {
                RichTextBox.Clear();
                WorkerTodo = "reboot";
                FastbootWorker = new BackgroundWorker();
                FastbootWorker.WorkerSupportsCancellation = true;
                FastbootWorker.DoWork += Worker;
                FastbootWorker.RunWorkerCompleted += AllIsDone;
                FastbootWorker.RunWorkerAsync();
                FastbootWorker.Dispose();
            }
            else
            {
                RichLogs(" ", Color.White, true, true);
                RichLogs("Fastboot Is Running", Color.WhiteSmoke, false, true);
            }
        }
        private void Worker(object sender, DoWorkEventArgs e)
        {
            bool Connect = FastbootConnect();
            if (Connect)
            {
                RichLogs("Device Connected! ", Color.Lime, false, true);
                Delay(0.5d);

                if (WorkerTodo == "flash")
                {
                    string product = Fastboot.Command("getvar:product").Payload;
                    if (product.Contains(LabelProductName.Text))
                    {
                        totaldo = 0.ToString();
                        using (var stringReader = new StringReader(TodoCommand))
                        {
                            while (stringReader.Peek() != -1)
                            {
                                string str1 = stringReader.ReadLine();
                                string command = "";
                                string partition = "";
                                string oem = "";
                                string filename = "";

                                if (!string.IsNullOrEmpty(str1))
                                {

                                    Console.WriteLine(str1);

                                    var strArrays = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                    if (strArrays.Length == 1)
                                    {
                                        command = strArrays[0];
                                        RichLogs(command + " ", Color.White, false, false);
                                        RichLogs(Fastboot.Command(command).Status.ToString(), Color.Lime, false, true);
                                    }

                                    if (strArrays.Length == 2)
                                    {
                                        command = strArrays[0];
                                        if (command == "erase")
                                        {
                                            partition = strArrays[1];
                                            RichLogs(command + " " + partition + " ", Color.White, false, false);
                                            RichLogs(Fastboot.Command(command + ":" + partition).Status.ToString(), Color.Lime, false, true);
                                        }
                                        else if (command == "boot")
                                        {
                                            filename = strArrays[1];
                                            if (File.Exists(filename))
                                            {
                                                Fastboot.UploadData(new FileStream(filename, FileMode.Open));
                                                RichLogs(Fastboot.Command(command).Status.ToString(), Color.Lime, false, true);
                                            }
                                            else
                                            {
                                                RichLogs("File Doesn't Exist", Color.Yellow, false, true);
                                            }
                                        }
                                        else if (command == "oem")
                                        {
                                            oem = strArrays[1];
                                            RichLogs(Fastboot.Command(command + " " + oem).Status.ToString(), Color.Lime, false, true);
                                        }
                                    }

                                    if (strArrays.Length == 3)
                                    {
                                        command = strArrays[0];
                                        if (command == "flash")
                                        {
                                            partition = strArrays[1];
                                            filename = strArrays[2];
                                            if (File.Exists(filename))
                                            {
                                                RichLogs(command + " " + partition + " ", Color.White, false, false);
                                                Fastboot.UploadData(new FileStream(filename, FileMode.Open));
                                                RichLogs(Fastboot.Command(command + ":" + partition).Status.ToString(), Color.Lime, false, true);
                                            }
                                            else
                                            {
                                                RichLogs("File Doesn't Exist", Color.Yellow, false, true);
                                            }
                                        }
                                        else if (command == "erase")
                                        {
                                            partition = strArrays[1];
                                            if (File.Exists(filename))
                                            {
                                                RichLogs(Fastboot.Command(command + ":" + partition).Status.ToString(), Color.Lime, false, true);
                                            }
                                        }
                                        else if (command == "boot")
                                        {
                                            filename = strArrays[2];
                                            if (File.Exists(filename))
                                            {
                                                Fastboot.UploadData(new FileStream(filename, FileMode.Open));
                                                RichLogs(Fastboot.Command(command).Status.ToString(), Color.Lime, false, true);
                                            }
                                            else
                                            {
                                                RichLogs("File Doesn't Exist", Color.Yellow, false, true);
                                            }
                                        }

                                    }

                                    totaldo = (totaldo + 1d).ToString();
                                    ProcessBar1(Conversions.ToLong(totaldo), Conversions.ToLong(totalchecked));
                                }
                                if (FastbootWorker.CancellationPending)
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                    }

                    else
                    {
                        Console.WriteLine("From Device : " + Fastboot.Command("getvar:product").Payload + "From Image : " + LabelProductName.Text);
                        RichLogs("Error! Missmatching image and device.", Color.Red, false, true);
                    }
                }


                else if (WorkerTodo == "info")
                {
                    ReadFastbootDeviceInfo();
                }

                else if (WorkerTodo == "reboot")
                {
                    RichLogs("Rebooting into Android... ", Color.White, false, false);
                    RichLogs(Fastboot.Command("reboot").Status.ToString(), Color.Lime, false, true);
                }

                else if (WorkerTodo == "EDL")
                {
                    RichLogs("Rebooting into EDL Mode... ", Color.White, false, false);
                    RichLogs(Fastboot.Command("reboot-edl").Status.ToString(), Color.Lime, false, true);

                }

            }
            if (FastbootWorker.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        #endregion

    }
}