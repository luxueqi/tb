using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BaiduHelper;
using System.Threading;
using System.IO;
using System.Diagnostics;
namespace Tieba
{
    public partial class frBlock : Form
    {
        public frBlock()
        {
            InitializeComponent();
        }
        public string tbname,breson;
        public int day = 1;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim()=="")
            {
                return;
            }

            string[] names = textBox1.Lines;

            
            Thread tt=new Thread(new ThreadStart(()=>{

                try
                {
                    string fid = HttpHelper.Fid(tbname);
                    button1.Enabled = false;
                    do
                    {
                        string log = "";
                        for (int i = 0; i < names.Length; i++)
                        {
                            try
                            {
                                string name = names[i].Trim();

                                string res="";

                                if (name.StartsWith("p:"))
                                {


                                    res = Common.wyblock(name.Replace("p:",""),day,breson,fid);
                                }else if(name.StartsWith("u:"))
                                {
                                    res = Common.Block("", name.Replace("u:", ""), day, breson, tbname, fid);
                                }
                                else
                                {
                                     res = Common.Block(name, "", day, breson, tbname, fid);


                                }


                                label1.Text = (i + 1).ToString() + "/" + names.Length + "-->" + res;
                               
                            }
                            catch (Exception ee)
                            {

                                label1.Text = (i + 1).ToString() + "/" + names.Length + "-->" + ee.Message;
                            }
                            log +=names[i]+"->"+ label1.Text + "\r\n";
                            Thread.Sleep(1000);

                        }
                       
                        File.AppendAllText("block.log",DateTime.Now.ToShortDateString()+":\r\n"+ log);
                      
                        if (checkBox1.Checked)
                        {
                            label1.Text = "下次执行时间->00:30:00";
                            TimeSpan ts = Convert.ToDateTime(DateTime.Now.AddDays(1.0).ToString("yyyy-MM-dd 00:30:00")) - DateTime.Now;
                            Thread.Sleep(ts);
                           
                        }
                    } while (checkBox1.Checked);
                }
                catch (Exception ee)
                {

                    MessageBox.Show("初始失败:" + ee.Message);
                }

                button1.Enabled = true;
            }));

            tt.IsBackground = true;

            tt.Start();
        }

        private void frBlock_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (button1.Enabled==false )
            {
                DialogResult rg = MessageBox.Show("操作:yes隐藏后台,no退出(停止循环)", "提示", MessageBoxButtons.YesNo);
                if (rg == DialogResult.Yes)
                {
                    this.Hide();
                    e.Cancel = true;
                }
            }
           
           
        }

        
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (File.Exists("block.log"))
            {
                Process.Start("block.log");
            }
           
        }
    }
}
