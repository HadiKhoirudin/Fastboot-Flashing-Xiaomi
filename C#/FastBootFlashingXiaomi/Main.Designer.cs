using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FastBootFlashingXiaomi
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Main : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            DataView = new DataGridView();
            DataView.CellDoubleClick += new DataGridViewCellEventHandler(DataView_CellDoubleClick);
            RichTextBox = new RichTextBox();
            RichTextBox.TextChanged += new EventHandler(RichTextBox_TextChanged);
            TextBoxLocation = new TextBox();
            Label1 = new Label();
            ButtonBrowse = new Button();
            ButtonBrowse.Click += new EventHandler(ButtonBrowse_Click);
            ButtonFlash = new Button();
            ButtonFlash.Click += new EventHandler(ButtonFlash_Click);
            ProgressBar1 = new ProgressBar();
            Label2 = new Label();
            LabelProductName = new Label();
            ComboBoxDevices = new ComboBox();
            ButtonEDL = new Button();
            ButtonEDL.Click += new EventHandler(ButtonEDL_Click);
            ButtonSTOP = new Button();
            ButtonSTOP.Click += new EventHandler(ButtonSTOP_Click);
            ButtonReadInfo = new Button();
            ButtonReadInfo.Click += new EventHandler(ButtonReadInfo_Click);
            LabelTimer = new Label();
            CheckBox = new CheckBox();
            CheckBox.CheckedChanged += new EventHandler(CheckBox_CheckedChanged);
            ButtonRebootSYS = new Button();
            ButtonRebootSYS.Click += new EventHandler(ButtonRebootSYS_Click);
            Column1 = new DataGridViewCheckBoxColumn();
            Column2 = new DataGridViewComboBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)DataView).BeginInit();
            SuspendLayout();
            // 
            // DataView
            // 
            DataView.AllowUserToAddRows = false;
            DataView.AllowUserToDeleteRows = false;
            DataView.BackgroundColor = SystemColors.Window;
            DataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataView.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
            DataView.Location = new Point(12, 12);
            DataView.Name = "DataView";
            DataView.RowHeadersVisible = false;
            DataView.Size = new Size(432, 288);
            DataView.TabIndex = 0;
            // 
            // RichTextBox
            // 
            RichTextBox.BackColor = Color.Black;
            RichTextBox.Location = new Point(450, 35);
            RichTextBox.Name = "RichTextBox";
            RichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            RichTextBox.Size = new Size(338, 265);
            RichTextBox.TabIndex = 1;
            RichTextBox.Text = "";
            // 
            // TextBoxLocation
            // 
            TextBoxLocation.Location = new Point(72, 313);
            TextBoxLocation.Name = "TextBoxLocation";
            TextBoxLocation.Size = new Size(210, 20);
            TextBoxLocation.TabIndex = 2;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(9, 316);
            Label1.Name = "Label1";
            Label1.Size = new Size(57, 13);
            Label1.TabIndex = 3;
            Label1.Text = "Location : ";
            // 
            // ButtonBrowse
            // 
            ButtonBrowse.Location = new Point(288, 311);
            ButtonBrowse.Name = "ButtonBrowse";
            ButtonBrowse.Size = new Size(75, 23);
            ButtonBrowse.TabIndex = 4;
            ButtonBrowse.Text = "Browse";
            ButtonBrowse.UseVisualStyleBackColor = true;
            // 
            // ButtonFlash
            // 
            ButtonFlash.Location = new Point(369, 311);
            ButtonFlash.Name = "ButtonFlash";
            ButtonFlash.Size = new Size(75, 23);
            ButtonFlash.TabIndex = 5;
            ButtonFlash.Text = "Flash";
            ButtonFlash.UseVisualStyleBackColor = true;
            // 
            // ProgressBar1
            // 
            ProgressBar1.Location = new Point(12, 342);
            ProgressBar1.Name = "ProgressBar1";
            ProgressBar1.Size = new Size(776, 18);
            ProgressBar1.TabIndex = 6;
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(450, 13);
            Label2.Name = "Label2";
            Label2.Size = new Size(84, 13);
            Label2.TabIndex = 7;
            Label2.Text = "Product Name : ";
            // 
            // LabelProductName
            // 
            LabelProductName.AutoSize = true;
            LabelProductName.Location = new Point(534, 13);
            LabelProductName.Name = "LabelProductName";
            LabelProductName.Size = new Size(10, 13);
            LabelProductName.TabIndex = 8;
            LabelProductName.Text = "-";
            // 
            // ComboBoxDevices
            // 
            ComboBoxDevices.FormattingEnabled = true;
            ComboBoxDevices.Location = new Point(585, 10);
            ComboBoxDevices.Name = "ComboBoxDevices";
            ComboBoxDevices.Size = new Size(202, 21);
            ComboBoxDevices.TabIndex = 10;
            // 
            // ButtonEDL
            // 
            ButtonEDL.Location = new Point(540, 311);
            ButtonEDL.Name = "ButtonEDL";
            ButtonEDL.Size = new Size(75, 23);
            ButtonEDL.TabIndex = 11;
            ButtonEDL.Text = "Reboot EDL";
            ButtonEDL.UseVisualStyleBackColor = true;
            // 
            // ButtonSTOP
            // 
            ButtonSTOP.Location = new Point(702, 311);
            ButtonSTOP.Name = "ButtonSTOP";
            ButtonSTOP.Size = new Size(75, 23);
            ButtonSTOP.TabIndex = 12;
            ButtonSTOP.Text = "STOP";
            ButtonSTOP.UseVisualStyleBackColor = true;
            // 
            // ButtonReadInfo
            // 
            ButtonReadInfo.Location = new Point(459, 311);
            ButtonReadInfo.Name = "ButtonReadInfo";
            ButtonReadInfo.Size = new Size(75, 23);
            ButtonReadInfo.TabIndex = 13;
            ButtonReadInfo.Text = "Get Info";
            ButtonReadInfo.UseVisualStyleBackColor = true;
            // 
            // LabelTimer
            // 
            LabelTimer.AutoSize = true;
            LabelTimer.BackColor = SystemColors.Window;
            LabelTimer.Location = new Point(766, 14);
            LabelTimer.Name = "LabelTimer";
            LabelTimer.Size = new Size(19, 13);
            LabelTimer.TabIndex = 14;
            LabelTimer.Text = "[  ]";
            // 
            // CheckBox
            // 
            CheckBox.AutoSize = true;
            CheckBox.Location = new Point(17, 17);
            CheckBox.Name = "CheckBox";
            CheckBox.Size = new Size(15, 14);
            CheckBox.TabIndex = 15;
            CheckBox.UseVisualStyleBackColor = true;
            // 
            // ButtonRebootSYS
            // 
            ButtonRebootSYS.Location = new Point(621, 311);
            ButtonRebootSYS.Name = "ButtonRebootSYS";
            ButtonRebootSYS.Size = new Size(75, 23);
            ButtonRebootSYS.TabIndex = 16;
            ButtonRebootSYS.Text = "Reboot SYS";
            ButtonRebootSYS.UseVisualStyleBackColor = true;
            // 
            // Column1
            // 
            Column1.HeaderText = "";
            Column1.Name = "Column1";
            Column1.Width = 20;
            // 
            // Column2
            // 
            Column2.HeaderText = "Command";
            Column2.Items.AddRange(new object[] { "boot", "erase", "flash", "oem", "reboot", "reboot-edl" });
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.True;
            Column2.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // Column3
            // 
            Column3.HeaderText = "Partition";
            Column3.Name = "Column3";
            // 
            // Column4
            // 
            Column4.HeaderText = "Filename                  Double Click [...]";
            Column4.Name = "Column4";
            Column4.Width = 208;
            // 
            // Column5
            // 
            Column5.HeaderText = "Path";
            Column5.Name = "Column5";
            Column5.Visible = false;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(795, 371);
            Controls.Add(ButtonRebootSYS);
            Controls.Add(CheckBox);
            Controls.Add(LabelTimer);
            Controls.Add(ButtonReadInfo);
            Controls.Add(ButtonSTOP);
            Controls.Add(ButtonEDL);
            Controls.Add(ComboBoxDevices);
            Controls.Add(LabelProductName);
            Controls.Add(Label2);
            Controls.Add(ProgressBar1);
            Controls.Add(ButtonFlash);
            Controls.Add(ButtonBrowse);
            Controls.Add(Label1);
            Controls.Add(TextBoxLocation);
            Controls.Add(RichTextBox);
            Controls.Add(DataView);
            MaximizeBox = false;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fastboot Flashing Xiaomi";
            ((System.ComponentModel.ISupportInitialize)DataView).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        internal DataGridView DataView;
        internal RichTextBox RichTextBox;
        internal TextBox TextBoxLocation;
        internal Label Label1;
        internal Button ButtonBrowse;
        internal Button ButtonFlash;
        internal ProgressBar ProgressBar1;
        internal Label Label2;
        internal Label LabelProductName;
        internal ComboBox ComboBoxDevices;
        internal Button ButtonEDL;
        internal Button ButtonSTOP;
        internal Button ButtonReadInfo;
        internal Label LabelTimer;
        internal CheckBox CheckBox;
        internal Button ButtonRebootSYS;
        internal DataGridViewCheckBoxColumn Column1;
        internal DataGridViewComboBoxColumn Column2;
        internal DataGridViewTextBoxColumn Column3;
        internal DataGridViewTextBoxColumn Column4;
        internal DataGridViewTextBoxColumn Column5;
    }
}