using System;
using System.Windows.Forms;

namespace Tieba.Core.Forms
{
    public partial class frmGetBduss : Form
    {
        public frmGetBduss()
        {
            InitializeComponent();
        }

        private void getBDUSS_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(Conf.UPDATE_URL+"/bduss");
        }
    }
}
