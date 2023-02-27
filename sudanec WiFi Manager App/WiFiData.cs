using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudanec_WiFi_Manager_App
{
    internal class WiFiData
    {
        public static List<string> getAllWiFiProfiles()
        {
            try
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

                List<string> resultList = new List<string>();
                string cmdOutput = cmd.StandardOutput.ReadToEnd();
                int i = 0;

                while (i > -1)
                {
                    i = cmdOutput.IndexOf(" : ");
                    if (i > -1)
                    {
                        cmdOutput = cmdOutput.Substring(i + 3);
                        resultList.Add(GetFirstLine(cmdOutput));
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not load WiFi profiles: " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<string>();
            }
        }

        public static List<string> getWiFiDetails(string wifiProfile)
        {
            try
            {
                List<string> resultList = new List<string>();
                System.Diagnostics.Process cmd = new System.Diagnostics.Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine("netsh wlan show profile \"name=" + wifiProfile + "\" key=clear");
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();

                string cmdOutput = cmd.StandardOutput.ReadToEnd();
                if (cmdOutput.IndexOf("Type                   : ") > -1)
                {
                    cmdOutput = cmdOutput.Substring(cmdOutput.IndexOf("Type                   : ") + 25);
                    resultList.Add(GetFirstLine(cmdOutput));
                }
                else resultList.Add("N/A");

                if (cmdOutput.IndexOf("Radio type             : ") > -1)
                {
                    cmdOutput = cmdOutput.Substring(cmdOutput.IndexOf("Radio type             : ") + 25);
                    resultList.Add(GetFirstLine(cmdOutput));
                }
                else resultList.Add("N/A");

                if (cmdOutput.IndexOf("Vendor extension          : ") > -1)
                {
                    cmdOutput = cmdOutput.Substring(cmdOutput.IndexOf("Vendor extension          : ") + 28);
                    resultList.Add(GetFirstLine(cmdOutput));
                }
                else resultList.Add("N/A");

                if (cmdOutput.IndexOf("Authentication         : ") > -1)
                {
                    cmdOutput = cmdOutput.Substring(cmdOutput.IndexOf("Authentication         : ") + 25);
                    resultList.Add(GetFirstLine(cmdOutput));
                }
                else resultList.Add("N/A");

                string tempCipher = "";
                if (cmdOutput.IndexOf("Cipher                 : ") > -1)
                {
                    cmdOutput = cmdOutput.Substring(cmdOutput.IndexOf("Cipher                 : ") + 25);
                    tempCipher = GetFirstLine(cmdOutput);

                    while (cmdOutput.IndexOf("Cipher                 : ") > -1)
                        {
                            cmdOutput = cmdOutput.Substring(cmdOutput.IndexOf("Cipher                 : ") + 25);
                            tempCipher = tempCipher + ", " + GetFirstLine(cmdOutput);
                        }

                    resultList.Add(tempCipher);
                }
                else resultList.Add("N/A");

                if (cmdOutput.IndexOf("Key Content            : ") > -1)
                {
                    cmdOutput = cmdOutput.Substring(cmdOutput.IndexOf("Key Content            : ") + 25);
                    resultList.Add(GetFirstLine(cmdOutput));
                }
                else resultList.Add("N/A");

                return resultList;
            } catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not get WiFi details for network " + wifiProfile + ": " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<string>();
            }
        }

        static string GetFirstLine(string text)
        {
            try
            {
                using (var reader = new StringReader(text))
                {
                    return ManageString(reader.ReadLine());
                }
            }
            catch (Exception ex)
            {
                return "N/A";
            }
        }

        static string ManageString(string text)
        {
            return text;
        }
    }
}
