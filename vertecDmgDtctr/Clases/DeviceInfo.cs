using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace vertecDmgDtctr.Clases
{
    public class DeviceInfo
    {
        public static String StartADB(String instruccion)
        {
            string result = "";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            ProcessStartInfo info = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;

            startInfo.FileName = "C:/Program Files (x86)/Android/android-sdk/platform-tools/adb.exe ";
            startInfo.Arguments = instruccion; startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false; startInfo.RedirectStandardOutput = true;
            using (Process process = Process.Start(startInfo))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                { result = reader.ReadToEnd(); }
            }
            return result;
        }
    }
}
