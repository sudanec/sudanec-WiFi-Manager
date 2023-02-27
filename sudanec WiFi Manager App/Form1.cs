namespace sudanec_WiFi_Manager_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            try
            {
                this.StartPosition = FormStartPosition.CenterScreen;
                InitializeComponent();
                button2.Enabled = CheckExcelPresent();
                button4.Enabled = CheckExcelPresent();
                SetTooltips();
                //Task.Run(LoadNetworksToGrid);
                LoadNetworksToGrid();
                LoadWiFisToGrid(true);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("System error when initializing the application: " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetTooltips()
        {
            try
            {
                System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                ToolTip1.SetToolTip(button1, "Opens the author's website with donate options");
                System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
                ToolTip1.SetToolTip(button2, "Exports data to Microsoft Excel (only available when Microsoft Excel is installed on the machine)");
                System.Windows.Forms.ToolTip ToolTip3 = new System.Windows.Forms.ToolTip();
                ToolTip1.SetToolTip(button4, "Exports data to Microsoft Excel (only available when Microsoft Excel is installed on the machine)");
            }
            catch { }
        }

        private bool CheckExcelPresent()
        {
            try
            {
                Type officeType = Type.GetTypeFromProgID("Excel.Application");
                if (officeType == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task LoadWiFisToGrid(bool refresh = false)
        {
            LoadingForm loadingFrm = new LoadingForm();
            try
            {
                loadingFrm.setMessage("Loading available WiFi. Please wait.");
                loadingFrm.Show();
                loadingFrm.TopMost = true;
                loadingFrm.Refresh();
                if(refresh) await NativeWifiData.RefreshAsync();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt = await Task.Run(() =>
                {
                    dt.Columns.Add("Network (SSID)", typeof(string));
                    dt.Columns.Add("Signal", typeof(string));
                    dt.Columns.Add("AP MAC address (BSSID)", typeof(string));
                    dt.Columns.Add("Link quality", typeof(string));
                    dt.Columns.Add("Band / frequency", typeof(string));
                    dt.Columns.Add("Channel", typeof(string));
                    dt.Columns.Add("BSS type", typeof(string));

                    int j;
                    List<List<string>> wifis = NativeWifiData.getAllAvailableWifi();

                    foreach (List<string> network in wifis)
                    {
                        System.Data.DataRow row1 = dt.NewRow();
                        
                        for (j = 0; j < network.Count; j++) { row1[j] = network[j]; }
                        dt.Rows.Add(row1);
                    }
                    return dt;
                });
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView2.AutoResizeColumns();
                dataGridView2.ReadOnly = true;
                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Following error occurred when loading available WiFi networks: " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loadingFrm.Close();
            }
        }

            private async Task LoadNetworksToGrid()
        {
            LoadingForm loadingFrm = new LoadingForm();
            try
            {
                loadingFrm.setMessage("Loading WiFi profiles. Please wait.");
                loadingFrm.Show();
                loadingFrm.TopMost = true;
                loadingFrm.Refresh();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt = await Task.Run(() =>
                {
                    dt.Columns.Add("Network", typeof(string));
                    dt.Columns.Add("Type", typeof(string));
                    dt.Columns.Add("Radio", typeof(string));
                    dt.Columns.Add("Vendor", typeof(string));
                    dt.Columns.Add("Auth", typeof(string));
                    dt.Columns.Add("Cipher", typeof(string));
                    dt.Columns.Add("Password", typeof(string));
                    //dt.Columns.Add("Roaming", typeof(string));
                    //dt.Columns.Add("Data Limit", typeof(string));
                    //dt.Columns.Add("Over Data Limit", typeof(string));

                    int i, j;
                    List<string> wifis = WiFiData.getAllWiFiProfiles();
                    List<string> wifiDetails = new List<string>();

                    for (i = 0; i < wifis.Count; i++)
                    {
                        System.Data.DataRow row1 = dt.NewRow();
                        wifiDetails = WiFiData.getWiFiDetails(wifis[i]);
                        row1["Network"] = wifis[i];
                        for (j = 0; j < wifiDetails.Count; j++) { row1[j + 1] = wifiDetails[j]; }
                        dt.Rows.Add(row1);
                    }
                    return dt;
                });
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.AutoResizeColumns();
                dataGridView1.ReadOnly = true;
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Following error occurred when loading known WiFi profiles: " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                loadingFrm.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            { 
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = "/c start https://www.sudanec.com/donate"
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to open default web browser: " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void copyAlltoClipboard()
        {
            try
            {
                dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                dataGridView1.SelectAll();
                DataObject dataObj = dataGridView1.GetClipboardContent();
                if (dataObj != null)
                    Clipboard.SetDataObject(dataObj);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not copy data from the table: " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OfficeTools.Export_Excel(dataGridView1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadWiFisToGrid(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OfficeTools.Export_Excel(dataGridView2);
        }
    }
}