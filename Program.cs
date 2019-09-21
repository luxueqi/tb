using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BaiduHelper;
using System.IO;
namespace Tieba
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                if (!Directory.Exists("User"))
                {
                    Directory.CreateDirectory("User");
                }
                if (!Directory.Exists("pz"))
                {
                    Directory.CreateDirectory("pz");
                }

                if (!Directory.Exists("img"))
                {
                    Directory.CreateDirectory("img");
                }
                string res = HttpHelper.HttpGet("http://luxueqi.sinaapp.com/tb.php", System.Text.Encoding.UTF8);
                if (res != "201909223")
                {
                    new System.Net.WebClient().DownloadFile("http://luxueqi.sinaapp.com/tieba.zip", "tieba.zip");
                    MessageBox.Show("下载更新完成tieba.zip");

                }
                else
                {
                    //Application.Run(new Bduss());

                    Application.Run(new Bduss());
                }
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.Message.Replace("luxueqi","").Replace("sinaapp.com",""));
            }
            
            
           
        }
    }
}