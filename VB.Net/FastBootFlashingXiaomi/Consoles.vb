Imports System.ComponentModel
Imports System.IO
Module Consoles
    Public Sub Delay(ByVal dblSecs As Double)
        Now.AddSeconds(0.0000115740740740741)
        Dim dateTime As DateTime = Now.AddSeconds(0.0000115740740740741)
        Dim dateTime1 As DateTime = dateTime.AddSeconds(dblSecs)
        While DateTime.Compare(Now, dateTime1) <= 0
            Application.DoEvents()
        End While
    End Sub
    Public Function Fastboot(cmd As String, worker As BackgroundWorker, ee As DoWorkEventArgs) As String
        Console.WriteLine("Fastboot Command : " & cmd)
        Dim output As String = ""
        Dim fastBootExe As Process = New Process()
        fastBootExe.StartInfo.FileName = Application.StartupPath & "\fastboot.exe"
        fastBootExe.StartInfo.Arguments = $"{cmd}"
        fastBootExe.StartInfo.CreateNoWindow = True
        fastBootExe.StartInfo.UseShellExecute = False
        fastBootExe.StartInfo.RedirectStandardOutput = True
        fastBootExe.StartInfo.RedirectStandardError = True

        If worker.CancellationPending Then
            fastBootExe.Dispose()
            ee.Cancel = True
            Return output
        Else
            fastBootExe.Start()
            Dim readerStdError As StreamReader = fastBootExe.StandardError
            Dim readerStdOutput As StreamReader = fastBootExe.StandardError
            output = readerStdError.ReadToEnd() & readerStdOutput.ReadToEnd()
            fastBootExe.WaitForExit()
        End If
        Console.WriteLine(output)
        Return output

    End Function

End Module
