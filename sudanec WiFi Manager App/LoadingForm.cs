using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sudanec_WiFi_Manager_App
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
            ControlBox = false;
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public void setMessage(string newText)
        {
            LoadingLabel.Text = newText;
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {

        }

        private void LoadingLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
