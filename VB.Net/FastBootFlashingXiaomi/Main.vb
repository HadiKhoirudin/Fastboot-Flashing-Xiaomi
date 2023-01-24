Imports System.ComponentModel
Imports System.IO
Public Class Main

    Public WithEvents FastbootWorker As New BackgroundWorker

    Friend Shared SharedUI As Main
    Public WorkerTodo As String
    Public IsConnected As Boolean
    Public TodoCommand As String
    Public totalchecked As String
    Public DevicesName As String
    Public totaldo As String
    Public totallength As Long = 0
    Public counter As Integer = 30
    Public textbox As New TextBox()
#Region "UI"
    Public Sub New()
        InitializeComponent()
        AddHandler Load, AddressOf Main_Load
        SharedUI = Me
    End Sub

    Private Sub RichTextBox_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox.TextChanged
        RichTextBox.Invoke(Sub()
                               RichTextBox.SelectionStart = RichTextBox.Text.Length
                               RichTextBox.ScrollToCaret()
                           End Sub)
    End Sub
    Public Sub RichLogs(msg As String, colour As Color, isBold As Boolean, Optional NextLine As Boolean = False)
        RichTextBox.Invoke(Sub()
                               RichTextBox.SelectionStart = RichTextBox.Text.Length
                               Dim selectionColor As Color = RichTextBox.SelectionColor
                               RichTextBox.SelectionColor = colour
                               If isBold Then
                                   RichTextBox.SelectionFont = New Font(RichTextBox.Font, FontStyle.Bold)
                               Else
                                   RichTextBox.SelectionFont = New Font(RichTextBox.Font, FontStyle.Regular)
                               End If
                               RichTextBox.AppendText(msg)
                               RichTextBox.SelectionColor = selectionColor
                               If NextLine Then
                                   If RichTextBox.TextLength > 0 Then
                                       RichTextBox.AppendText(vbCrLf)
                                   End If
                               End If
                           End Sub)
    End Sub
    Private Sub Main_Load(sender As Object, e As EventArgs)
        RichLogs("<+++++++++++  Fastboot Flashing Xiaomi  +++++++++++>", Color.White, True, True)
        RichLogs("► Software  " & vbTab & ": ", Color.White, True, False)
        RichLogs("Fastboot Tool", Color.White, True, True)
        RichLogs("► License  " & vbTab & ": ", Color.White, True, False)
        RichLogs("Maintainer", Color.White, True, True)
        RichLogs("  =============================================", Color.White, True, True)
        RichLogs("► Websites  " & vbTab & ":  https://facebook.com/f.hadikhoir/", Color.White, True, True)
        RichLogs("  =============================================", Color.White, True, True)
        RichLogs("", Color.White, True, True)
    End Sub
    Public Sub ProcessBar1(Process As Long, total As Long)
        ProgressBar1.Invoke(New Action(Sub()
                                           ProgressBar1.Value = CInt(Math.Round((Process * 100L) / total))
                                       End Sub))
    End Sub
    Public Sub ProcessBar2(Process As Long, total As Long)
        ProgressBar2.Invoke(New Action(Sub()
                                           ProgressBar2.Value = CInt(Math.Round((Process * 100L) / total))
                                       End Sub))
    End Sub

#End Region


#Region "Function"
    Public Function FastbootConnect(worker As BackgroundWorker, ee As DoWorkEventArgs) As Boolean
        IsConnected = False
        counter = 30
        RichLogs("Waiting Fastboot devices... ", Color.White, False, False)
        ComboBoxDevices.Invoke(CType(Sub() ComboBoxDevices.Text = "Waiting devices to connect... ", Action))
        LabelProductName.Invoke(CType(Sub() LabelProductName.Text = "-", Action))
        Dim status As Boolean = Fastboot("getvar product", worker, ee).Contains("product")
        If status Then
            IsConnected = True
            textbox.Clear()
            textbox.Text = Fastboot("getvar product", worker, ee).Replace("product: ", "")
            LabelProductName.Invoke(CType(Sub() LabelProductName.Text = textbox.Lines(0), Action))

            textbox.Clear()
            textbox.Text = Fastboot("getvar serialno", worker, ee).Replace("serialno: ", "")
            ComboBoxDevices.Invoke(CType(Sub() ComboBoxDevices.Text = "Fastboot Serial Devices - " & textbox.Lines(0), Action))
            Return True
        Else
            ComboBoxDevices.Invoke(CType(Sub() ComboBoxDevices.Text = "", Action))
            RichLogs("Devices Not Found!", Color.Red, False, True)
        End If
        Return False
    End Function

    Private Sub CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox.CheckedChanged
        If DataView.Rows.Count > 0 Then
            If CheckBox.Checked Then
                For Each item As DataGridViewRow In DataView.Rows
                    item.Cells(0).Value = True
                Next
            Else
                For Each item As DataGridViewRow In DataView.Rows
                    item.Cells(0).Value = False
                Next
            End If
        End If
    End Sub
    Private Sub DataView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataView.CellDoubleClick
        If DataView.Rows.Count > 0 Then
            If e.ColumnIndex = 3 Then
                If DataView.CurrentRow.Cells(1).Value = "flash" OrElse DataView.CurrentRow.Cells(1).Value = "boot" Then
                    Dim openFileDialog As New OpenFileDialog()
                    openFileDialog.Title = "Select File Partition " + DataView.CurrentRow.Cells(2).Value
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
                    openFileDialog.FileName = "*.*"
                    openFileDialog.Filter = "ALL FILE  (*.*)|*.*"
                    openFileDialog.FilterIndex = 2
                    openFileDialog.RestoreDirectory = True
                    If openFileDialog.ShowDialog() = DialogResult.OK Then
                        DataView.CurrentRow.Cells(3).Value = openFileDialog.SafeFileName
                        DataView.CurrentRow.Cells(4).Value = Path.Combine(New String() {Path.GetDirectoryName(openFileDialog.FileName)})
                    End If
                Else
                    MsgBox("Custom file for flash and boot command!")
                End If
            End If
        End If
    End Sub
    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles ButtonBrowse.Click
        Dim openFileDialog As New OpenFileDialog() With
{
    .Title = "File",
    .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
    .FileName = "*.bat*",
    .Filter = "bat file |*.bat* ",
    .FilterIndex = 2,
    .RestoreDirectory = True
}
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            TextBoxLocation.Text = Path.Combine(New String() {Path.GetDirectoryName(openFileDialog.FileName)})

            DataView.Rows.Clear()

            Dim str As String = String.Concat(New String() {File.ReadAllText(openFileDialog.FileName)})

            Dim product As String = str.Substring(str.LastIndexOf("^product: *") + 1)
            product = product.Replace("product: *", "").Replace("""", "").Replace(" || exit /B 1", "")

            Dim resultproduct As New TextBox

            resultproduct.Text = product

            Dim strs As List(Of String) = New List(Of String)()
            Dim lines As String() = resultproduct.Lines
            Dim num As Integer = 0

            While num < CInt(lines.Length)
                Dim textlines As String = lines(num)
                strs.Add(textlines)
                num = num + 1
            End While

            product = strs(0)
            DevicesName = product
            LabelProductName.Text = product
            If str.Contains(")") Then
                str = str.Substring(str.LastIndexOf(")") + 1)
            End If

            Using stringReader As StringReader = New StringReader(str)
                While stringReader.Peek() <> -1
                    Dim str1 As String = stringReader.ReadLine()
                    Dim command As String = ""
                    Dim partition As String = ""
                    Dim filename As String = ""
                    Dim path As String = TextBoxLocation.Text & "\images"
                    If str1.Contains("||") Then
                        Dim l As Integer
                        Dim p As Integer
                        l = str1.Length
                        p = str1.IndexOf("||") - 1
                        str1 = str1.Remove(p, l - p)
                        str1 = str1.Replace("fastboot %* ", "").Replace("%~dp0images\", "").Replace("pause", "").Replace("::", "").Replace("""", "")
                    Else
                        str1 = str1.Replace("fastboot %* ", "").Replace("%~dp0images\", "").Replace("pause", "").Replace("::", "").Replace("""", "")
                    End If

                    If str1 <> String.Empty Then

                        Dim strArrays As String() = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

                        command = strArrays(0)

                        If command <> "getvar" AndAlso command <> "echo" Then
                            If strArrays.Length = 2 Then
                                partition = strArrays(1)
                            End If

                            If strArrays.Length = 3 Then
                                partition = strArrays(1)
                                filename = strArrays(2)
                            End If

                            DataView.Invoke(New Action(Sub()
                                                           DataView.Rows.Add(True, command, partition, filename, path)
                                                       End Sub))
                        End If

                    End If
                End While
            End Using
        End If

    End Sub
    Public Sub ReadFastbootDeviceInfo(worker As BackgroundWorker, e As DoWorkEventArgs)


    End Sub
    Public Sub AllIsDone(sender As Object, e As RunWorkerCompletedEventArgs)
        RichLogs("_______________________________________________", Color.WhiteSmoke, True, True)
        RichLogs("All Progress Completed ... ", Color.WhiteSmoke, False, True)
        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
        ProgressBar2.Invoke(CType(Sub() ProgressBar2.Value = 100, Action))
        ProgressBar3.Invoke(CType(Sub() ProgressBar3.Value = 100, Action))
        ProcessKill()
    End Sub
    Private Sub ButtonSTOP_Click(sender As Object, e As EventArgs) Handles ButtonSTOP.Click
        If FastbootWorker.IsBusy Then
            counter = 1
            FastbootWorker.CancelAsync()
        End If
        ProcessKill()
    End Sub

    Public Sub ProcessKill()
        Dim array As String() = New String() {"adb", "adb.exe", "fastboot", "fastboot.exe"}

        For Each text As String In array
            Dim processes As Process() = Process.GetProcesses()

            For Each process As Process In processes

                If process.ProcessName.ToLower() = text.ToLower() Then
                    process.Kill()
                    process.WaitForExit()
                    process.Dispose()
                End If
            Next

        Next
    End Sub

    Private Sub ButtonFlash_Click(sender As Object, e As EventArgs) Handles ButtonFlash.Click
        If Not FastbootWorker.IsBusy Then
            Dim flag As Boolean
            For Each item As DataGridViewRow In DataView.Rows
                If item.Cells(0).Value = True Then
                    flag = True
                End If
            Next

            If flag Then
                RichTextBox.Clear()
                WorkerTodo = "flash"

                TodoCommand = ""
                totalchecked = 0
                Dim commands As String = ""
                Dim args As String = ""
                Dim name As String = ""
                Dim path As String = ""
                Dim filename As String = ""
                For Each item As DataGridViewRow In DataView.Rows
                    If item.Cells(DataView.Columns(0).Index).Value = True Then

                        totalchecked += 1

                        commands = item.Cells(DataView.Columns(1).Index).Value
                        args = item.Cells(DataView.Columns(2).Index).Value
                        name = item.Cells(DataView.Columns(3).Index).Value
                        path = item.Cells(DataView.Columns(4).Index).Value & "\"

                        If name <> String.Empty Then
                            filename = path & name
                        Else
                            filename = ""
                        End If

                        If args = "" Then

                            TodoCommand = String.Concat(TodoCommand, commands & vbCrLf & "")

                        ElseIf filename = "" Then

                            TodoCommand = String.Concat(TodoCommand, commands & " " & args & vbCrLf & "")

                        Else

                            TodoCommand = String.Concat(TodoCommand, commands & " " & args & " " & filename & vbCrLf & "")

                        End If

                    End If
                Next

                FastbootWorker = New BackgroundWorker()
                FastbootWorker.WorkerSupportsCancellation = True
                AddHandler FastbootWorker.DoWork, AddressOf Worker
                AddHandler FastbootWorker.RunWorkerCompleted, AddressOf AllIsDone
                FastbootWorker.RunWorkerAsync()
                FastbootWorker.Dispose()
            End If
        Else
            RichLogs(" ", Color.White, True, True)
            RichLogs("Fastboot Is Running", Color.WhiteSmoke, False, True)
        End If
    End Sub
    Private Sub ButtonEDL_Click(sender As Object, e As EventArgs) Handles ButtonEDL.Click
        If Not FastbootWorker.IsBusy Then
            RichTextBox.Clear()
            WorkerTodo = "EDL"
            FastbootWorker = New BackgroundWorker()
            FastbootWorker.WorkerSupportsCancellation = True
            AddHandler FastbootWorker.DoWork, AddressOf Worker
            AddHandler FastbootWorker.RunWorkerCompleted, AddressOf AllIsDone
            FastbootWorker.RunWorkerAsync()
            FastbootWorker.Dispose()
        Else
            RichLogs(" ", Color.White, True, True)
            RichLogs("Fastboot Is Running", Color.WhiteSmoke, False, True)
        End If
    End Sub

    Private Sub ButtonReadInfo_Click(sender As Object, e As EventArgs) Handles ButtonReadInfo.Click
        If Not FastbootWorker.IsBusy Then
            RichTextBox.Clear()
            WorkerTodo = "info"
            FastbootWorker = New BackgroundWorker()
            FastbootWorker.WorkerSupportsCancellation = True
            AddHandler FastbootWorker.DoWork, AddressOf Worker
            AddHandler FastbootWorker.RunWorkerCompleted, AddressOf AllIsDone
            FastbootWorker.RunWorkerAsync()
            FastbootWorker.Dispose()
        Else
            RichLogs(" ", Color.White, True, True)
            RichLogs("Fastboot Is Running", Color.WhiteSmoke, False, True)
        End If
    End Sub

    Private Sub ButtonRebootSYS_Click(sender As Object, e As EventArgs) Handles ButtonRebootSYS.Click
        If Not FastbootWorker.IsBusy Then
            RichTextBox.Clear()
            WorkerTodo = "reboot"
            FastbootWorker = New BackgroundWorker()
            FastbootWorker.WorkerSupportsCancellation = True
            AddHandler FastbootWorker.DoWork, AddressOf Worker
            AddHandler FastbootWorker.RunWorkerCompleted, AddressOf AllIsDone
            FastbootWorker.RunWorkerAsync()
            FastbootWorker.Dispose()
        Else
            RichLogs(" ", Color.White, True, True)
            RichLogs("Fastboot Is Running", Color.WhiteSmoke, False, True)
        End If
    End Sub
    Private Sub Worker(sender As Object, e As DoWorkEventArgs)
        Dim Connect = FastbootConnect(sender, e)
        If Connect Then
            ProgressBar2.Invoke(CType(Sub() ProgressBar2.Value = 0, Action))
            ProgressBar3.Invoke(CType(Sub() ProgressBar3.Value = 0, Action))
            RichLogs("Device Connected! ", Color.Lime, False, True)
            Delay(0.5)
            If WorkerTodo = "flash" Then
                textbox.Clear()
                textbox.Text = Fastboot("getvar product", sender, e).Replace("product: ", "")
                Dim product = textbox.Lines(0)
                If product.Contains(DevicesName) Then
                    totaldo = 0
                    Using stringReader As StringReader = New StringReader(TodoCommand)
                        While stringReader.Peek() <> -1
                            Dim str1 As String = stringReader.ReadLine()
                            Dim command As String
                            Dim partition As String
                            Dim oem As String
                            Dim filename As String

                            If str1 <> String.Empty Then

                                Console.WriteLine(str1)

                                totaldo += 1

                                Delay(0.5)

                                Dim strArrays As String() = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

                                If strArrays.Length = 1 Then
                                    ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = True, Action))
                                    command = strArrays(0)
                                    If command = "reboot" Then
                                        RichLogs("Rebooting into system  >> ", Color.White, False, False)
                                    ElseIf command = "reboot-edl" Then
                                        RichLogs("Rebooting into EDL Mode  >> ", Color.White, False, False)
                                    End If

                                    Dim exec As String = Fastboot(command, sender, e)
                                    If exec.ToLower.Contains("okay") AndAlso exec.ToLower.Contains("finished") OrElse exec.ToLower.Contains("finished") Then
                                        RichLogs("[OK]", Color.Lime, False, True)
                                    Else
                                        RichLogs("[Failed]", Color.Red, False, True)
                                    End If
                                    ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
                                End If

                                If strArrays.Length = 2 Then
                                    ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = True, Action))
                                    command = strArrays(0)
                                    If command = "erase" Then
                                        partition = strArrays(1)
                                        RichLogs("Erasing  >> " & partition & " ", Color.White, False, False)

                                        Dim exec As String = Fastboot(command & " " & partition, sender, e)
                                        If exec.ToLower.Contains("okay") AndAlso exec.ToLower.Contains("finished") OrElse exec.ToLower.Contains("finished") Then
                                            RichLogs("[OK]", Color.Lime, False, True)
                                        Else
                                            RichLogs("[Failed]", Color.Red, False, True)
                                        End If
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
                                    ElseIf command = "boot" Then
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = True, Action))
                                        filename = strArrays(1)
                                        If File.Exists(filename) Then

                                            Dim exec As String = Fastboot("Booting  >> " & filename & " ", sender, e)
                                            If exec.ToLower.Contains("okay") AndAlso exec.ToLower.Contains("finished") OrElse exec.ToLower.Contains("finished") Then
                                                RichLogs("[OK]", Color.Lime, False, True)
                                            Else
                                                RichLogs("[Failed]", Color.Red, False, True)
                                            End If

                                        Else
                                            RichLogs("File Doesn't Exist", Color.Yellow, False, True)
                                        End If
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
                                    ElseIf command = "oem" Then
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = True, Action))
                                        oem = strArrays(1)

                                        Dim exec As String = Fastboot("OEM Command >> " & oem & " ", sender, e)
                                        If exec.ToLower.Contains("okay") AndAlso exec.ToLower.Contains("finished") OrElse exec.ToLower.Contains("finished") Then
                                            RichLogs("[OK]", Color.Lime, False, True)
                                        Else
                                            RichLogs("[Failed]", Color.Red, False, True)
                                        End If
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
                                    End If
                                End If

                                If strArrays.Length = 3 Then
                                    command = strArrays(0)
                                    If command = "flash" Then
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = True, Action))
                                        partition = strArrays(1)
                                        filename = strArrays(2)
                                        If File.Exists(filename) Then
                                            RichLogs("Flashing >> " & partition & " ", Color.White, False, False)

                                            Dim exec As String = Fastboot(command & " " & partition & " " & filename, sender, e)
                                            If exec.ToLower.Contains("okay") AndAlso exec.ToLower.Contains("finished") OrElse exec.ToLower.Contains("finished") Then
                                                RichLogs("[OK]", Color.Lime, False, True)
                                            Else
                                                RichLogs("[Failed]", Color.Red, False, True)
                                            End If

                                        Else
                                            RichLogs("File Doesn't Exist", Color.Yellow, False, True)
                                        End If
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
                                    ElseIf command = "erase" Then
                                        partition = strArrays(1)
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = True, Action))
                                        RichLogs("Erasing >> " & partition & " ", Color.White, False, False)
                                        Dim exec As String = Fastboot(command & " " & partition, sender, e)
                                        If exec.ToLower.Contains("okay") AndAlso exec.ToLower.Contains("finished") OrElse exec.ToLower.Contains("finished") Then
                                            RichLogs("[OK]", Color.Lime, False, True)
                                        Else
                                            RichLogs("[Failed]", Color.Red, False, True)
                                        End If
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
                                    ElseIf command = "boot" Then
                                        filename = strArrays(2)
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = True, Action))
                                        RichLogs("Booting >> " & filename & " ", Color.White, False, False)
                                        If File.Exists(filename) Then

                                            Dim exec As String = Fastboot(command & " " & filename, sender, e)
                                            If exec.ToLower.Contains("okay") AndAlso exec.ToLower.Contains("finished") OrElse exec.ToLower.Contains("finished") Then
                                                RichLogs("[OK]", Color.Lime, False, True)
                                            Else
                                                RichLogs("[Failed]", Color.Red, False, True)
                                            End If

                                        Else
                                            RichLogs("File Doesn't Exist", Color.Yellow, False, True)
                                        End If
                                        ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
                                    End If

                                End If
                            End If
                            If FastbootWorker.CancellationPending Then
                                Exit While
                            End If
                            ProcessBar2(totaldo, totalchecked)
                        End While
                    End Using

                Else
                    RichLogs("Error! Missmatching image and device.", Color.Red, False, True)
                End If


            ElseIf WorkerTodo = "info" Then
                ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = True, Action))
                RichLogs("Reading Device Info : ... ", Color.White, False, False)
                Dim exec As String = Fastboot("getvar all", sender, e).Replace("(bootloader) ", "").Replace(":", "*" & vbTab & "[ i]  ")
                If exec.ToLower.Contains("okay") AndAlso exec.ToLower.Contains("finished") OrElse exec.ToLower.Contains("finished") Then
                    RichLogs("[OK]", Color.Lime, False, True)
                    RichLogs(exec, Color.SkyBlue, False, True)
                Else
                    RichLogs("[Failed]", Color.Red, False, True)
                End If
                ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
            ElseIf WorkerTodo = "reboot" Then
                ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = True, Action))
                RichLogs("Rebooting into Android : ... ", Color.White, False, False)
                Dim exec As String = Fastboot("reboot", sender, e)
                If exec.ToLower.Contains("okay") AndAlso exec.ToLower.Contains("finished") OrElse exec.ToLower.Contains("finished") Then
                    RichLogs("[OK]", Color.Lime, False, True)
                Else
                    RichLogs("[Failed]", Color.Red, False, True)
                End If
                ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
            ElseIf WorkerTodo = "EDL" Then
                ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = True, Action))
                RichLogs("Rebooting into EDL Mode : ... ", Color.White, False, False)
                Dim exec As String = Fastboot("reboot-edl", sender, e)
                If exec.ToLower.Contains("okay") AndAlso exec.ToLower.Contains("finished") OrElse exec.ToLower.Contains("finished") Then
                    RichLogs("[OK]", Color.Lime, False, True)
                Else
                    RichLogs("[Failed]", Color.Red, False, True)
                End If
                ProgressBar1.Invoke(CType(Sub() ProgressBar1.Visible = False, Action))
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        RichLogs(Fastboot("getvar all", FastbootWorker, e), Color.Lime, False, True)
    End Sub

#End Region

End Class
