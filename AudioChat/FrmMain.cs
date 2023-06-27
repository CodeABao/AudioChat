using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioChat
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            webView21.Source = new Uri("https://chat.openai.com/c/ef83ba76-b6f2-4e7e-b840-cf132799fd6c");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void BtnSound_Click(object sender, EventArgs e)
        {

        }
    }
}
