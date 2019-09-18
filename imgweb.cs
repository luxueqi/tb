using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tieba
{
    public partial class imgweb : Form
    {
        public imgweb()
        {
            InitializeComponent();
        }
        public string html = "";
        private void imgweb_Load(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = html;
        }
    }
}
