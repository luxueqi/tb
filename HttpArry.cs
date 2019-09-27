using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Text.RegularExpressions;
using Tieba;

namespace BaiduHelper
{    
   
   
    public class HttpHelper
    {   
       public static string loginCookie="";

       public static string[] P_jq(string t, string t1, string t2,bool zhuan=false,int i=0)
        {
            List<string> ls = new List<string>();
            int  l = 0, r = 0;
            while (t.IndexOf(t1, i) > 0)
            {
                l = t.IndexOf(t1,i) + t1.Length;
                r = t.IndexOf(t2, l);
                i = r + t2.Length;
                if (r > 0)
                {
                    string temp = t.Substring(l, r - l);
                    temp=zhuan?Regex.Unescape(temp):temp;
                    ls.Add(temp);
                }
            }
            return ls.ToArray();
        }
        public static string Jq(string t, string t1, string t2)
        {
            string result = null;
            int l = 0, r = 0;
            l = t.IndexOf(t1) + t1.Length;
            r = t.IndexOf(t2, l);
            if (l >=t1.Length && r > 0)
            {
                result = t.Substring(l, r - l);
            }
            return result;
        }
       /* public static string encode(string str)
        {
            string htext = "";
            for (int i = 0; i < str.Length; i++)
            {
                htext = htext + (char)(str[i] + i * i - 1);
            }
            return htext;
        }
        public static string decode(string str)
        {
            string dtext = "";
            for (int i = 0; i < str.Length; i++)
            {
                dtext = dtext + (char)(str[i] - i * i + 1);
            }
            return dtext;
        }*/
        public static string GetMD5HashFromFile(string temp)
        {

          //  temp = HttpUtility.UrlDecode(temp, Encoding.UTF8);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bt = md5.ComputeHash(Encoding.UTF8.GetBytes(temp));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bt.Length; i++)
            {
                sb.Append(bt[i].ToString("x2"));
            }
            return sb.ToString().ToUpper();


        }
        //public static string HttpGet(string url, Encoding encond, string cookie = null, bool loginbool = false)
        //{
        //    string result = "";

        //    using (WebClient wb = new WebClient())
        //    {
        //        wb.Headers["Cookie"] = cookie;
        //        wb.Proxy = null;
        //        wb.Encoding = encond;
        //        result = wb.DownloadString(url);
        //        if (loginbool)
        //        {
        //            result = wb.ResponseHeaders[HttpResponseHeader.Location];
        //            string cksto = wb.ResponseHeaders[HttpResponseHeader.SetCookie];
        //            if (cksto.Contains("STOKEN="))
        //            {
        //                loginCookie += " ;STOKEN=" + Jq(cksto, "STOKEN=", ";");
        //            }


        //        }


        //        wb.Dispose();
        //    }


        //    return result;
        //}
        public static string HttpGet(string url, Encoding encond, string cookie = null, bool loginbool = false,bool blgzip=false)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            StreamReader strReader = null;
            string getHtml = "";
            bool blerror = false;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
               req.Proxy = null;
                req.Timeout =5000;
                req.Headers["Cookie"] = cookie;

                if (blgzip)
                {
                    req.Headers["Accept-Encoding"] = "gzip, deflate, sdch, br";
                    req.AutomaticDecompression = DecompressionMethods.GZip;
                }

                req.KeepAlive = false;                       //防止链接中断无响应
                req.ProtocolVersion = HttpVersion.Version10;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
             req.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
                req.AllowAutoRedirect = false;
                res = (HttpWebResponse)req.GetResponse();
                //res.GetResponseStream().ReadTimeout = 3000;
               
               
                if (loginbool)
                {
                    getHtml = res.Headers[HttpResponseHeader.Location];
                    string cksto = res.Headers[HttpResponseHeader.SetCookie];
                    if (cksto.Contains("STOKEN="))
                    {
                        loginCookie += " ;STOKEN=" + Jq(cksto, "STOKEN=", ";");
                    }


                }
                else
                {
                    if (res.StatusCode==HttpStatusCode.Found||res.StatusCode==HttpStatusCode.MovedPermanently)
                    {
                        getHtml = HttpGet(res.Headers[HttpResponseHeader.Location], encond, cookie, loginbool, blgzip);
                    }
                    else
                    {
                        strReader = new StreamReader(res.GetResponseStream(), encond);
                        getHtml = strReader.ReadToEnd();  
                    }
                  
                }

            }
            catch (Exception ee)
            {
                blerror = true;
                getHtml = ee.Message;
                
            }
            if (strReader!=null)
            {
                strReader.Close();
                strReader.Dispose();
            }
            if (res!=null)
            {
                res.Close();
                res = null;
            }
            if (req!=null)
            {
                req.Abort();

                req = null;
            }
            
            if (blerror)
            {
                throw new Exception(getHtml);
            }
            //if (getHtml=="")
            //{
            //     throw new Exception(getHtml);
            //}
            return getHtml;

           
          
        }
        public static string HttpPost(string url,string postData,string cookie ,string proxy,bool loginbool=false)
        {
            WebProxy pro = new WebProxy(proxy, true);
            pro.Credentials = CredentialCache.DefaultCredentials;
            byte[] bt = Encoding.UTF8.GetBytes(postData);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(url));
            req.Proxy = pro;
            req.Timeout = 9000;
            req.KeepAlive = false;                       //防止链接中断无响应
            req.ProtocolVersion = HttpVersion.Version10;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
              
            if (!string.IsNullOrEmpty(cookie))
            {
                req.Headers["Cookie"] = cookie;
            }
            //req.CookieContainer = new CookieContainer();
            //req.CookieContainer.Add(
            string result = "";
          
                using (Stream  streamReq = req.GetRequestStream())
                {
                    streamReq.Write(bt, 0, bt.Length);
                    streamReq.Close();
                    streamReq.Dispose();
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    res.GetResponseStream().ReadTimeout = 9000;
                    using (StreamReader reader = new StreamReader(res.GetResponseStream(),Encoding.UTF8))
                    {
                       
                        result = reader.ReadToEnd();
                        if (loginbool)
                        {
                            if (result.Contains("href += \"err_no=0&"))
	                        {
                                //PTOKEN=c5c88ad1a75fa77317d519796ff6755a;
                                loginCookie = "BDUSS=" + Jq(res.Headers[HttpResponseHeader.SetCookie], "BDUSS=", ";") + "; PTOKEN=" + Jq(res.Headers[HttpResponseHeader.SetCookie].Replace("PTOKEN=deleted", ""), "PTOKEN=", ";");
                                string stokenurl= HttpGet("https://wappass.baidu.com/v3/login/api/auth/?tpl=tb&jump=&return_type=3&u=https%3A%2F%2Ftieba.baidu.com%2Findex.html&notjump=1", Encoding.UTF8, loginCookie,true);
                                HttpGet(stokenurl, Encoding.UTF8, loginCookie, true);
	                        } 
                        }
                        reader.Close();
                        reader.Dispose();
                        res.Close();
                        req.Abort();
                    }
                }

            
           
            return result;
        
        
        
        }
     
        public static string Login(string username,string password, string vcode ,string pic)
        {   
            
            string postdata = "staticpage=http%3A%2F%2Fyouxi.baidu.com%2Fv3Jump.html&charset=UTF-8&token=&tpl=yx&subpro=&apiver=v3&tt=1487284381911&codestring="+pic+"&safeflg=0&u=http%3A%2F%2Fyouxi.baidu.com&isPhone=false&detect=1&gid=E081AD6-475E-4D7E-8DE4-E2F1157688EA&quick_user=1&logintype=basicLogin&logLoginType=pc_loginBasic&idc=&loginmerge=true&username="+HttpHelper.urlencode( username)+"&password="+HttpHelper.urlencode( password)+"&mem_pass=on&ppui_logintime=51&verifycode=" + vcode + "&callback=parent.bd__pcbs__x7uamo";

            return HttpHelper.HttpPost("https://passport.baidu.com/v2/api/?login", postdata, "BAIDUID=621C76A976E3DCEF722078DD0BEE2291:FG=1; HOSUPPORT=1", null, true);
          }

        public static string Fid(string kw)
        {

            return Jq(HttpGet(Conf.HTTP_URL+"/f/commit/share/fnameShareApi?ie=utf-8&fname=" + kw, Encoding.UTF8), "\"fid\":", ",");
        }

        public static string Tbs(string cookie)
        {

            string res = HttpGet(Conf.HTTP_URL+"/dc/common/tbs",Encoding.UTF8,cookie);
              if (res.Contains("is_login\":1"))
             {
                 return  Jq(res, "tbs\":\"", "\"");
             }
              else
              {
                  return null;
              }


        }


        public static string urldecode(string txt)
        {

            return HttpUtility.UrlDecode(txt);
        
        }


        public static string urlencode(string txt)
        {

            return HttpUtility.UrlEncode(txt);

        }
    
    }

    


}
