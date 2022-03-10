using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudanec_WiFi_Manager_App
{
    internal class WiFiData
    {
        public static void getAllWiFiProfiles()
        {
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("netsh wlan show profiles");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            string cmdOutput = cmd.StandardOutput.ReadToEnd();
            cmdOutput = cmdOutput.Substring(cmdOutput.IndexOf(":"));
            //System.Windows.Forms.MessageBox.Show(cmd.StandardOutput.ReadToEnd());
        }
    }
}
