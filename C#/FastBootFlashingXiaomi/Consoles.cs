using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace FastBootFlashingXiaomi
{
    static class Consoles
    {
        public static void Delay(double dblSecs)
        {
            DateTime.Now.AddSeconds(0.0000115740740740741d);
            var dateTime = DateTime.Now.AddSeconds(0.0000115740740740741d);
            var dateTime1 = dateTime.AddSeconds(dblSecs);
            while (DateTime.Compare(DateTime.Now, dateTime1) <= 0)
                Application.DoEvents();
        }
        public static string Fastboot(string cmd, BackgroundWorker worker, DoWorkEventArgs ee)
        {
            Console.WriteLine("Fastboot Command : " + cmd);
            string output = "";
            var fastBootExe = new Process();
            fastBootExe.StartInfo.FileName = Application.StartupPath + @"\fastboot.exe";
            fastBootExe.StartInfo.Arguments = $"{cmd}";
            fastBootExe.StartInfo.CreateNoWindow = true;
            fastBootExe.StartInfo.UseShellExecute = false;
            fastBootExe.StartInfo.RedirectStandardOutput = true;
            fastBootExe.StartInfo.RedirectStandardError = true;

            if (worker.CancellationPending)
            {
                fastBootExe.Dispose();
                ee.Cancel = true;
                return output;
            }
            else
            {
                fastBootExe.Start();
                var readerStdError = fastBootExe.StandardError;
                var readerStdOutput = fastBootExe.StandardError;
                output = readerStdError.ReadToEnd() + readerStdOutput.ReadToEnd();
                fastBootExe.WaitForExit();
            }
            Console.WriteLine(output);
            return output;

        }

    }
}