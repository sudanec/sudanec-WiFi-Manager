using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedNativeWifi;

namespace sudanec_WiFi_Manager_App
{
    internal class NativeWifiData
    {
        public static List<List<string>> getAllAvailableWifi()
        {
            try
            {
                //List<string> temp = NativeWifi.EnumerateAvailableNetworkSsids().Select(x => x.ToString()).Distinct().ToList();
                List<List<string>> returner = new List<List<string>>();
                //var temp = NativeWifi.EnumerateAvailableNetworks()
                var temp = NativeWifi.EnumerateBssNetworks()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Ssid.ToString()))
                    .OrderByDescending(x => x.SignalStrength)//SignalQuality)
                    .Distinct();
                
                if (temp is null) { return new List<List<string>>(); }

                foreach (BssNetworkPack network in temp)
                {
                    List<string> WiFi = new List<string>();
                    WiFi.Add(network.Ssid.ToString());
                    WiFi.Add(network.SignalStrength.ToString());
                    WiFi.Add(network.Bssid.ToString());
                    WiFi.Add(network.LinkQuality.ToString());
                    WiFi.Add(network.Band.ToString() + " / " + network.Frequency.ToString());
                    WiFi.Add(network.Channel.ToString());
                    WiFi.Add(network.BssType.ToString());
                    returner.Add(WiFi);
                }

                /*foreach (AvailableNetworkPack network in temp)
                {
                    //System.Windows.Forms.MessageBox.Show("macka");
                    List<string> WiFi = new List<string>();
                    WiFi.Add(network.Ssid.ToString());
                    WiFi.Add(network.SignalQuality.ToString());
                    WiFi.Add(network.CipherAlgorithm.ToString());
                    WiFi.Add(network.AuthenticationAlgorithm.ToString());
                    WiFi.Add(network.BssType.ToString());
                    WiFi.Add(network.ProfileName.ToString());
                    returner.Add(WiFi);
                }*/
                return returner;

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not get available WiFi networks: " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<List<string>>();
            }
        }

        public static Task RefreshAsync()
        {
            return NativeWifi.ScanNetworksAsync(timeout: TimeSpan.FromSeconds(10));
        }

        /*public static List<string> getWiFiDetails(string wifiProfile)
        {
            try
            {
                List<string> resultList = new List<string>();

                var availableNetwork = NativeWifi.EnumerateAvailableNetworks()
                    .Where(x => !string.IsNullOrWhiteSpace(wifiProfile))
                    .OrderByDescending(x => x.SignalQuality)
                    .FirstOrDefault();

                resultList.Add(availableNetwork.SignalQuality.ToString());

                return resultList;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not get WiFi details for available network " + wifiProfile + ": " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<string>();
            }
        }*/

    }
}
