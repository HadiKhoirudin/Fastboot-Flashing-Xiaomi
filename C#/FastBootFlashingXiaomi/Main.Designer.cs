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
            this.DataView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RichTextBox = new System.Windows.Forms.RichTextBox();
            this.TextBoxLocation = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.ButtonBrowse = new System.Windows.Forms.Button();
            this.ButtonFlash = new System.Windows.Forms.Button();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.Label2 = new System.Windows.Forms.Label();
            this.LabelProductName = new System.Windows.Forms.Label();
            this.ComboBoxDevices = new System.Windows.Forms.ComboBox();
            this.ButtonEDL = new System.Windows.Forms.Button();
            this.ButtonSTOP = new System.Windows.Forms.Button();
            this.ButtonReadInfo = new System.Windows.Forms.Button();
            this.LabelTimer = new System.Windows.Forms.Label();
            this.CheckBox = new System.Windows.Forms.CheckBox();
            this.ButtonRebootSYS = new System.Windows.Forms.Button();
            this.ProgressBar2 = new System.Windows.Forms.ProgressBar();
            this.ProgressBar3 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).BeginInit();
            this.SuspendLayout();
            // 
            // DataView
            // 
            this.DataView.AllowUserToAddRows = false;
            this.DataView.AllowUserToDeleteRows = false;
            this.DataView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.DataView.Location = new System.Drawing.Point(12, 12);
            this.DataView.Name = "DataView";
            this.DataView.RowHeadersVisible = false;
            this.DataView.Size = new System.Drawing.Size(432, 288);
            this.DataView.TabIndex = 0;
            this.DataView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataView_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Width = 20;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Command";
            this.Column2.Items.AddRange(new object[] {
            "boot",
            "erase",
            "flash",
            "oem",
            "reboot",
            "reboot-edl"});
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Partition";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Filename                  Double Click [...]";
            this.Column4.Name = "Column4";
            this.Column4.Width = 208;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Path";
            this.Column5.Name = "Column5";
            // 
            // RichTextBox
            // 
            this.RichTextBox.BackColor = System.Drawing.Color.Black;
            this.RichTextBox.Location = new System.Drawing.Point(450, 35);
            this.RichTextBox.Name = "RichTextBox";
            this.RichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.RichTextBox.Size = new System.Drawing.Size(338, 265);
            this.RichTextBox.TabIndex = 1;
            this.RichTextBox.Text = "";
            this.RichTextBox.TextChanged += new System.EventHandler(this.RichTextBox_TextChanged);
            // 
            // TextBoxLocation
            // 
            this.TextBoxLocation.Location = new System.Drawing.Point(72, 313);
            this.TextBoxLocation.Name = "TextBoxLocation";
            this.TextBoxLocation.Size = new System.Drawing.Size(210, 20);
            this.TextBoxLocation.TabIndex = 2;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(9, 316);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(57, 13);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "Location : ";
            // 
            // ButtonBrowse
            // 
            this.ButtonBrowse.Location = new System.Drawing.Point(288, 311);
            this.ButtonBrowse.Name = "ButtonBrowse";
            this.ButtonBrowse.Size = new System.Drawing.Size(75, 23);
            this.ButtonBrowse.TabIndex = 4;
            this.ButtonBrowse.Text = "Browse";
            this.ButtonBrowse.UseVisualStyleBackColor = true;
            this.ButtonBrowse.Click += new System.EventHandler(this.ButtonBrowse_Click);
            // 
            // ButtonFlash
            // 
            this.ButtonFlash.Location = new System.Drawing.Point(369, 311);
            this.ButtonFlash.Name = "ButtonFlash";
            this.ButtonFlash.Size = new System.Drawing.Size(75, 23);
            this.ButtonFlash.TabIndex = 5;
            this.ButtonFlash.Text = "Flash";
            this.ButtonFlash.UseVisualStyleBackColor = true;
            this.ButtonFlash.Click += new System.EventHandler(this.ButtonFlash_Click);
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Location = new System.Drawing.Point(12, 342);
            this.ProgressBar1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.ProgressBar1.MarqueeAnimationSpeed = 50;
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(776, 10);
            this.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ProgressBar1.TabIndex = 6;
            this.ProgressBar1.Visible = false;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(450, 13);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(84, 13);
            this.Label2.TabIndex = 7;
            this.Label2.Text = "Product Name : ";
            // 
            // LabelProductName
            // 
            this.LabelProductName.AutoSize = true;
            this.LabelProductName.Location = new System.Drawing.Point(534, 13);
            this.LabelProductName.Name = "LabelProductName";
            this.LabelProductName.Size = new System.Drawing.Size(10, 13);
            this.LabelProductName.TabIndex = 8;
            this.LabelProductName.Text = "-";
            // 
            // ComboBoxDevices
            // 
            this.ComboBoxDevices.FormattingEnabled = true;
            this.ComboBoxDevices.Location = new System.Drawing.Point(585, 10);
            this.ComboBoxDevices.Name = "ComboBoxDevices";
            this.ComboBoxDevices.Size = new System.Drawing.Size(202, 21);
            this.ComboBoxDevices.TabIndex = 10;
            // 
            // ButtonEDL
            // 
            this.ButtonEDL.Location = new System.Drawing.Point(540, 311);
            this.ButtonEDL.Name = "ButtonEDL";
            this.ButtonEDL.Size = new System.Drawing.Size(75, 23);
            this.ButtonEDL.TabIndex = 11;
            this.ButtonEDL.Text = "Reboot EDL";
            this.ButtonEDL.UseVisualStyleBackColor = true;
            this.ButtonEDL.Click += new System.EventHandler(this.ButtonEDL_Click);
            // 
            // ButtonSTOP
            // 
            this.ButtonSTOP.Location = new System.Drawing.Point(702, 311);
            this.ButtonSTOP.Name = "ButtonSTOP";
            this.ButtonSTOP.Size = new System.Drawing.Size(75, 23);
            this.ButtonSTOP.TabIndex = 12;
            this.ButtonSTOP.Text = "STOP";
            this.ButtonSTOP.UseVisualStyleBackColor = true;
            this.ButtonSTOP.Click += new System.EventHandler(this.ButtonSTOP_Click);
            // 
            // ButtonReadInfo
            // 
            this.ButtonReadInfo.Location = new System.Drawing.Point(459, 311);
            this.ButtonReadInfo.Name = "ButtonReadInfo";
            this.ButtonReadInfo.Size = new System.Drawing.Size(75, 23);
            this.ButtonReadInfo.TabIndex = 13;
            this.ButtonReadInfo.Text = "Get Info";
            this.ButtonReadInfo.UseVisualStyleBackColor = true;
            this.ButtonReadInfo.Click += new System.EventHandler(this.ButtonReadInfo_Click);
            // 
            // LabelTimer
            // 
            this.LabelTimer.AutoSize = true;
            this.LabelTimer.BackColor = System.Drawing.SystemColors.Window;
            this.LabelTimer.Location = new System.Drawing.Point(766, 13);
            this.LabelTimer.Name = "LabelTimer";
            this.LabelTimer.Size = new System.Drawing.Size(19, 13);
            this.LabelTimer.TabIndex = 14;
            this.LabelTimer.Text = "[  ]";
            // 
            // CheckBox
            // 
            this.CheckBox.AutoSize = true;
            this.CheckBox.Location = new System.Drawing.Point(17, 17);
            this.CheckBox.Name = "CheckBox";
            this.CheckBox.Size = new System.Drawing.Size(15, 14);
            this.CheckBox.TabIndex = 15;
            this.CheckBox.UseVisualStyleBackColor = true;
            this.CheckBox.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // ButtonRebootSYS
            // 
            this.ButtonRebootSYS.Location = new System.Drawing.Point(621, 311);
            this.ButtonRebootSYS.Name = "ButtonRebootSYS";
            this.ButtonRebootSYS.Size = new System.Drawing.Size(75, 23);
            this.ButtonRebootSYS.TabIndex = 16;
            this.ButtonRebootSYS.Text = "Reboot SYS";
            this.ButtonRebootSYS.UseVisualStyleBackColor = true;
            this.ButtonRebootSYS.Click += new System.EventHandler(this.ButtonRebootSYS_Click);
            // 
            // ProgressBar2
            // 
            this.ProgressBar2.Location = new System.Drawing.Point(12, 352);
            this.ProgressBar2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.ProgressBar2.Name = "ProgressBar2";
            this.ProgressBar2.Size = new System.Drawing.Size(776, 10);
            this.ProgressBar2.TabIndex = 17;
            // 
            // ProgressBar3
            // 
            this.ProgressBar3.Location = new System.Drawing.Point(12, 342);
            this.ProgressBar3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.ProgressBar3.MarqueeAnimationSpeed = 50;
            this.ProgressBar3.Name = "ProgressBar3";
            this.ProgressBar3.Size = new System.Drawing.Size(776, 10);
            this.ProgressBar3.TabIndex = 18;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 369);
            this.Controls.Add(this.ProgressBar1);
            this.Controls.Add(this.ProgressBar3);
            this.Controls.Add(this.ProgressBar2);
            this.Controls.Add(this.ButtonRebootSYS);
            this.Controls.Add(this.CheckBox);
            this.Controls.Add(this.LabelTimer);
            this.Controls.Add(this.ButtonReadInfo);
            this.Controls.Add(this.ButtonSTOP);
            this.Controls.Add(this.ButtonEDL);
            this.Controls.Add(this.ComboBoxDevices);
            this.Controls.Add(this.LabelProductName);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.ButtonFlash);
            this.Controls.Add(this.ButtonBrowse);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.TextBoxLocation);
            this.Controls.Add(this.RichTextBox);
            this.Controls.Add(this.DataView);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fastboot Flashing Xiaomi C#";
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        internal ProgressBar ProgressBar2;
        internal DataGridViewCheckBoxColumn Column1;
        internal DataGridViewComboBoxColumn Column2;
        internal DataGridViewTextBoxColumn Column3;
        internal DataGridViewTextBoxColumn Column4;
        internal DataGridViewTextBoxColumn Column5;
        internal ProgressBar ProgressBar3;
    }
}