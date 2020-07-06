using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BaiduHelper;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace Tieba
{
    public partial class TaskForm : Form
    {
        public TaskForm()
        {
            InitializeComponent();


            Control.CheckForIllegalCrossThreadCalls = false;
            // ServicePointManager.Expect100Continue = true;
        }

        public User user;


        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            writeList();
            saveContentType();
            Application.ExitThread();
        }
        Task task;
        bool stopflag;
        private void button1_Click(object sender, EventArgs e)
        {


            if (button1.Text == "启动")
            {

                Mode mode = setMode();


                if (mode.ctrkeys.Count == 0 && mode.cttkeys.Count == 0 && mode.cttxtblacks.Count == 0 && mode.cttxtwhites.Count == 0 && mode.cttxtconwhites.Count == 0)
                {
                    MessageBox.Show("请设置扫描内容", "提示");

                    return;
                }

                Common.Kw = mode.mangertb;

                Common.Fid = HttpHelper.Fid(Common.Kw);
                ServicePointManager.DefaultConnectionLimit = 128;

                task = new Task(new ListCallback(ShowList), new TxtCallback(ShowInfo), mode, (int)numericUpDown1.Value);

                task.startThread();

                button1.Text = "停止";
                richTextBox1.Focus();
                button2.Enabled = true;
                //tabControl1.TabPages[1].
                //groupBox1.Enabled = false;
                //groupBox2.Enabled = false;
                //groupBox3.Enabled = false;
                //groupBox8.Enabled = false;
            }
            else
            {
                ShowInfo("正在停止...", Color.Red);
                if (!stopflag)
                {
                    ThreadPool.QueueUserWorkItem(x =>
                    {
                        stopflag = true;
                        task.stopThread();

                        button1.Text = "启动";
                        button2.Text = "暂停";
                        button2.Enabled = false;
                        stopflag = false;
                        ShowInfo("已停止", Color.Red);
                    });
                }


                //FreeConsole();
                //groupBox1.Enabled = true ;
                //groupBox2.Enabled = true;
                //groupBox3.Enabled = true;
                //groupBox8.Enabled = true;
            }



        }



        private void Form2_Load(object sender, EventArgs e)
        {
            // user = new User();
            loaduser();
            Init();

            Pz();
            readList();
            readContentType();
            Black.txtCallback = new TxtCallback(ShowInfo);
            //Task.txtCallback = new TxtCallback(ShowInfo);
            //Task.listCallback = new ListCallback(ShowList);
            ////Task.manxunhuan = new ManualResetEvent(true);
            //Task.manua = new ManualResetEvent(true);
        }

        private void loaduser()
        {

            if (Directory.Exists("User"))
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("切换用户");
                comboBox4.Items.AddRange(Directory.GetFiles("User"));
                comboBox4.Items.Remove("User\\user.xml");
            }

        }


        private void Init()
        {

            Common.user = user;

            try
            {
                string[] tb = Common.MangerTb();

                if (tb.Length == 0)
                {
                    MessageBox.Show("当前用户没有管理的贴吧,请更换用户", "提示");
                    this.Close();
                    return;
                }
                this.Text = "当前登陆账号:" + user.un;

                mangertb.Items.Clear();

                mangertb.Items.AddRange(tb);

                mangertb.SelectedIndex = 0;

                ckmode.SelectedIndex = 0;

                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 1;

                comboBox4.SelectedIndex = 0;
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.Message, "初始化失败");
            }



        }


        private void Pz()
        {
            Mode mode = new Mode();
            if (File.Exists(Application.StartupPath + "\\pz\\peizhi.xml"))
            {
                mode = Common.DeSerialize<Mode>(Application.StartupPath + "\\pz\\peizhi.xml");
            }
            //txtblack.Text = mode.blacks.Replace("\n", "\r\n");
            ckdel.Checked = mode.isdel;
            ckblock.Checked = mode.isblock;
            //ckshou.Checked = mode.isshou;
            // ckzz.Checked = mode.iszz;
            //ckblack.Checked = mode.isblack;
            txtblockday.Text = mode.blockday.ToString();
            cklevel.Checked = mode.islevel;
            txtlevel.Text = mode.level.ToString();
            //txtftday.Text = mode.ftday.ToString();
            //ckftday.Checked = mode.isftday;
            txtscday.Text = mode.sctime.ToString();
            //txtwhite.Text = mode.white.Replace("\n", "\r\n");
            //txtkeys.Text = mode.keys.Replace("\n","\r\n");
            txtReason.Text = mode.reason;
            ckmode.Text = mode.mode;
            txtPn.Text = mode.pn.ToString();
            txtage.Text = mode.tbage.ToString();
            txtpostnum.Text = mode.postnum.ToString();
            istbage.Checked = mode.istbage;
            ispostnum.Checked = mode.ispostnum;
            ckimg.Checked = mode.isimghash;
            ckblackname.Checked = mode.isblackname;
            ckdct.Checked = mode.isimgdct;
            //ckhimg.Checked = mode.ishimg;
            cklz.Checked = mode.islz;
            ckintro.Checked = mode.isintro;
        }
        private void textNumber(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
                e.Handled = true;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string da = txtblockday.Text.Trim();
            if (da != "1" && da != "3" && da != "10")
            {
                txtblockday.Text = "1";
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            int lev = int.Parse(txtlevel.Text.Trim());

            if (lev < 1 || lev > 18)
            {
                txtlevel.Text = "1";
            }

        }

        //private void checkBox4_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ckshou.Checked)
        //    {
        //        ckdel.Checked = false;
        //        ckblock.Checked = false;

        //        ckdel.Enabled = false;

        //        ckblock.Enabled = false;
        //    }
        //    else
        //    {
        //        ckblock.Enabled = true;

        //        ckdel.Enabled = true;
        //    }
        //}

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (cklevel.Checked)
            {
                txtlevel.Enabled = true;
            }
            else
            {
                txtlevel.Enabled = false;
            }
        }

        //private void checkBox6_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ckftday.Checked)
        //    {
        //        txtftday.Enabled = true;
        //    }
        //    else
        //    {
        //        txtftday.Enabled = false;
        //    }
        //}

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TestForm fr3 = new TestForm();

            fr3.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Mode mode = setMode();

            saveContentType();

            Common.Serialize<Mode>(mode, Application.StartupPath + "\\pz\\peizhi.xml");

            MessageBox.Show("保存成功", "提示");
        }

        private Mode setMode()
        {
            Mode mode = new Mode();
            mode.setValue("isdel", ckdel.Checked);
            mode.setValue("isintro", ckintro.Checked);
            mode.setValue("isblock", ckblock.Checked);
            mode.setValue("isblackname", ckblackname.Checked);
            mode.setValue("istbage", istbage.Checked);
            mode.setValue("ispostnum", ispostnum.Checked);
            mode.setValue("islz", cklz.Checked);
            // mode.setValue("isblack", ckblack.Checked);
            mode.setValue("islevel", cklevel.Checked);
            //mode.setValue("islevel", cklevel.Checked);
            mode.setValue("isimghash", ckimg.Checked);
            mode.setValue("isimgdct", ckdct.Checked);
            mode.setValue("blockday", int.Parse(txtblockday.Text));
            mode.setValue("level", int.Parse(txtlevel.Text));
            mode.setValue("sctime", int.Parse(txtscday.Text));
            mode.setValue("postnum", int.Parse(txtpostnum.Text));

            mode.setValue("pn", int.Parse(txtPn.Text));
            mode.setValue("tbage", double.Parse(txtage.Text));
            mode.setValue("mode", ckmode.Text);
            // mode.setValue("blacks", txtblack.Text);
            // mode.setValue("keys", txtkeys.Text);
            mode.setValue("mangertb", mangertb.Text);
            mode.setValue("reason", txtReason.Text);
            //mode.setValue("ishimg", ckhimg.Checked);

            if (mode.isimghash && File.Exists("pz\\hash.txt"))
            {
                mode.setValue("localimghash", File.ReadAllLines("pz\\hash.txt"));

            }
            mode.setValue("hashdistance", (int)numericUpDown2.Value);

            getContentType(out mode.cttxtwhites, out mode.cttxtblacks, out mode.ctrkeys, out mode.cttkeys, out mode.cttxtconwhites);

            //  mode.setValue("white", txtwhite.Text);
            //object[] obj = new object[] 

            //{ mangertb.Text,
            //  ckdel.Checked,
            //  ckblock.Checked,
            //  //ckshou.Checked,
            //  ckzz.Checked,
            //  ckblack.Checked,
            //  cklevel.Checked,
            //  ckftday.Checked,
            //  txtblockday.Text,
            //  txtlevel.Text,
            //  txtftday.Text,
            //  txtscday.Text,
            //  ckmode.Text,
            //  txtblack.Text,
            //  txtkeys.Text,
            //  txtReason.Text,
            //  txtPn.Text,
            //  txtwhite.Text,
            //  istbage.Checked,
            //  ispostnum.Checked,
            //  txtage.Text,
            //  txtpostnum.Text,
            //  ckimg.Checked,
            //  ckblackname.Checked,
            //  ckdct.Checked
            //};
            //Mode mode = new Mode(obj);
            return mode;
        }

        //[DllImport("kernel32.dll")]
        //public static extern bool AllocConsole();
        //[DllImport("kernel32.dll", EntryPoint = "FreeConsole")]
        //private static extern bool FreeConsole();
        //void ShowInfo1(string str, ConsoleColor color)
        //{


        //    Console.WriteLine(DateTime.Now.ToString("[HH:mm:ss]  ") + str, Console.ForegroundColor = color);


        //}
        // object obtxt = new object();
        private void ShowInfo(string str, Color color)
        {


            //Action<string> ac = (x) =>
            //{
            lock (this)
            {
                if (richTextBox1.Lines.Length > 300)
                {
                    richTextBox1.Clear();
                }
                richTextBox1.SelectionColor = color;
                richTextBox1.AppendText(DateTime.Now.ToString("H:m:s") + "->" + str + "\n");
                // this.Update();
            }


            //};
            ////this.BeginInvoke(ac, str);
            //Invoke(ac, str);

        }
        object oblist = new object();
        private void ShowList(Log log)
        {


            //Action<int> ac=(x)=>{
            //    //
            //if (!repeatLog.Contains(log.tid))
            //{
            lock (oblist)
            {
                ListViewItem lv = new ListViewItem();
                lv.Text = (listView1.Items.Count + 1).ToString();
                lv.SubItems.Add(log.author);
                lv.SubItems.Add(log.title);
                lv.SubItems.Add(log.tid);
                lv.SubItems.Add(log.type);
                lv.SubItems.Add(log.result);
                lv.SubItems.Add(log.tbname);
                lv.SubItems.Add(log.fid);
                lv.SubItems.Add(log.uid);
                listView1.Items.Add(lv);
                //repeatLog.Add(log.tid);
            }
            //}
            //else
            //{
            //    ShowInfo(log.tid + "-->已经包含", Color.Green);
            //}


            //};

            //    //Invoke(ac,0);
            //}


        }

        private void writeList()
        {

            //if (listView1.Items.Count != 0)
            //{
            List<Log> list = new List<Log>();
            foreach (ListViewItem item in listView1.Items)
            {
                list.Add(new Log(
                    item.SubItems[0].Text,
                    item.SubItems[1].Text,
                    item.SubItems[2].Text,
                    item.SubItems[3].Text,
                    item.SubItems[4].Text,
                    item.SubItems[5].Text,
                    item.SubItems[6].Text,
                    item.SubItems[7].Text,
                    item.SubItems[8].Text));
            }
            Common.Serialize<List<Log>>(list, Application.StartupPath + "\\pz\\log.xml");
            //string log = "";
            //foreach (ListViewItem item in listView1.Items)
            //{

            //    foreach (string it in item.SubItems)
            //    {
            //        log += it + "--->\r\n";
            //    }
            //}

            //File.WriteAllText(Application.StartupPath + "\\", log);
            //}

        }

        private void readList()
        {
            if (File.Exists(Application.StartupPath + "\\pz\\log.xml"))
            {
                List<Log> logs = Common.readXml<List<Log>>("pz\\log");
                listView1.Items.Clear();
                foreach (Log item in logs)
                {
                    ShowList(item);
                }

            }


        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "暂停")
            {
                task.reSet();
                button2.Text = "继续";
            }
            else
            {
                task.set();
                button2.Text = "暂停";
            }
        }

        //private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    //LoginForm lf = new LoginForm();

        //    //lf.Show();

        //    //this.Dispose();
        //}

        //private void ckblack_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ckblack.Checked)
        //    {
        //        if (txtblack.Text.Trim()=="")
        //        {
        //            ckblack.Checked = false;
        //            MessageBox.Show("黑名单为空","提示");
        //            txtblack.Focus();
        //        }
        //    }
        //}

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                {
                    listView1.Items.Remove(item);
                }
            }
            MessageBox.Show("移除成功", "提示");
        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    if (MessageBox.Show("是否删除选中","警告",MessageBoxButtons.OKCancel)==DialogResult.OK)
        //    {
        //        ThreadPool.QueueUserWorkItem(

        //                (x) => {



        //                    button3.Enabled = false;
        //                    button4.Enabled = false;
        //                    button5.Enabled = false;
        //                    foreach (ListViewItem item in listView1.Items)
        //                    {
        //                        try
        //                        {
        //                            if (item.Checked)
        //                            {
        //                                Common.Kw = item.SubItems[6].Text;
        //                                Common.Fid = item.SubItems[7].Text;
        //                                string res=Common.Delete(item.SubItems[3].Text);
        //                                if (res == "删除成功") { listView1.Items.Remove(item); } else { ShowInfo("执行删除选中:" + res, Color.Red); }
        //                            }
        //                        }
        //                        catch (Exception ee)
        //                        {

        //                            ShowInfo("执行删除选中:"+ee.Message, Color.Red);
        //                        }

        //                    }
        //                    button3.Enabled = true;
        //                    button4.Enabled = true;
        //                    button5.Enabled = true;
        //                    MessageBox.Show("执行完毕", "提示");
        //                }

        //            );
        //    }
        //}

        private void SelectClick(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            if (MessageBox.Show("你确定要" + bt.Text, "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                //tabControl1.SelectedIndex = 0;
                //richTextBox1.Focus();
                ThreadPool.QueueUserWorkItem(

                        (x) =>
                        {

                            button3.Enabled = false;
                            button4.Enabled = false;
                            button5.Enabled = false;
                            button6.Enabled = false;

                            foreach (ListViewItem item in listView1.Items)
                            {
                                try
                                {
                                    if (item.Checked)
                                    {
                                        Common.Kw = item.SubItems[6].Text;
                                        Common.Fid = item.SubItems[7].Text;
                                        string res = "";
                                        if (bt.Text == "封删选中")
                                        {
                                            res = Common.Block(item.SubItems[1].Text.StartsWith("昵称:") ? "" : item.SubItems[1].Text, item.SubItems[8].Text, int.Parse(txtblockday.Text.Trim()), txtReason.Text);
                                            // ShowInfo("执行删封禁选中:" + res, Color.Red);
                                        }
                                        string tidpid = item.SubItems[3].Text;
                                        if (tidpid.Contains("&"))
                                        {
                                            string[] tps = tidpid.Split('&');
                                            res += Common.Del(tps[0], tps[1]);
                                        }
                                        else
                                        {
                                            res += Common.Delete(tidpid);
                                        }
                                        item.SubItems[5].Text = res;
                                        //ShowInfo("执行删封禁选中:" + res, Color.Red);
                                        //if (res == "删除成功") { listView1.Items.Remove(item); }
                                    }
                                }
                                catch (Exception ee)
                                {

                                    item.SubItems[5].Text = ee.Message;
                                }
                                item.Checked = false;

                            }
                            button3.Enabled = true;
                            button4.Enabled = true;
                            button5.Enabled = true;
                            button6.Enabled = true;
                            MessageBox.Show("执行完毕", "提示");
                        }

                    );
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否清空全部", "警告", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                listView1.Items.Clear();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {

                if (item.Checked)
                {
                    string tidpid = item.SubItems[3].Text;
                    if (tidpid.Contains("&"))
                    {
                        string[] tps = tidpid.Split('&');
                        Process.Start(Conf.HTTP_URL+"/p/" + tps[0] + "?pid=" + tps[1]);
                    }
                    else
                    {
                        Process.Start(Conf.HTTP_URL+"/p/" + tidpid);
                    }

                }

            }
        }

        private void txtPn_TextChanged(object sender, EventArgs e)
        {
            int pn = int.Parse(txtPn.Text.Trim());
            pn = pn <= 1 ? 1 : pn;
            txtPn.Text = pn.ToString();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Conf.HTTP_URL+"/f?ie=utf-8&kw=" + mangertb.Text);
        }
        ID idInfo;
        private void button8_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {


                string un = textBox1.Text.Trim();

                if (un == "")
                {
                    MessageBox.Show("请输入用户名", "提示");

                    return;
                }
                button8.Enabled = false;
                idInfo = new ID(un);

                if (idInfo.error != string.Empty)
                {
                    MessageBox.Show("获取错误:" + idInfo.error, "提示");
                    button8.Enabled = true;
                    return;
                }

                listView2.Items.Clear();
                pictureBox1.ImageLocation = idInfo.image;

                labAge.Text = "吧龄:" + idInfo.age;

                //labEmail.Text = "邮箱:" + idInfo.email;

                //labPhone.Text = "手机:" + idInfo.phone;

                labPostNum.Text = "发帖量:" + idInfo.postNum;

                labPrivate.Text = "隐藏动态:" + (idInfo.isprivate ? "是" : "否");

                labUid.Text = "UID:" + idInfo.uid;

                //labRegTime.Text = "注册时间:" + idInfo.regTime;

                labSwitchImTime.Text = "头像更换:" + idInfo.switchImageTime;

                labAssistant.Text = "小吧主:" + idInfo.assist;

                labManger.Text = "吧主:" + idInfo.manger;

                button8.Enabled = true;

            });
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView2.Items.Clear();
            if (textBox1.Text.Trim() == "") return;
            linkLabel5.LinkVisited = true;

            ThreadPool.QueueUserWorkItem(o =>
            {

                if (idInfo == null)
                {
                    idInfo = new ID(textBox1.Text.Trim());
                }
                linkLabel5.Enabled = false;
                List<Accention> lisAcc = idInfo.GetLikeTb();
                if (idInfo.error != "")
                {
                    MessageBox.Show("获取错误:" + idInfo.error, "提示");
                    linkLabel5.Enabled = true;
                    return;
                }

                foreach (Accention acc in lisAcc)
                {
                    ShowListLike(acc);
                }
                MessageBox.Show("获取完成", "提示");
                linkLabel5.Enabled = true;
            });
        }

        private void ShowListLike(Accention acc)
        {

            Action<int> ac = (x) =>
            {

                ListViewItem lv = new ListViewItem();
                lv.Text = (listView2.Items.Count + 1).ToString();
                lv.SubItems.Add(acc.tbname);
                lv.SubItems.Add(acc.level);
                lv.SubItems.Add(acc.black);
                lv.SubItems.Add(acc.intime);
                    // lock (this)
                    //{
                    listView2.Items.Add(lv);
                    //   }


                };
            Invoke(ac, 0);


        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Process.Start(Conf.HTTP_URL +"/f?kw=" + listView2.SelectedItems[0].SubItems[1].Text);
            }
            catch (Exception)
            {


            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Trim() == "") return;
                Process.Start(Conf.HTTP_URL+"/home/main?ie=utf-8&un=" + textBox1.Text.Trim());
            }
            catch (Exception)
            {


            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void ckmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ckmode.Text == "简单模式")
            {
                txtPn.Enabled = false;

                //txtftday.Enabled = false;

                txtlevel.Enabled = false;

                cklevel.Enabled = false;

                ckintro.Enabled = false;

                //ckftday.Enabled = false;

                //ckimg.Enabled = false;
            }
            else
            {
                txtPn.Enabled = true;

                //txtftday.Enabled = true;

                txtlevel.Enabled = true;

                cklevel.Enabled = true;

                ckintro.Enabled = true;

                //ckftday.Enabled = true;

                // ckimg.Enabled = true
                ;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GC.Collect();
        }

        //private void groupBox1_Enter(object sender, EventArgs e)
        //{

        //}

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (istbage.Checked)
            {
                txtage.Enabled = true;
            }
            else
            {
                txtage.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (ispostnum.Checked)
            {
                txtpostnum.Enabled = true;
            }
            else
            {
                txtpostnum.Enabled = false;
            }
        }

        private void txtage_TextChanged(object sender, EventArgs e)
        {
            double res = 0;
            if (!double.TryParse(txtage.Text.Trim(), out res))
            {

                txtage.Text = "1";
            }
        }

        //private void button10_Click(object sender, EventArgs e)
        //{


        //}



        //private void ckimg_CheckedChanged(object sender, EventArgs e)
        //{

        //}

        private void button10_Click(object sender, EventArgs e)
        {

            if (Directory.Exists("img"))
            {
                string[] paths = Directory.GetFiles("img");
                imghash ih;
                string strhash = "";
                //btimg.Enabled = false;
                button10.Enabled = false;
                for (int i = 0; i < paths.Length; i++)
                {

                    ih = new imghash(paths[i], ckdct.Checked);
                    string name = Path.GetFileName(paths[i]);
                    strhash += name + ":" + ih.GetHash() + "\r\n";
                }

                File.WriteAllText("pz\\hash.txt", strhash);
                //btimg.Enabled = true;
                button10.Enabled = true;

                MessageBox.Show("更新完成！");

            }
            else
            {
                MessageBox.Show("img文件夹不存在！");

                ckimg.Checked = false;
            }

        }

        //private void ckimg_CheckedChanged(object sender, EventArgs e)
        //{

        //}

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value > 10 || numericUpDown2.Value < 0)
            {
                numericUpDown2.Value = 5;
            }
        }

        private void ckimg_Click(object sender, EventArgs e)
        {
            if (ckimg.Checked)
            {
                MessageBox.Show("添加新图片后，请更新");
            }
        }

        //private void richTextBox1_TextChanged(object sender, EventArgs e)
        //{

        //}
        Thread blackThread;
        Black bl;
        private void button11_Click(object sender, EventArgs e)
        {


            try
            {
                //string name = textBox3.Text.Trim();

                string kw = textBox2.Text.Trim();

                if (kw == "")
                {
                    MessageBox.Show("吧名不能为空");
                    return;
                }



                if (button11.Text == "开始")
                {
                    //if (booltaskFlag)
                    //{
                    //    MessageBox.Show("请先终止其他任务");
                    //    return;
                    //}
                    if (DialogResult.No == MessageBox.Show("你确定要这么做？", "提示", MessageBoxButtons.YesNo))
                    {
                        return;
                    }
                    //booltaskFlag = true;
                    //if (name != "")
                    //{

                    //    button11.Enabled = false;
                    //    bl = new Black(kw, name, checkBox1.Checked, checkBox2.Checked);

                    //    bl.bname();

                    //    MessageBox.Show(bl.error);
                    //    button11.Enabled = true;
                    //    booltaskFlag = false;
                    //    return;
                    //}
                    bl = new Black(kw, checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, (int)numericUpDown3.Value, (int)numericUpDown4.Value, (int)numericUpDown5.Value, (int)numericUpDown6.Value, checkBox4.Checked, double.Parse(textBox3.Text.Trim()), checkBox5.Checked, textBox4.Text.Trim());

                    if (bl.error == "")
                    {
                        blackThread = new Thread(new ThreadStart(() => { bl.bnames(); }));
                        blackThread.IsBackground = true;
                        blackThread.Start();
                        button11.Text = "停止";
                        tabControl1.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show(bl.error);
                        //booltaskFlag = false;
                    }
                }
                else
                {
                    //booltaskFlag = false;

                    blackThread.Abort();
                    bl.setStop();
                    button11.Text = "开始";

                }
            }
            catch (Exception ee)
            {

                ShowInfo(ee.Message, Color.Red);
            }

        }

        private void mangertb_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = mangertb.Text;
        }




        private void getContentType(out List<ContentType> listwhite, out List<ContentType> listblack, out List<ContentType> listrkey, out List<ContentType> listtkey, out List<ContentType> listcon)
        {
            listwhite = new List<ContentType>();
            listblack = new List<ContentType>();
            listrkey = new List<ContentType>();
            listtkey = new List<ContentType>();
            listcon = new List<ContentType>();
            foreach (ListViewItem item in listView3.Items)
            {
                bool iszztemp = item.SubItems[2].Text == "是" ? true : false;

                bool isshoutemp = item.SubItems[3].Text == "是" ? true : false;
                switch (item.SubItems[1].Text)
                {
                    case "白名单":
                        listwhite.Add(new ContentType(item.SubItems[0].Text, iszztemp));
                        break;
                    case "黑名单":
                        listblack.Add(new ContentType(item.SubItems[0].Text, iszztemp));
                        break;
                    case "回复关键词":
                        listrkey.Add(new ContentType(item.SubItems[0].Text, iszztemp, isshoutemp));
                        break;
                    case "标题关键词":
                        listtkey.Add(new ContentType(item.SubItems[0].Text, iszztemp, isshoutemp));
                        break;
                    case "信任内容":
                        listcon.Add(new ContentType(item.SubItems[0].Text, iszztemp));
                        break;
                }
            }


        }

        private void saveContentType()
        {

            List<ContentType> list = new List<ContentType>();
            foreach (ListViewItem item in listView3.Items)
            {
                bool iszztemp = item.SubItems[2].Text == "是" ? true : false;

                bool isshoutemp = item.SubItems[3].Text == "是" ? true : false;
                list.Add(new ContentType(
                    item.SubItems[0].Text,
                    iszztemp,
                    isshoutemp,
                    item.SubItems[1].Text
                    ));
            }
            Common.Serialize<List<ContentType>>(list, Application.StartupPath + "\\pz\\typelog.xml");



        }
        private void readContentType()
        {
            if (File.Exists(Application.StartupPath + "\\pz\\typelog.xml"))
            {
                List<ContentType> typelog = Common.readXml<List<ContentType>>("pz\\typelog");
                listView3.Items.Clear();
                foreach (ContentType item in typelog)
                {
                    ListViewItem lv = new ListViewItem();

                    lv.Text = item.content;

                    lv.SubItems.Add(item.type);

                    lv.SubItems.Add(item.iszz == true ? "是" : "否");

                    lv.SubItems.Add(item.isshou == true ? "是" : "否");

                    listView3.Items.Add(lv);
                }

            }


        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            if (txtsetContent.Text.Trim() == "")
            {
                MessageBox.Show("内容不能为空");
                return;
            }
            ListViewItem lv = new ListViewItem();
            lv.Text = txtsetContent.Text;
            for (int i = 0; i < listView3.Items.Count; i++)
            {
                if (listView3.Items[i].SubItems[0].Text == lv.Text)
                {
                    MessageBox.Show("该项目已存在");

                    return;

                }
            }
            lv.SubItems.Add(comboBox1.Text);
            lv.SubItems.Add(comboBox2.Text);
            if (comboBox1.Text != "关键词")
            {

                lv.SubItems.Add("否");
            }
            else
            {

                lv.SubItems.Add(comboBox3.Text);

            }

            listView3.Items.Add(lv);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("确认要删除？", "提示", MessageBoxButtons.YesNo))
            {
                return;
            }
            try
            {
                listView3.Items.Remove(listView3.SelectedItems[0]);
            }
            catch (Exception)
            {


            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("确认要清空？", "提示", MessageBoxButtons.YesNo))
            {
                return;
            }
            listView3.Items.Clear();
        }

        private void 删除选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count == 0)
            {
                return;
            }

            button13_Click(null, null);
        }

        private void 修改选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count==0)
            {
                return;
            }
            contentForm cf = new contentForm();

            cf.index = listView3.SelectedIndices[0];

            bool iszz = listView3.SelectedItems[0].SubItems[2].Text == "是" ? true : false;

            bool isshou = listView3.SelectedItems[0].SubItems[3].Text == "是" ? true : false;

            cf.content = new ContentType(listView3.SelectedItems[0].SubItems[0].Text, iszz, isshou, listView3.SelectedItems[0].SubItems[1].Text);

            cf.ShowDialog(this);
        }

        private void linkLabel7_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Imgtest imgtest = new Imgtest();

            imgtest.ShowDialog();
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("确认要添加" + mangertb.Text + "吧吧务团队？", "提示", MessageBoxButtons.YesNo))
            {
                return;
            }
            try
            {
                linkLabel8.Enabled = false;
                string res = HttpHelper.HttpGet(Conf.HTTP_URL+"/bawu2/platform/listBawuTeamInfo?ie=utf-8&word=" + mangertb.Text, Encoding.GetEncoding("GBK"));

                string[] bawu = HttpHelper.P_jq(res, "class=\"user_name\" title=\"", "\"");

                string cftemp = "", partemp = "";
                for (int i = 0; i < bawu.Length; i++)
                {
                    if (!cftemp.Contains(bawu[i]))
                    {
                        //ListViewItem lv = new ListViewItem();
                        //lv.Text = bawu[i];
                        //lv.SubItems.Add("白名单");
                        //lv.SubItems.Add("否");
                        //lv.SubItems.Add("否");
                        partemp += bawu[i] + "|";
                        cftemp += bawu[i] + "\r\n";
                        //listView3.Items.Add(lv);
                    }
                }

                partemp = "^(" + partemp.TrimEnd('|') + ")$";
                ListViewItem lv = new ListViewItem();
                lv.Text = partemp;
                lv.SubItems.Add("白名单");
                lv.SubItems.Add("是");
                lv.SubItems.Add("否");
                listView3.Items.Add(lv);
                MessageBox.Show("添加完成");
            }
            catch (Exception ee)
            {

                MessageBox.Show("添加失败:" + ee.Message);
            }
            linkLabel8.Enabled = true;

        }


        frBlock fr;
        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (fr == null || fr.IsDisposed)
            {
                fr = new frBlock();
            }
            fr.tbname = textBox2.Text.Trim();
            fr.day = int.Parse(txtblockday.Text.Trim());
            fr.breson = txtReason.Text.Trim();
            if (fr.tbname == "")
            {
                MessageBox.Show("吧名不能为空");
                return;
            }
            fr.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string html = listView1.SelectedItems[0].SubItems[2].Text;
                html = html.Substring(html.IndexOf(">") + 1);
                imgweb im = new imgweb();
                im.Text = "作者：" + listView1.SelectedItems[0].SubItems[1].Text;
                im.html = html;
                im.ShowDialog();
            }
            catch (Exception)
            {


            }
        }



        private void comboBox4_DropDownClosed(object sender, EventArgs e)
        {
            if (button1.Text != "启动")
            {
                MessageBox.Show("请先结束当前任务");
            }
            else if (comboBox4.Text != "切换用户")
            {
                if (DialogResult.Yes == MessageBox.Show("是否切换用户？", "提示", MessageBoxButtons.YesNo))
                {
                    user = Common.DeSerialize<User>(comboBox4.Text);
                    Form2_Load(null, null);
                }

            }
            comboBox4.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "关键词")
            {
                comboBox2.SelectedIndex = 1;
            }
        }






        //private void button14_Click(object sender, EventArgs e)
        //{
        //    saveContentType();
        //}
        //private void button11_Click(object sender, EventArgs e)
        //{
        //    Common.Kw = "";

        //    Common.Fid = HttpHelper.Fid(Common.Kw);

        //    string res = Common.Black("萧萧");
        //}

        //private void button10_Click(object sender, EventArgs e)
        //{
        //    //imghash ih = new imghash("1.jpg");

        //    //string a=ih.GetHash();{"no":0,"data":{"ret":{"is_done":true}},"error":""}

        //    //WebClient wb = new WebClient();

        //    //ih = new imghash(wb.OpenRead("http://imgsrc.baidu.com/forum/pic/item/073d83cb39dbb6fd0bc5f29c0024ab18952b37d0.jpg"));

        //    //string b= ih.GetHash();

        //    //int s=imghash.CalcSimilarDegree(a, b);
        //    //MessageBox.Show(  s.ToString());
        //}






        //private void button8_Click(object sender, EventArgs e)
        //{
        //    ThreadPool.QueueUserWorkItem(o =>
        //    {
        //        AddPhone add = new AddPhone("186", "567", "北京");

        //        add.txtCallback = new TxtCallback(ShowInfo);

        //        add.Wzc(add.Result().ToArray());

        //        MessageBox.Show(add.defaultResult.Count.ToString());
        //    });
        //}
    }
}
