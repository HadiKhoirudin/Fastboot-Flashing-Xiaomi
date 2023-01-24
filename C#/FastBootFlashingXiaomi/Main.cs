using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        internal static Main SharedUI;
        public string WorkerTodo;
        public bool IsConnected;
        public string TodoCommand;
        public string totalchecked;
        public string DevicesName;
        public string totaldo;
        public long totallength = 0L;
        public int counter = 30;
        public TextBox textbox = new TextBox();
        #region UI
        public Main()
        {
            FastbootWorker = new BackgroundWorker();
            InitializeComponent();
            Load += Main_Load;
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
        public void ProcessBar2(long Process, long total)
        {
            ProgressBar2.Invoke(new Action(() => ProgressBar2.Value = (int)Math.Round(Math.Round(Process * 100L / (double)total))));
        }

        #endregion


        #region Function
        public bool FastbootConnect(BackgroundWorker worker, DoWorkEventArgs ee)
        {
            IsConnected = false;
            counter = 30;
            RichLogs("Waiting Fastboot devices... ", Color.White, false, false);
            ComboBoxDevices.Invoke(new Action(() => ComboBoxDevices.Text = "Waiting devices to connect... "));
            LabelProductName.Invoke(new Action(() => LabelProductName.Text = "-"));
            bool status = Consoles.Fastboot("getvar product", worker, ee).Contains("product");
            if (status)
            {
                IsConnected = true;
                textbox.Clear();
                textbox.Text = Consoles.Fastboot("getvar product", worker, ee).Replace("product: ", "");
                LabelProductName.Invoke(new Action(() => LabelProductName.Text = textbox.Lines[0]));

                textbox.Clear();
                textbox.Text = Consoles.Fastboot("getvar serialno", worker, ee).Replace("serialno: ", "");
                ComboBoxDevices.Invoke(new Action(() => ComboBoxDevices.Text = "Fastboot Serial Devices - " + textbox.Lines[0]));
                return true;
            }
            else
            {
                ComboBoxDevices.Invoke(new Action(() => ComboBoxDevices.Text = ""));
                RichLogs("Devices Not Found!", Color.Red, false, true);
            }
            return false;
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
                            DataView.CurrentRow.Cells[4].Value = Path.Combine(new string[] { Path.GetDirectoryName(openFileDialog.FileName) });
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
                string[] lines = resultproduct.Lines;
                int num = 0;

                while (num < lines.Length)
                {
                    string textlines = lines[num];
                    strs.Add(textlines);
                    num = num + 1;
                }

                product = strs[0];
                DevicesName = product;
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
                            str1 = str1.Replace("fastboot %* ", "").Replace(@"%~dp0images\", "").Replace("pause", "").Replace("::", "").Replace("\"", "");
                        }
                        else
                        {
                            str1 = str1.Replace("fastboot %* ", "").Replace(@"%~dp0images\", "").Replace("pause", "").Replace("::", "").Replace("\"", "");
                        }

                        if (!string.IsNullOrEmpty(str1))
                        {

                            string[] strArrays = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            command = strArrays[0];

                            if (command != "getvar" && command != "echo")
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
        public void ReadFastbootDeviceInfo(BackgroundWorker worker, DoWorkEventArgs e)
        {


        }
        public void AllIsDone(object sender, RunWorkerCompletedEventArgs e)
        {
            RichLogs("_______________________________________________", Color.WhiteSmoke, true, true);
            RichLogs("All Progress Completed ... ", Color.WhiteSmoke, false, true);
            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
            ProgressBar2.Invoke(new Action(() => ProgressBar2.Value = 100));
            ProgressBar3.Invoke(new Action(() => ProgressBar3.Value = 100));
            ProcessKill();
        }
        private void ButtonSTOP_Click(object sender, EventArgs e)
        {
            if (FastbootWorker.IsBusy)
            {
                counter = 1;
                FastbootWorker.CancelAsync();
            }
            ProcessKill();
        }

        public void ProcessKill()
        {
            string[] array = new string[] { "adb", "adb.exe", "fastboot", "fastboot.exe" };

            foreach (string text in array)
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process process in processes)
                {

                    if ((process.ProcessName.ToLower() ?? "") == (text.ToLower() ?? ""))
                    {
                        process.Kill();
                        process.WaitForExit();
                        process.Dispose();
                    }
                }

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
                    string commands = "";
                    string args = "";
                    string name = "";
                    string path = "";
                    string filename = "";
                    foreach (DataGridViewRow item in DataView.Rows)
                    {
                        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(item.Cells[DataView.Columns[0].Index].Value, true, false)))
                        {

                            totalchecked = (totalchecked + 1d).ToString();

                            commands = Conversions.ToString(item.Cells[DataView.Columns[1].Index].Value);
                            args = Conversions.ToString(item.Cells[DataView.Columns[2].Index].Value);
                            name = Conversions.ToString(item.Cells[DataView.Columns[3].Index].Value);
                            path = Conversions.ToString(Operators.ConcatenateObject(item.Cells[DataView.Columns[4].Index].Value, @"\"));

                            if (!string.IsNullOrEmpty(name))
                            {
                                filename = path + name;
                            }
                            else
                            {
                                filename = "";
                            }

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
            bool Connect = FastbootConnect((BackgroundWorker)sender, e);
            if (Connect)
            {
                ProgressBar2.Invoke(new Action(() => ProgressBar2.Value = 0));
                ProgressBar3.Invoke(new Action(() => ProgressBar3.Value = 0));
                RichLogs("Device Connected! ", Color.Lime, false, true);
                Consoles.Delay(0.5d);
                if (WorkerTodo == "flash")
                {
                    textbox.Clear();
                    textbox.Text = Consoles.Fastboot("getvar product", (BackgroundWorker)sender, e).Replace("product: ", "");
                    string product = textbox.Lines[0];
                    if (product.Contains(DevicesName))
                    {
                        totaldo = 0.ToString();
                        using (var stringReader = new StringReader(TodoCommand))
                        {
                            while (stringReader.Peek() != -1)
                            {
                                string str1 = stringReader.ReadLine();
                                string command;
                                string partition;
                                string oem;
                                string filename;

                                if (!string.IsNullOrEmpty(str1))
                                {

                                    Console.WriteLine(str1);

                                    totaldo = (totaldo + 1d).ToString();

                                    Consoles.Delay(0.5d);

                                    string[] strArrays = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                    if (strArrays.Length == 1)
                                    {
                                        ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = true));
                                        command = strArrays[0];
                                        if (command == "reboot")
                                        {
                                            RichLogs("Rebooting into system  >> ", Color.White, false, false);
                                        }
                                        else if (command == "reboot-edl")
                                        {
                                            RichLogs("Rebooting into EDL Mode  >> ", Color.White, false, false);
                                        }

                                        string exec = Consoles.Fastboot(command, (BackgroundWorker)sender, e);
                                        if (exec.ToLower().Contains("okay") && exec.ToLower().Contains("finished") || exec.ToLower().Contains("finished"))
                                        {
                                            RichLogs("[OK]", Color.Lime, false, true);
                                        }
                                        else
                                        {
                                            RichLogs("[Failed]", Color.Red, false, true);
                                        }
                                        ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
                                    }

                                    if (strArrays.Length == 2)
                                    {
                                        ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = true));
                                        command = strArrays[0];
                                        if (command == "erase")
                                        {
                                            partition = strArrays[1];
                                            RichLogs("Erasing  >> " + partition + " ", Color.White, false, false);

                                            string exec = Consoles.Fastboot(command + " " + partition, (BackgroundWorker)sender, e);
                                            if (exec.ToLower().Contains("okay") && exec.ToLower().Contains("finished") || exec.ToLower().Contains("finished"))
                                            {
                                                RichLogs("[OK]", Color.Lime, false, true);
                                            }
                                            else
                                            {
                                                RichLogs("[Failed]", Color.Red, false, true);
                                            }
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
                                        }
                                        else if (command == "boot")
                                        {
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = true));
                                            filename = strArrays[1];
                                            if (File.Exists(filename))
                                            {

                                                string exec = Consoles.Fastboot("Booting  >> " + filename + " ", (BackgroundWorker)sender, e);
                                                if (exec.ToLower().Contains("okay") && exec.ToLower().Contains("finished") || exec.ToLower().Contains("finished"))
                                                {
                                                    RichLogs("[OK]", Color.Lime, false, true);
                                                }
                                                else
                                                {
                                                    RichLogs("[Failed]", Color.Red, false, true);
                                                }
                                            }

                                            else
                                            {
                                                RichLogs("File Doesn't Exist", Color.Yellow, false, true);
                                            }
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
                                        }
                                        else if (command == "oem")
                                        {
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = true));
                                            oem = strArrays[1];

                                            string exec = Consoles.Fastboot("OEM Command >> " + oem + " ", (BackgroundWorker)sender, e);
                                            if (exec.ToLower().Contains("okay") && exec.ToLower().Contains("finished") || exec.ToLower().Contains("finished"))
                                            {
                                                RichLogs("[OK]", Color.Lime, false, true);
                                            }
                                            else
                                            {
                                                RichLogs("[Failed]", Color.Red, false, true);
                                            }
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
                                        }
                                    }

                                    if (strArrays.Length == 3)
                                    {
                                        command = strArrays[0];
                                        if (command == "flash")
                                        {
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = true));
                                            partition = strArrays[1];
                                            filename = strArrays[2];
                                            if (File.Exists(filename))
                                            {
                                                RichLogs("Flashing >> " + partition + " ", Color.White, false, false);

                                                string exec = Consoles.Fastboot(command + " " + partition + " " + filename, (BackgroundWorker)sender, e);
                                                if (exec.ToLower().Contains("okay") && exec.ToLower().Contains("finished") || exec.ToLower().Contains("finished"))
                                                {
                                                    RichLogs("[OK]", Color.Lime, false, true);
                                                }
                                                else
                                                {
                                                    RichLogs("[Failed]", Color.Red, false, true);
                                                }
                                            }

                                            else
                                            {
                                                RichLogs("File Doesn't Exist", Color.Yellow, false, true);
                                            }
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
                                        }
                                        else if (command == "erase")
                                        {
                                            partition = strArrays[1];
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = true));
                                            RichLogs("Erasing >> " + partition + " ", Color.White, false, false);
                                            string exec = Consoles.Fastboot(command + " " + partition, (BackgroundWorker)sender, e);
                                            if (exec.ToLower().Contains("okay") && exec.ToLower().Contains("finished") || exec.ToLower().Contains("finished"))
                                            {
                                                RichLogs("[OK]", Color.Lime, false, true);
                                            }
                                            else
                                            {
                                                RichLogs("[Failed]", Color.Red, false, true);
                                            }
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
                                        }
                                        else if (command == "boot")
                                        {
                                            filename = strArrays[2];
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = true));
                                            RichLogs("Booting >> " + filename + " ", Color.White, false, false);
                                            if (File.Exists(filename))
                                            {

                                                string exec = Consoles.Fastboot(command + " " + filename, (BackgroundWorker)sender, e);
                                                if (exec.ToLower().Contains("okay") && exec.ToLower().Contains("finished") || exec.ToLower().Contains("finished"))
                                                {
                                                    RichLogs("[OK]", Color.Lime, false, true);
                                                }
                                                else
                                                {
                                                    RichLogs("[Failed]", Color.Red, false, true);
                                                }
                                            }

                                            else
                                            {
                                                RichLogs("File Doesn't Exist", Color.Yellow, false, true);
                                            }
                                            ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
                                        }

                                    }
                                }
                                if (Conversions.ToBoolean(FastbootWorker.CancellationPending))
                                {
                                    break;
                                }
                                ProcessBar2(Conversions.ToLong(totaldo), Conversions.ToLong(totalchecked));
                            }
                        }
                    }

                    else
                    {
                        RichLogs("Error! Missmatching image and device.", Color.Red, false, true);
                    }
                }


                else if (WorkerTodo == "info")
                {
                    ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = true));
                    RichLogs("Reading Device Info : ... ", Color.White, false, false);
                    string exec = Consoles.Fastboot("getvar all", (BackgroundWorker)sender, e).Replace("(bootloader) ", "").Replace(":", "*" + Constants.vbTab + "[ i]  ");
                    if (exec.ToLower().Contains("okay") && exec.ToLower().Contains("finished") || exec.ToLower().Contains("finished"))
                    {
                        RichLogs("[OK]", Color.Lime, false, true);
                        RichLogs(exec, Color.SkyBlue, false, true);
                    }
                    else
                    {
                        RichLogs("[Failed]", Color.Red, false, true);
                    }
                    ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
                }
                else if (WorkerTodo == "reboot")
                {
                    ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = true));
                    RichLogs("Rebooting into Android : ... ", Color.White, false, false);
                    string exec = Consoles.Fastboot("reboot", (BackgroundWorker)sender, e);
                    if (exec.ToLower().Contains("okay") && exec.ToLower().Contains("finished") || exec.ToLower().Contains("finished"))
                    {
                        RichLogs("[OK]", Color.Lime, false, true);
                    }
                    else
                    {
                        RichLogs("[Failed]", Color.Red, false, true);
                    }
                    ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
                }
                else if (WorkerTodo == "EDL")
                {
                    ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = true));
                    RichLogs("Rebooting into EDL Mode : ... ", Color.White, false, false);
                    string exec = Consoles.Fastboot("reboot-edl", (BackgroundWorker)sender, e);
                    if (exec.ToLower().Contains("okay") && exec.ToLower().Contains("finished") || exec.ToLower().Contains("finished"))
                    {
                        RichLogs("[OK]", Color.Lime, false, true);
                    }
                    else
                    {
                        RichLogs("[Failed]", Color.Red, false, true);
                    }
                    ProgressBar1.Invoke(new Action(() => ProgressBar1.Visible = false));
                }
            }
        }

        #endregion

    }
}