using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using BaiduHelper;
using Tieba;
using System.Text.RegularExpressions;
using System.IO;
namespace Tieba
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            
        }

       
       
        User user;
        string pic = null;
       
        private void button1_Click(object sender, EventArgs e)
        {


            try
            {
                if (user==null)
                {
                    user = new User();
                }
                user.un = textBox1.Text.Trim();

                user.psd = textBox2.Text.Trim();

                string vocd = textBox3.Text.Trim();

                if (user.un != "" && user.psd != "")
                {
                    button1.Enabled =false;
                    string res = HttpHelper.Login(user.un, user.psd, vocd,pic);

                    resLogin(res);
                   
                }
                else
                {
                    MessageBox.Show("账号或密码不能为空", "提示");
                }
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.Message);
            }
            button1.Enabled = true;

        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
        }

        private void resLogin(string res)
        {
            if (res.Contains("href += \"err_no=0&"))
            {
                user.cookie = HttpHelper.loginCookie;
                user.tbs = HttpHelper.Tbs(user.cookie);
                if (!Common.CookToUn(ref  user))
                {
                    throw new Exception("获取un错误");
                  
                }
                string path = Application.StartupPath + "\\User\\user.xml";
                if (!File.Exists(path))
                {
                    Common.Serialize<User>(user, path);
                }
                else
                {
                    Common.Serialize<User>(user, Application.StartupPath + "\\User\\" + user.un + ".xml");
                }
               
                loadTaskForm();
                return;
            }
            if (res.Contains("href += \"err_no=4&"))
            {
                MessageBox.Show("用户名或密码错误");
              
            }
            if (res.Contains("href += \"err_no=6&"))
            {
                MessageBox.Show("验证码错误");
                pic = HttpHelper.Jq(res, "codeString=", "&");
               
            }
            if (res.Contains("href += \"err_no=257&"))
            {
                MessageBox.Show("请输入验证码");
                pic = HttpHelper.Jq(res, "codeString=", "&");

            }
            pictureBox1_Click(null, null);
            
        }

        private void loadTaskForm()
        {
            TaskForm fr2 = new TaskForm();

            fr2.Text = "当前登陆账号:" + user.un;

            fr2.user = user;

            fr2.Show();

            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(pic))
            {
                pictureBox1.ImageLocation = "https://passport.baidu.com/cgi-bin/genimage?" + pic;
                textBox3.Clear();
                textBox3.Enabled=true;
            }

        }
        public bool bdussbool = false;
        private void LoginForm_Load(object sender, EventArgs e)
        {
            //string res = Common.uidtointro("758104145");
            if (!bdussbool)
            {
                user = Common.readXml<User>("User\\user");
                if (user == null)
                {
                    user = new User();
                }
                textBox1.Text = user.un;

                textBox2.Text = user.psd;

                if (!string.IsNullOrEmpty(user.cookie))
                {
                    linkLabel1_LinkClicked(null, null);
                }
                
            }
           
            //pictureBox1.ImageLocation = "https://passport.baidu.com/cgi-bin/genimage?njG6506e2e4a392e22d02b9145fde0131142f82de06c50413e4";
        }
        Bduss bduss;
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (bduss==null)
            {
                bduss = new Bduss();
                bduss.lf = this;
            }

            
            this.Hide();

            bduss.ShowDialog();


            

            
        }

        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    user = Common.readXml<User>("pz\\user");

        //    textBox1.Text = user.un;

        //    textBox2.Text = user.psd;

        //}
    }
}
