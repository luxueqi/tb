using System;
using System.Windows.Forms;

namespace Tieba.Core.Forms
{
    public partial class getBDUSS : Form
    {
        public getBDUSS()
        {
            InitializeComponent();
        }

        private void getBDUSS_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(Conf.UPDATE_URL+"/bduss");
        }
    }
}
