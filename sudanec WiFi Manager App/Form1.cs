namespace sudanec_WiFi_Manager_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            loadNetworksToGrid();
            WiFiData.getWiFiData();
        }

        private void loadNetworksToGrid()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Network", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Radio", typeof(string));
            dt.Columns.Add("Vendor", typeof(string));
            dt.Columns.Add("Auth", typeof(string));
            dt.Columns.Add("Cipher", typeof(string));
            dt.Columns.Add("Password", typeof(string));
            dt.Columns.Add("Roaming", typeof(string));
            dt.Columns.Add("Data Limit", typeof(string));
            dt.Columns.Add("Over Data Limit", typeof(string));

            System.Data.DataRow row1 = dt.NewRow();
            row1["Network"] = "Test";
            dt.Rows.Add(row1);

            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            dataGridView1.ReadOnly = true;
            dataGridView1.DataSource = dt;
            //dataGridView1.DataBind();
        }
    }
}