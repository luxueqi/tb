using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tieba
{
    public partial class contentForm : Form
    {
        public contentForm()
        {
            InitializeComponent();
        }

        public ContentType content;
        public int index = 0;
        private void content_Load(object sender, EventArgs e)
        {
            textBox1.Text = content.content;

            switch (content.type)
            {
                case "白名单":
                    comboBox1.SelectedIndex = 2;
                    break;
                case "黑名单":
                    comboBox1.SelectedIndex = 3;
                    break;
                case "回复关键词":
                    comboBox1.SelectedIndex =0;
                    break;
                case "标题关键词":
                    comboBox1.SelectedIndex = 1;
                    break;
                case "信任内容":
                    comboBox1.SelectedIndex = 4;
                    break;
            }

            switch (content.iszz)
            {
                case true:
                    comboBox2.SelectedIndex = 0;
                    break;
                case false :
                    comboBox2.SelectedIndex = 1;
                    break;
            }

            switch (content.isshou)
            {
                case true:
                    comboBox3.SelectedIndex = 0;
                    break;
                case false:
                    comboBox3.SelectedIndex = 1;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TaskForm ts=this.Owner as TaskForm;

             ts.listView3.Items[index].SubItems[0].Text = textBox1.Text;

             ts.listView3.Items[index].SubItems[1].Text = comboBox1.Text;

             ts.listView3.Items[index].SubItems[2].Text = comboBox2.Text;

             if (comboBox1.Text.Contains( "关键词"))
             {
                  ts.listView3.Items[index].SubItems[3].Text = "否";
             }
             else
             {
                 ts.listView3.Items[index].SubItems[3].Text = comboBox3.Text;
             }

             MessageBox.Show("修改成功");
        }
    }
}
