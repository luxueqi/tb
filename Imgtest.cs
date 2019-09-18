using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tieba
{
    public partial class Imgtest : Form
    {
        public Imgtest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                button1.Enabled = false;
                string path1 = textBox1.Text.Trim();

                string path2 = textBox2.Text.Trim();

                if (path1==""||path2=="")
                {
                    return;
                }
                //string hash1 ,hash2;
                //if (checkBox1.Checked)
                //{
                   string  hash1 = new imghash(path1,checkBox1.Checked).GetHash();
                   string hash2 = new imghash(path2, checkBox1.Checked).GetHash();
                //}
                //else
                //{
                //    hash1 = new imghash(path1).GetHash();
                //    hash2 = new imghash(path2).GetHash();
                //}
               

                int dd = imghash.CalcSimilarDegree(hash1, hash2);

                label3.Text = "result:" + dd.ToString();

               
            }
            catch (Exception ee)
            {

                label3.Text = ee.Message;

            }

            button1.Enabled = true;
        }
    }
}
