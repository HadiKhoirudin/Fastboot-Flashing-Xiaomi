<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DataView = New System.Windows.Forms.DataGridView()
        Me.RichTextBox = New System.Windows.Forms.RichTextBox()
        Me.TextBoxLocation = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonBrowse = New System.Windows.Forms.Button()
        Me.ButtonFlash = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LabelProductName = New System.Windows.Forms.Label()
        Me.ComboBoxDevices = New System.Windows.Forms.ComboBox()
        Me.ButtonEDL = New System.Windows.Forms.Button()
        Me.ButtonSTOP = New System.Windows.Forms.Button()
        Me.ButtonReadInfo = New System.Windows.Forms.Button()
        Me.LabelTimer = New System.Windows.Forms.Label()
        Me.CheckBox = New System.Windows.Forms.CheckBox()
        Me.ButtonRebootSYS = New System.Windows.Forms.Button()
        Me.Column1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.DataView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataView
        '
        Me.DataView.AllowUserToAddRows = False
        Me.DataView.AllowUserToDeleteRows = False
        Me.DataView.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5})
        Me.DataView.Location = New System.Drawing.Point(12, 12)
        Me.DataView.Name = "DataView"
        Me.DataView.RowHeadersVisible = False
        Me.DataView.Size = New System.Drawing.Size(432, 288)
        Me.DataView.TabIndex = 0
        '
        'RichTextBox
        '
        Me.RichTextBox.BackColor = System.Drawing.Color.Black
        Me.RichTextBox.Location = New System.Drawing.Point(450, 35)
        Me.RichTextBox.Name = "RichTextBox"
        Me.RichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.RichTextBox.Size = New System.Drawing.Size(338, 265)
        Me.RichTextBox.TabIndex = 1
        Me.RichTextBox.Text = ""
        '
        'TextBoxLocation
        '
        Me.TextBoxLocation.Location = New System.Drawing.Point(72, 313)
        Me.TextBoxLocation.Name = "TextBoxLocation"
        Me.TextBoxLocation.Size = New System.Drawing.Size(210, 20)
        Me.TextBoxLocation.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 316)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Location : "
        '
        'ButtonBrowse
        '
        Me.ButtonBrowse.Location = New System.Drawing.Point(288, 311)
        Me.ButtonBrowse.Name = "ButtonBrowse"
        Me.ButtonBrowse.Size = New System.Drawing.Size(75, 23)
        Me.ButtonBrowse.TabIndex = 4
        Me.ButtonBrowse.Text = "Browse"
        Me.ButtonBrowse.UseVisualStyleBackColor = True
        '
        'ButtonFlash
        '
        Me.ButtonFlash.Location = New System.Drawing.Point(369, 311)
        Me.ButtonFlash.Name = "ButtonFlash"
        Me.ButtonFlash.Size = New System.Drawing.Size(75, 23)
        Me.ButtonFlash.TabIndex = 5
        Me.ButtonFlash.Text = "Flash"
        Me.ButtonFlash.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 342)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(776, 18)
        Me.ProgressBar1.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(450, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Product Name : "
        '
        'LabelProductName
        '
        Me.LabelProductName.AutoSize = True
        Me.LabelProductName.Location = New System.Drawing.Point(534, 13)
        Me.LabelProductName.Name = "LabelProductName"
        Me.LabelProductName.Size = New System.Drawing.Size(10, 13)
        Me.LabelProductName.TabIndex = 8
        Me.LabelProductName.Text = "-"
        '
        'ComboBoxDevices
        '
        Me.ComboBoxDevices.FormattingEnabled = True
        Me.ComboBoxDevices.Location = New System.Drawing.Point(585, 10)
        Me.ComboBoxDevices.Name = "ComboBoxDevices"
        Me.ComboBoxDevices.Size = New System.Drawing.Size(202, 21)
        Me.ComboBoxDevices.TabIndex = 10
        '
        'ButtonEDL
        '
        Me.ButtonEDL.Location = New System.Drawing.Point(540, 311)
        Me.ButtonEDL.Name = "ButtonEDL"
        Me.ButtonEDL.Size = New System.Drawing.Size(75, 23)
        Me.ButtonEDL.TabIndex = 11
        Me.ButtonEDL.Text = "Reboot EDL"
        Me.ButtonEDL.UseVisualStyleBackColor = True
        '
        'ButtonSTOP
        '
        Me.ButtonSTOP.Location = New System.Drawing.Point(702, 311)
        Me.ButtonSTOP.Name = "ButtonSTOP"
        Me.ButtonSTOP.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSTOP.TabIndex = 12
        Me.ButtonSTOP.Text = "STOP"
        Me.ButtonSTOP.UseVisualStyleBackColor = True
        '
        'ButtonReadInfo
        '
        Me.ButtonReadInfo.Location = New System.Drawing.Point(459, 311)
        Me.ButtonReadInfo.Name = "ButtonReadInfo"
        Me.ButtonReadInfo.Size = New System.Drawing.Size(75, 23)
        Me.ButtonReadInfo.TabIndex = 13
        Me.ButtonReadInfo.Text = "Get Info"
        Me.ButtonReadInfo.UseVisualStyleBackColor = True
        '
        'LabelTimer
        '
        Me.LabelTimer.AutoSize = True
        Me.LabelTimer.BackColor = System.Drawing.SystemColors.Window
        Me.LabelTimer.Location = New System.Drawing.Point(766, 14)
        Me.LabelTimer.Name = "LabelTimer"
        Me.LabelTimer.Size = New System.Drawing.Size(19, 13)
        Me.LabelTimer.TabIndex = 14
        Me.LabelTimer.Text = "[  ]"
        '
        'CheckBox
        '
        Me.CheckBox.AutoSize = True
        Me.CheckBox.Location = New System.Drawing.Point(17, 17)
        Me.CheckBox.Name = "CheckBox"
        Me.CheckBox.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox.TabIndex = 15
        Me.CheckBox.UseVisualStyleBackColor = True
        '
        'ButtonRebootSYS
        '
        Me.ButtonRebootSYS.Location = New System.Drawing.Point(621, 311)
        Me.ButtonRebootSYS.Name = "ButtonRebootSYS"
        Me.ButtonRebootSYS.Size = New System.Drawing.Size(75, 23)
        Me.ButtonRebootSYS.TabIndex = 16
        Me.ButtonRebootSYS.Text = "Reboot SYS"
        Me.ButtonRebootSYS.UseVisualStyleBackColor = True
        '
        'Column1
        '
        Me.Column1.HeaderText = ""
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 20
        '
        'Column2
        '
        Me.Column2.HeaderText = "Command"
        Me.Column2.Items.AddRange(New Object() {"boot", "erase", "flash", "oem", "reboot", "reboot-edl"})
        Me.Column2.Name = "Column2"
        Me.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'Column3
        '
        Me.Column3.HeaderText = "Partition"
        Me.Column3.Name = "Column3"
        '
        'Column4
        '
        Me.Column4.HeaderText = "Filename                  Double Click [...]"
        Me.Column4.Name = "Column4"
        Me.Column4.Width = 208
        '
        'Column5
        '
        Me.Column5.HeaderText = "Path"
        Me.Column5.Name = "Column5"
        Me.Column5.Visible = False
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(795, 371)
        Me.Controls.Add(Me.ButtonRebootSYS)
        Me.Controls.Add(Me.CheckBox)
        Me.Controls.Add(Me.LabelTimer)
        Me.Controls.Add(Me.ButtonReadInfo)
        Me.Controls.Add(Me.ButtonSTOP)
        Me.Controls.Add(Me.ButtonEDL)
        Me.Controls.Add(Me.ComboBoxDevices)
        Me.Controls.Add(Me.LabelProductName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.ButtonFlash)
        Me.Controls.Add(Me.ButtonBrowse)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBoxLocation)
        Me.Controls.Add(Me.RichTextBox)
        Me.Controls.Add(Me.DataView)
        Me.MaximizeBox = False
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Fastboot Flashing Xiaomi"
        CType(Me.DataView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DataView As DataGridView
    Friend WithEvents RichTextBox As RichTextBox
    Friend WithEvents TextBoxLocation As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ButtonBrowse As Button
    Friend WithEvents ButtonFlash As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Label2 As Label
    Friend WithEvents LabelProductName As Label
    Friend WithEvents ComboBoxDevices As ComboBox
    Friend WithEvents ButtonEDL As Button
    Friend WithEvents ButtonSTOP As Button
    Friend WithEvents ButtonReadInfo As Button
    Friend WithEvents LabelTimer As Label
    Friend WithEvents CheckBox As CheckBox
    Friend WithEvents ButtonRebootSYS As Button
    Friend WithEvents Column1 As DataGridViewCheckBoxColumn
    Friend WithEvents Column2 As DataGridViewComboBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents Column5 As DataGridViewTextBoxColumn
End Class
