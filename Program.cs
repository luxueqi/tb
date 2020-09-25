using System;
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
                //if (!Directory.Exists("pz"))
                //{
                //    Directory.CreateDirectory("pz");
                //}

                //if (!Directory.Exists("img"))
                //{
                //    Directory.CreateDirectory("img");
                //}
                string res = HttpHelper.HttpGet(Conf.UPDATE_URL+"/tb.php", System.Text.Encoding.UTF8);
                if (res != Conf.strtime)
                {
                    new System.Net.WebClient().DownloadFile(Conf.UPDATE_URL +"/tieba.zip", "tieba.zip");
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

                MessageBox.Show(ee.Message.Replace("applinzi.com", ""));
            }
            
            
           
        }
    }
}