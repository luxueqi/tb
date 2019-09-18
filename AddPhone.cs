using System;
using System.Collections.Generic;
using System.Text;
using BaiduHelper;
using System.Threading;
using System.Text.RegularExpressions;
using System.Drawing;
namespace Tieba
{
   
   //public delegate void TxtCallback(string str, Color color);

   public class AddPhone
    {

       private string lphone;

       private string rphone;

       private string area;

       private List<string> resPhone = new List<string>();

       private string strPhone;

       /// <summary>
       /// 记录线程是否执行完毕
       /// </summary>
       private int flagThread = 0;
       
      public AddPhone() { }
       /// <summary>
       /// 初始化
       /// </summary>
       /// <param name="lphone">号码前缀</param>
       /// <param name="rphone">号码后缀</param>
       /// <param name="area">地区</param>
       public AddPhone(string lphone, string rphone, string area = "")
       {

           this.lphone = lphone;

           this.rphone = rphone;

           this.area = area;
          
           
       }
       private int ThreadCount = 20;
       /// <summary>
       /// 生成手机号
       /// </summary>
       /// <returns></returns>
       public string CreatePhone()
       {
           int end = 20;
           if (area == "")
           {
               ThreadTask();

           }
           else
           {
               try
               {
                   string res = HttpHelper.HttpGet("http://www.chahaoba.com/" + area + lphone, Encoding.UTF8);

                   MatchCollection mc = new Regex(@"title=""(\d{7})"">").Matches(res);
                   ThreadCount = 1;
                   for (int i = 0; i < mc.Count; i++)
                   {
                       this.lphone = mc[i].Groups[1].Value;

                       RunTask(0);
                   }
                   end = mc.Count;
               }
               catch (Exception ee)
               {
                   txtCallback(ee.Message, ConsoleColor.Red);

               }


           }
           while (flagThread != end)
           {
               Thread.Sleep(300);
           }
          
           return strPhone.Trim();
       }
      
       private void ThreadTask()
       {

           for (int i = 0; i < ThreadCount; i++)
           {
               Thread tt = new Thread(RunTask);
               tt.IsBackground = true;
               tt.Start(i);
               Thread.Sleep(20);
           }


       
       }
     
       /// <summary>
       /// 任务进度
       /// </summary>
       private int allTask = 0;
       /// <summary>
       /// 号码生成方法
       /// </summary>
       /// <param name="index"></param>
       private void RunTask(object index)
       {    
           int start=(int)index;

           int wx = 11 - lphone.Length - rphone.Length;

          // string res ="";
           StringBuilder sb = new StringBuilder();
           for (int i = start; i < Math.Pow(10, wx); i += ThreadCount)
           {
               sb.AppendLine(lphone.ToString() + i.ToString().PadLeft(wx, '0') + rphone.ToString());
             
                
           }
           lock (this)
           {
               this.strPhone+=sb.ToString();
           }
           Interlocked.Increment(ref flagThread);
       
       }
       /// <summary>
       /// 记录失败值
       /// </summary>
       public List<string> defaultResult = new List<string>();
       /// <summary>
       /// 返回已经被注册的手机号
       /// </summary>
       private string successResult;
       /// <summary>
       /// 待检测任务数
       /// </summary>
       private int count = 0;
        /// <summary>
        /// 检查未注册
        /// </summary>
        /// <param name="taskResult">待检测任务</param>
        /// <param name="wThreadCount">检测线程</param>
        /// <returns></returns>
       public string UnRegPhone(string[]taskResult,int wThreadCount=30)
       {
           allTask = 0;
           resPhone.Clear();
           defaultResult.Clear();
           successResult="";
           resPhone.AddRange(taskResult);
           if (resPhone.Count!=0)
           {
               ThreadCount = wThreadCount;
               flagThread = 0;
                count=resPhone.Count;
               for (int i = 0; i < ThreadCount; i++)
               {
                   Thread tt = new Thread(WRunTask);
                   tt.IsBackground = true;
                   tt.Start(i);
                   Thread.Sleep(20);
               }

               while (flagThread!=ThreadCount)
               {
                   Thread.Sleep(20);
               }

           }
           return successResult.Trim();
       }
       public TxtCallback txtCallback;
       private void WRunTask(object ob)
       {
       
            int start=(int)ob;
            StringBuilder sbsuccess = new StringBuilder();
            List<string> tempDefault = new List<string>();
            for (int i = 0; i < count; i+=ThreadCount)
            {
                try
                {
                    string phone = resPhone[i];
                    string res = HttpHelper.HttpGet("http://wappass.baidu.com/wp/api/reg/check/phone?tpl=tb&mobilenum=" + phone, Encoding.UTF8);
                    Interlocked.Increment(ref allTask);
                    if (res.Contains("\"no\": \"0\""))
                    {
                        txtCallback(allTask+"-->"+phone + "未注册", Color.Red);
                        //result.Remove(phone);
                    }
                    else
                    {
                        sbsuccess.AppendLine(phone);
                        //successResult+=phone+"\r\n";
                        txtCallback(allTask + "-->" + phone + new Regex(@"msg"": ""([^""]+?)""").Match(res).Groups[1].Value, Color.Green);
                    }
                }
                catch (Exception ee)
                {
                    Interlocked.Increment(ref allTask);
                   tempDefault.Add(resPhone[i]);
                    txtCallback(ee.Message, Color.Red);
                }
               
            }
            lock (this)
            {
                successResult += sbsuccess.ToString();
                defaultResult.AddRange(tempDefault);
            }
            Interlocked.Increment(ref flagThread);
          
       
       }
   
   }
}
