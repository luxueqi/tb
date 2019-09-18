﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
namespace Tieba
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox2.Enabled = false;
                textBox3.Enabled = true;
            }
            else
            {
                textBox2.Enabled = true;
                textBox3.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string par = textBox1.Text.Trim();

            Regex rg = new Regex(par);

            if (radioButton1.Checked)
            {
                string con = textBox2.Text.Trim();

                if (rg.IsMatch(con))
                {
                    MessageBox.Show("该内容可匹配:"+rg.Match(con).Value, "提示");
                }
                else
                {
                    MessageBox.Show("该内容无法匹配", "提示");
                }
            }
            else if (radioButton2.Checked)
            {
                try
                {
                    Thread th = new Thread(new ThreadStart(delegate
                    {
                        TbInfo tbinfo = new TbInfo(textBox3.Text.Trim());
                        textBox4.Clear();
                        textBox4.AppendText("匹配到的标题如下：\r\n\r\n");
                        int i = 0;
                        foreach (string item in tbinfo.Titles)
                        {
                            if (rg.IsMatch(item))
                            {
                                i++;
                                textBox4.AppendText(i+":"+item + "\r\n\r\n");
                            }
                          
                        }

                        button1.Enabled = true;
                    }));
                    th.IsBackground = true;
                    th.Start();
                    button1.Enabled = false;
                }
                catch (Exception ee)
                {
                    
                    textBox4.AppendText(ee.Message+"\r\n");
                }


            }
            else
            {
                //try
                //{
                    Thread th = new Thread(new ThreadStart(delegate
                    {
                        try
                        {
                            Title title = new Title(textBox5.Text.Trim());
                            textBox4.Clear();
                            textBox4.AppendText("匹配情况如下：\r\n\r\n");
                            int i = 0;
                            foreach (string item in title.Content)
                            {

                                i++;
                                if (rg.IsMatch(item))
                                {

                                    textBox4.AppendText(i + ":" + item + "\r\n\r\n");
                                }
                                else
                                {
                                    textBox4.AppendText(i + "->未匹配->" + item + "\r\n\r\n");
                                }

                            }
                        }
                        catch (Exception ee )
                        {

                            textBox4.AppendText(ee.Message + "\r\n");
                        }
                       

                        button1.Enabled = true;
                    }));
                    th.IsBackground = true;
                    th.Start();
                    button1.Enabled = false;
                //}
                //catch (Exception ee)
                //{

                //    textBox4.AppendText(ee.Message + "\r\n");
                //}
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox2.Enabled = false;
                textBox5.Enabled = true;
            }
            else
            {
                textBox2.Enabled = true;
                textBox5.Enabled = false;
            }

        }
    }
}
