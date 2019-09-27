using System;
using System.Collections.Generic;
using System.Text;
using BaiduHelper;
using System.Text.RegularExpressions;
using System.Web;
namespace Tieba
{
   public class ID
    {
       public string un;

       public string uid;

       public string age;

       private List<Accention>  acction=new List<Accention>();

       public string postNum;

       //public string regTime;

       public bool isprivate;

       //public string email;

       //public string phone;

       public string image;

       public string switchImageTime;

       public string manger;

       public string assist;

       public string error;

       public ID() { }

       public ID(string un)
       {

           this.un = un;
           error = "";
           GetIdInfo();
       
       }


       private void GetIdInfo()
       {

           try
           {
               string url =Conf.HTTP_URL+ "/home/get/panel?ie=utf-8&un=" + un;

               string res =HttpHelper.HttpGet(url, Encoding.UTF8);

               if (res.Contains("\"no\":1130023"))
               {
                   this.error = "该用户不存在";
                   return;
               }

               uid = new Regex(@"""id"":([^,]+)").Match(res).Groups[1].Value;

               isprivate = res.Contains("is_private\":1");

               Match mc = new Regex(@"""portrait"":""([^""]+)"",""portrait_h"":""[^""]+"",""portrait_time"":([^,]+),""sex"":""\w+?"",""tb_age"":""?([^""]+)""?,""post_num"":""?([^,|""]+)").Match(res);

               switchImageTime = mc.Groups[2].Value == "0" ? "" : Common.UnixTimeToStr(long.Parse(mc.Groups[2].Value));

               age =mc.Groups[3].Value;

               postNum =Regex.Unescape( mc.Groups[4].Value);

               image = "http://himg.baidu.com/sys/portraitl/item/" + mc.Groups[1].Value+".jpg";

               //string koupei = HttpHelper.HttpGet("http://koubei.baidu.com/home/" + uid, Encoding.UTF8);

               //regTime =Common.UnixTimeToStr( long.Parse( HttpHelper.Jq(koupei, "regtime\":", ",")));

               //email = HttpHelper.Jq(koupei, "secureemail\":\"", "\"");

               //phone = HttpHelper.Jq(koupei, "securemobil\":\"", "\"");

               
               GetManger(@"forum_name"":""([^""]+)"",""forum_role"":""manager""");
               GetManger(@"forum_name"":""([^""]+)"",""forum_role"":""assist""",1);
           
           }
           catch (Exception ee)
           {

               this.error = ee.Message;
           }
       
       
       }

       public List<Accention> GetLikeTb()
       {
           try
           {
               string res2 = HttpHelper.HttpGet(Conf.HTTP_URL+"/home/main/?ie=utf-8&un=" + un, Encoding.GetEncoding("GBK"));

               MatchCollection mcs = new Regex(@"forum_name"":""([^""]+)"",""is_black"":(\d),""is_top"":\d,""in_time"":([^,]+),""level_id"":(\d{1,2}),").Matches(res2);

               foreach (Match item in mcs)
               {
                   acction.Add(new Accention(Regex.Unescape(item.Groups[1].Value), item.Groups[4].Value, item.Groups[2].Value, Common.UnixTimeToStr(long.Parse(item.Groups[3].Value))));
               }
           }
           catch (Exception ee)
           {
               this.error = ee.Message;
              
           }
          

           return acction;
       }

       private void GetManger(string partten,int i=0)
       {
           error = "";
           string re3 = Regex.Unescape(HttpHelper.HttpGet(Conf.HTTP_URL+"/pmc/tousu/getRole?manager_uname=" + HttpUtility.UrlEncode(HttpUtility.UrlEncode(un)), Encoding.UTF8, Common.user.cookie));

           MatchCollection mcs = new Regex(partten).Matches(re3);

           foreach (Match item in mcs)
           {
               if (i==0)
               {
                   manger += item.Groups[1].Value + " ";
               }
               else
               {
                   assist += item.Groups[1].Value + " ";
               }
               
           }
       }
    }

}
