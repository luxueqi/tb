using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BaiduHelper;
using System.IO;
using Tieba.Core.Forms;

namespace Tieba
{
    public partial class Bduss : Form
    {
        public Bduss()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            string bduss = textBox1.Text.Trim();

            if (bduss.Contains("BDUSS="))
            {
                // bduss = "BDUSS="+bduss;
                try
                {
                    User user = new User();
                    user.cookie = bduss;

                    if (!Common.CookToUn(ref user))
                    {
                        MessageBox.Show("无效的BDUSS", "提示");

                        return;
                    }
                    else
                    {
                        TaskForm fr2 = new TaskForm();

                        fr2.Text = "当前登陆账号:" + user.un;

                        user.tbs = HttpHelper.Tbs(bduss);
                        //  user= new User(un, "", bduss,);

                        fr2.user = user;


                        string userpath = Application.StartupPath + "\\User\\" + user.un;

                        if (!Directory.Exists(userpath))
                        {
                            Directory.CreateDirectory(userpath);
                        }


                        Common.Serialize<User>(user, userpath + "\\user.xml");
                        this.Hide();

                        fr2.ShowDialog();




                    }
                }
                catch (Exception ee)
                {

                    MessageBox.Show(ee.Message, "提示");
                }


            }
            else
            {
                MessageBox.Show("请输入正确的BDUSS", "提示");
                textBox1.Clear();
            }
        }

        private void Bduss_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
        }

        private void Bduss_Load(object sender, EventArgs e)
        {

            string[] unfiles = Directory.GetDirectories("User");

            comboBox4.Items.AddRange(unfiles);

            if (comboBox4.Items.Count > 0)
            {
                comboBox4.SelectedIndex = 0;
            }



        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmGetBduss bd = new frmGetBduss();
            bd.ShowDialog();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            User us1 = Common.readXml<User>(comboBox4.Text + "\\user");
            textBox1.Text = us1.cookie;
            this.Text = "当前账号:" + us1.un;
        }




        /* public  LoginForm lf;
         private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
         {
             if (lf==null)
             {
                 lf = new LoginForm();
             }


             lf.bdussbool = true;
             this.Hide();

             lf.Show();


         }*/

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    List<string> ls = new List<string>();
        //    ls.Add("12");
        //    List<string> lss = new List<string>();
        //    lss.AddRange(ls);
        //}
    }
}
