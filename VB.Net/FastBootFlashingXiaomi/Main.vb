Imports System.ComponentModel
Imports System.IO
Public Class Main

    Public WithEvents FastbootWorker As New BackgroundWorker
    Public Shared Fastboot As New Fastboot

    Friend Shared SharedUI As Main
    Public WorkerTodo As String
    Public IsConnected As Boolean
    Public TodoCommand As String
    Public totalchecked As String
    Public totaldo As String
    Public totallength As Long = 0
    Public counter As Integer = 30
#Region "UI"
    Public Sub New()
        AddHandler Load, AddressOf Main_Load
        InitializeComponent()
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
    Public Function FastbootConnect() As Boolean
        counter = 30
        RichLogs("Waiting devices to connect... ", Color.White, False, False)
        ComboBoxDevices.Invoke(CType(Sub() ComboBoxDevices.Text = "", Action))
        LabelProductName.Invoke(CType(Sub() LabelProductName.Text = "-", Action))
        Dim status As Boolean = Fastboot.Wait()
        If status Then
            Fastboot.Connect()
            IsConnected = True
            ComboBoxDevices.Invoke(CType(Sub() ComboBoxDevices.Text = "Fastboot Device - " & Fastboot.GetSerialNumber(), Action))
            LabelProductName.Invoke(CType(Sub() LabelProductName.Text = Fastboot.Command("getvar:product").Payload, Action))
            Return True
        Else
            ComboBoxDevices.Invoke(CType(Sub() ComboBoxDevices.Text = "", Action))
            RichLogs("Devices Not Found!", Color.Red, False, True)
        End If
        Return False
    End Function

    Public Sub Delay(ByVal dblSecs As Double)
        Now.AddSeconds(0.0000115740740740741)
        Dim dateTime As DateTime = Now.AddSeconds(0.0000115740740740741)
        Dim dateTime1 As DateTime = dateTime.AddSeconds(dblSecs)
        While DateTime.Compare(Now, dateTime1) <= 0
            Application.DoEvents()
        End While
    End Sub
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
                        str1 = str1.Replace("fastboot %* ", "").Replace("%~dp0images\", "").Replace("pause", "")
                    Else
                        str1 = str1.Replace("fastboot %* ", "").Replace("%~dp0images\", "").Replace("pause", "")
                    End If

                    If str1 <> String.Empty Then

                        Console.WriteLine(str1)

                        Dim strArrays As String() = str1.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

                        command = strArrays(0)

                        If command <> "getvar" Then
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
    Public Sub ReadFastbootDeviceInfo()

        RichLogs("Reading Device Info : ... ", Color.White, False, True)
        RichLogs(Fastboot.Command("getvar:all").Payload, Color.White, False, True)

    End Sub
    Public Sub AllIsDone(sender As Object, e As RunWorkerCompletedEventArgs)
        RichLogs(" ", Color.Red, True, True)
        RichLogs("_______________________________________________", Color.WhiteSmoke, True, True)
        RichLogs("All Progress Completed ... ", Color.WhiteSmoke, False, True)
        If IsConnected Then
            Fastboot.Disconnect()
        End If
    End Sub
    Private Sub ButtonSTOP_Click(sender As Object, e As EventArgs) Handles ButtonSTOP.Click
        If FastbootWorker.IsBusy Then
            counter = 1
            FastbootWorker.CancelAsync()
        End If
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
        Dim Connect = FastbootConnect()
        If Connect Then
            RichLogs("Device Connected! ", Color.Lime, False, True)
            Delay(0.5)
            If WorkerTodo = "flash" Then
                Dim product = Fastboot.Command("getvar:product").Payload
                If product.Contains(LabelProductName.Text) Then
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
                                    command = strArrays(0)
                                    RichLogs(command & " ", Color.White, False, False)
                                    RichLogs(Fastboot.Command(command).Status.ToString, Color.Lime, False, True)
                                End If

                                If strArrays.Length = 2 Then
                                    command = strArrays(0)
                                    If command = "erase" Then
                                        partition = strArrays(1)
                                        RichLogs(command & " " & partition & " ", Color.White, False, False)
                                        RichLogs(Fastboot.Command(command & ":" & partition).Status.ToString, Color.Lime, False, True)
                                    ElseIf command = "boot" Then
                                        filename = strArrays(1)
                                        If File.Exists(filename) Then
                                            Fastboot.UploadData(New FileStream(filename, FileMode.Open))
                                            RichLogs(Fastboot.Command(command).Status.ToString, Color.Lime, False, True)
                                        Else
                                            RichLogs("File Doesn't Exist", Color.Yellow, False, True)
                                        End If
                                    ElseIf command = "oem" Then
                                        oem = strArrays(1)
                                        RichLogs(Fastboot.Command(command & " " & oem).Status.ToString, Color.Lime, False, True)
                                    End If
                                End If

                                If strArrays.Length = 3 Then
                                    command = strArrays(0)
                                    If command = "flash" Then
                                        partition = strArrays(1)
                                        filename = strArrays(2)
                                        If File.Exists(filename) Then
                                            RichLogs(command & " " & partition & " ", Color.White, False, False)
                                            Fastboot.UploadData(New FileStream(filename, FileMode.Open))
                                            RichLogs(Fastboot.Command(command & ":" & partition).Status.ToString, Color.Lime, False, True)
                                        Else
                                            RichLogs("File Doesn't Exist", Color.Yellow, False, True)
                                        End If
                                    ElseIf command = "erase" Then
                                        partition = strArrays(1)
                                        RichLogs(Fastboot.Command(command & ":" & partition).Status.ToString, Color.Lime, False, True)
                                    ElseIf command = "boot" Then
                                        filename = strArrays(2)
                                        RichLogs(command & " ", Color.White, False, False)
                                        If File.Exists(filename) Then
                                            Fastboot.UploadData(New FileStream(filename, FileMode.Open))
                                            RichLogs(Fastboot.Command(command).Status.ToString, Color.Lime, False, True)
                                        Else
                                            RichLogs("File Doesn't Exist", Color.Yellow, False, True)
                                        End If
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
                    Console.WriteLine("From Device : " & Fastboot.Command("getvar:product").Payload & "From Image : " & LabelProductName.Text)
                    RichLogs("Error! Missmatching image and device.", Color.Red, False, True)
                End If


            ElseIf WorkerTodo = "info" Then
                ReadFastbootDeviceInfo()

            ElseIf WorkerTodo = "reboot" Then
                RichLogs("Rebooting into Android... ", Color.White, False, False)
                RichLogs(Fastboot.Command("reboot").Status.ToString, Color.Lime, False, True)

            ElseIf WorkerTodo = "EDL" Then
                RichLogs("Rebooting into EDL Mode... ", Color.White, False, False)
                RichLogs(Fastboot.Command("reboot-edl").Status.ToString, Color.Lime, False, True)

            End If
        End If
    End Sub

#End Region

End Class
