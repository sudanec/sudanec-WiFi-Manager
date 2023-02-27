namespace sudanec_WiFi_Manager_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            try
            { 
                InitializeComponent();
                button2.Enabled = CheckExcelPresent();
                SetTooltips();
                //Task.Run(LoadNetworksToGrid);
                LoadNetworksToGrid();
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

        private void LoadNetworksToGrid()
        {
            try
            {
                System.Threading.Thread.Sleep(5000);
                System.Data.DataTable dt = new System.Data.DataTable();
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
                    for(j = 0; j < wifiDetails.Count; j++) { row1[j+1] = wifiDetails[j]; }
                    dt.Rows.Add(row1);
                }

                //var ctl = dataGridView1.AsyncState as System.Windows.Forms.Control;
                //if (ctl != null && ctl.InvokeRequired)
                //{
                //    ctl.Invoke(new Action<IAsyncResult>(LoadNetworksToGrid), dataGridView1);
                //} else
                //{

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.AutoResizeColumns();
                    dataGridView1.ReadOnly = true;
                    dataGridView1.DataSource = dt;
                //}
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Following error occurred when loading known WiFi profiles: " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                copyAlltoClipboard();
                Microsoft.Office.Interop.Excel.Application xlexcel;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                xlexcel = new Microsoft.Office.Interop.Excel.Application();
                xlexcel.ScreenUpdating = false;
                xlexcel.Visible = true;
                xlWorkBook = xlexcel.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
                xlWorkSheet.Columns[1].Delete();
                xlWorkSheet.Columns.AutoFit();
                xlWorkSheet.get_Range("A1").Select();
                xlexcel.ScreenUpdating = true;

                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.CurrentCell = dataGridView1[1, 0];
                }
            } catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("An error occurred when exporting to MS Excel: " + ex.Message, "sudanec WiFi Manager .::. Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}