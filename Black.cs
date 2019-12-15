using System;
using System.Collections.Generic;
using BaiduHelper;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;
namespace Tieba
{
    //public delegate void TxtCallback(string str, Color color);

    public  class Black
    {

        private bool boolblack;

        private bool boolblock;

        private bool boollevel;

        private int level;

        private int pn;

        private string  kw;

        private string fid;

        //private string name;

        private int threadcount;

        private int dtime;

        private bool boolpostnum;

        private double postnum;


        private bool boolzz;

        private string txtzz;

        public static TxtCallback txtCallback;

        public string error;


       
      
       public Black() { }

       //public Black(string kw,string name, bool boolbalck, bool boolblock)
       //{
       //    this.kw = kw;

       //    this.name = name;
          
       //    this.boolblack = boolbalck;

       //    this.boolblock = boolblock;

       //    this.error = "";

       //    try
       //    {
       //        this.fid = HttpHelper.Fid(kw);

       //        Common.Kw = kw;
       //        Common.Fid = fid;
       //    }
       //    catch (Exception ee)
       //    {

       //        this.error = "获取tid错误" + ee.Message;
       //    }


       //}

       public Black(string kw, bool boolbalck, bool boolblock, bool boollevel, int level, int pn,int threadcount,int dtime,bool boolpostnum,double postnum,bool boolzz,string txtzz)
        {
            this.kw = kw;

            this.boolblack = boolbalck;

            this.boolblock = boolblock;

            this.boollevel = boollevel;

            this.level = level;

            this.pn = pn;

            this.threadcount = threadcount;

            this.dtime = dtime;

            this.boolpostnum = boolpostnum;

            this.postnum = postnum;

            this.boolzz = boolzz;

            this.txtzz = txtzz;

            this.error = "";

            try
            {
                this.fid = HttpHelper.Fid(kw);
                //Common.Kw = kw;
                //Common.Fid = fid;
            }
            catch (Exception ee)
            {

                this.error = "获取fid错误"+ee.Message;
            }

            
        }


       //public void bname()
       //{
       //   this.error= namemethod(name);
       
       //}

       private string  namemethod(string namew,string portrait)
       {
           string res = "";

           if (this.boolblack)
           {

               res = Common.Black(namew,kw,fid);
           }

           if (this.boolblock)
           {
                //页面改版 portrait封禁方式暂时改为用户名
                // res += "-" + Common.Block(namew,"", 10, "lahei",kw,fid);
                res += "-" + Common.wyblock(portrait, 10, "laihei", this.fid);
           }

           return res;
       }

       private volatile bool stopflag;
       public void bnames()
       {
              Thread[] ths = new Thread[threadcount];
              this.count = 0;
              this.stopflag = false;
              for (int i = 0; i < threadcount; i++)
               {
                   ths[i] = new Thread(method);
                  
                   ths[i].IsBackground = true;

                   ths[i].Name = (i + 1).ToString();

                  
               }

              for (int i = 0; i < threadcount; i++)
               {
                   ths[i].Start(i);
               }

              
       }

       private int count;
       private void method(object ob)
       {
           int start = this.pn + (int)ob;

           for (int i = start; i > 0; i += threadcount)
           {
               try
               {
                   if (stopflag)
                   {
                       break;
                   }
                   List<Pluser> list = Common.members(i, kw);

                   for (int j = 0; j < list.Count; j++)
                   {
                       if (stopflag)
                       {
                           break;
                       }
                       int templevel = list[j].levle;

                       string tempname = list[j].un;
                       Interlocked.Increment(ref count);
                       try
                       {
                           if (this.boollevel&&templevel!=this.level)
                           {
                               txtCallback(count+"-"+i+"-"+tempname + "-用户等级:" + templevel + ",与指定等级不符合", Color.Green);
                               continue;
                           }

                           if (!this.boollevel && templevel >this.level)
                           {
                               txtCallback(count + "-" + i + "-" + tempname + "-用户等级:" + templevel + ",大于设定等级不符合", Color.Green);
                               continue;
                           }

                           if (this.boolzz&&!new Regex(this.txtzz).IsMatch(tempname))
                           {
                               txtCallback(count + "-" + i + "-" + tempname + "-正则匹配:" + this.txtzz + ",未匹配", Color.Green);
                               continue;
                           }

                           if (this.boolpostnum&&tempname!="")
                           {
                               ID id = new ID(tempname);

                               if (id.error=="")
                               {
                                    double postnumq = 0;
                                    if (id.postNum.Contains("万"))
                                    {
                                        id.postNum = id.postNum.Replace("万", "");

                                        postnumq = double.Parse(id.postNum) * 10000;

                                    }
                                    else
                                    {
                                        postnumq = double.Parse(id.postNum);
                                    }

                                    if (this.postnum <= postnumq)
                                    {
                                        txtCallback(count + "-" + i + "-" + tempname + "-用户发帖量:" + postnumq + ",大于设定不符合", Color.Green);
                                        continue;
                                    }

                               }
                               else
                               {
                                   txtCallback(count + "-" + i + "-" + tempname +"-错误:" +id.error, Color.Red);
                                   continue;
                               }
                           }
                           txtCallback(count + "-" + i + "-" + tempname + "-" + namemethod(tempname,list[j].portait), Color.Red);
                          
                       }
                       catch (Exception ee)
                       {

                           txtCallback(count + "-" + i + "-" + ee.Message, Color.Red);
                       }

                       Thread.Sleep(dtime * 1000);
                   }

               }
               catch (Exception ee)
               {

                   txtCallback(count + "-" + i + "-" + ee.Message, Color.Red);
               }
           }
       }

       public void setStop()
       {
           this.stopflag = true;
       }
       //public List<Pluser>  members(int page)
        //{
        //    //http://tieba.baidu.com/bawu2/platform/listMemberInfo?word=%E9%99%86%E9%9B%AA%E7%90%AA&ie=utf-8
        //    string url = "http://tieba.baidu.com/bawu2/platform/listMemberInfo?ie=utf-8&word=" + kw+"&pn="+page;

        //    string res = HttpHelper.HttpGet(url, Encoding.UTF8);


        //    return null;
            
        //}


    }



   public class Pluser
   {

       public string un;

       public int levle;

        public string portait;

       public Pluser(string un, int level,string portait)
       {
           this.un = un;

           this.levle = level;

            this.portait = portait;
       
       }
   
   }
}
