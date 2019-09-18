﻿using System;
using System.Collections.Generic;
using System.Text;
using BaiduHelper;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
namespace Tieba
{
    class Common
    {
        public static User user;
        public static int ConTime(string time)
        {

            DateTime dt = Convert.ToDateTime(time);

            TimeSpan ts = DateTime.UtcNow.Subtract(dt);



            return (int)Math.Round(ts.TotalMinutes);

        }
        public static string Kw;
        public static string Fid;
        public static bool CookToUn(ref User user)
        {
            //http://help.baidu.com/api/count  http://tieba.baidu.com/f/user/json_userinfo
            string res = HttpHelper.HttpGet("http://help.baidu.com/api/count", Encoding.GetEncoding("GBK"), user.cookie);
            if (!res.Contains("\"errno\":0"))
            {
                return false;
            }
            user.un = Regex.Unescape(HttpHelper.Jq(res, "uname\":\"", "\""));
            user.uid = HttpHelper.Jq(res, "\"uid\":", ",");
            return true;

        }

        public static string[] MangerTb()
        {

            string res = HttpHelper.HttpGet("http://tieba.baidu.com/pmc/tousu/getRole?manager_uname=" + user.un, Encoding.UTF8, user.cookie);

            return HttpHelper.P_jq(Regex.Unescape(res), "forum_name\":\"", "\"");

        }


        public static void Serialize<T>(T o, string filePath)
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
                formatter.Serialize(sw, o);
                sw.Flush();
                sw.Close();
            }
            catch (Exception) { }
        }

        public static T DeSerialize<T>(string filePath)
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
                T o = (T)formatter.Deserialize(sr);
                sr.Close();
                return o;
            }
            catch (Exception)
            {
            }
            return default(T);
        }

        public static string Block(string blockname, int day, string reason, string kw = "", string fid = "")
        {
            if (kw == "" && fid == "")
            {
                kw = Kw;
                fid = Fid;
            }

            string postdata = new Regex("BDUSS=(.{192})").Match(user.cookie).Value + "&day=" + day + "&fid=" + fid + "&ntn=banid&reason=" + reason + "&tbs=" + user.tbs + "&un=" + blockname + "&word=" + kw + "&z=1";

            postdata = postdata + "&sign=" + HttpHelper.GetMD5HashFromFile(postdata.Replace("&", "") + "tiebaclient!!!");

            string res = Regex.Unescape(HttpHelper.HttpPost("http://c.tieba.baidu.com/c/c/bawu/commitprison", postdata, user.cookie, null));

            if (res.Contains("error_code\":\"0\"") && res.Contains("\"un\":\""))
            {
                return "封禁成功";
            }
            return HttpHelper.Jq(res, "error_msg\":\"", "\"");
        }

        public static T readXml<T>(string xml)
        {

            T ut = default(T);
            if (File.Exists(Application.StartupPath + "\\" + xml + ".xml"))
            {
                ut = Common.DeSerialize<T>(Application.StartupPath + "\\" + xml + ".xml");
            }
            return ut;
        }
        public static string Delete(string tids, string kw = "", string fid = "")
        {

            //{"no":0,"err_code":0,"error":"","data":[]}

            if (kw == "" && fid == "")
            {
                kw = Kw;
                fid = Fid;
            }
            string postdata = "ie=utf-8&fid=" + fid + "&tbs=" + user.tbs + "&tid=" + tids + "&kw=" + kw + "&isBan=0";

            string res = HttpHelper.HttpPost("http://tieba.baidu.com/f/commit/thread/batchDelete", postdata, user.cookie, null);
            res = HttpHelper.Jq(res, "err_code\":", ",");
            if (res == "0")
            {
                return "删除成功";
            }
            return "删除失败:" + res;
        }
        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static string UnixTimeToStr(long timestamp)
        {
            var start = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return start.AddSeconds(timestamp).ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string Del(string tid, string pid, string kw = "", string fid = "")
        {
            if (kw == "" && fid == "")
            {
                kw = Kw;
                fid = Fid;
            }
            string postdata = "fid=" + fid + "&tbs=" + user.tbs + "&ie=utf-8&is_finf=false&is_vipdel=0&kw=" + kw + "&pid=" + pid + "&tid=" + tid + "&commit_fr=pb";

            string res = HttpHelper.HttpPost("http://tieba.baidu.com/f/commit/post/delete", postdata, user.cookie, null);
            res = HttpHelper.Jq(res, "err_code\":", ",");
            if (res == "0")
            {
                return "删除成功";
            }
            return "删除失败:" + res;
        }



        //public static string Del(string tid, string pid, string kw = "", string fid = "")
        //{

        //    if (kw==""&&fid=="")
        //    {
        //        kw = Kw;
        //        fid = Fid;
        //    }

        //    string postdata = new Regex("BDUSS=(.{192})").Match(user.cookie).Value + "&_client_id=wappc_1478022768867_350&_client_type=102&_client_version=1.3.1&_phone_imei=865335023934460&appid=bazhu&cuid=4424084304231382863E466761BC06CE|064439320533568&fid=" + fid + "&from=1006294s&is_vipdel=0&isfloor=0&model=2014501&obj_locate=2&pid=" + pid + "&src=1&stoken=&subapp_type=admin&tbs=" + user.tbs + "&word=" + kw + "&z=" + tid;


        //    postdata = postdata + "&sign=" + HttpHelper.GetMD5HashFromFile(postdata.Replace("&", "") + "tiebaclient!!!");

        //    string res = Regex.Unescape(HttpHelper.HttpPost("http://c.tieba.baidu.com/c/c/bawu/delpost", postdata, user.cookie, null));

        //    if (res.Contains("error_code\":\"0\""))
        //    {
        //        return "删除成功";
        //    }
        //    return HttpHelper.Jq(res, "error_msg\":\"", "\"");
        //}


        //public static string Delete(string tid,string kw = "", string fid = "")
        //{

        //    if (kw == "" && fid == "")
        //    {
        //        kw = Kw;
        //        fid = Fid;
        //    }

        //    string postdata = new Regex("BDUSS=(.{192})").Match(user.cookie).Value + "&_client_id=wappc_1478022768867_350&_client_type=102&_client_version=1.3.1&_phone_imei=865335023934460&appid=bazhu&cuid=4424084304231382863E466761BC06CE|064439320533568&fid=" + fid + "&from=1006294s&isban=0&model=2014501&stoken=&subapp_type=admin&tbs=" + user.tbs + "&tids=" + tid + "_";

        //    postdata = postdata + "&sign=" + HttpHelper.GetMD5HashFromFile(postdata.Replace("&", "") + "tiebaclient!!!");

        //    string res = Regex.Unescape(HttpHelper.HttpPost("http://c.tieba.baidu.com/bazhu/commit/bawu/delthreadbatch", postdata, user.cookie, null));

        //    if (res.Contains("error_code\":\"0\""))
        //    {
        //        return "删除成功";
        //    }
        //    return HttpHelper.Jq(res, "error_msg\":\"", "\"");
        //}


        public static string Black(string name, string kw = "", string fid = "")
        {
            if (kw == "" && fid == "")
            {
                kw = Kw;
                fid = Fid;
            }

            string postData = "tbs=" + user.tbs + "&usn=" + name + "&ie=gbk&fid=" + fid + "&fname=" + kw;

            string result = HttpHelper.HttpPost("http://tieba.baidu.com/f/like/commit/black/add", postData, user.cookie, null);
            if (result.Length > 4)
            {
                result = HttpHelper.Jq(result, "\"no\":", ",");

            }

            if (result == "0")
            {
                return "拉黑成功";
            }
            return "拉黑失败:" + result;

            //return result;

        }

        public static string uidtointro(string uid)
        {
            string data = "has_plist=1&is_owner=0&need_post_count=1&pn=1&rn=20&uid=" + uid;

            data = data + "&sign=" + HttpHelper.GetMD5HashFromFile(data.Replace("&", "") + "tiebaclient!!!");

            string res = HttpHelper.HttpPost("http://c.tieba.baidu.com/c/u/user/profile", data, null, null);

            return Regex.Unescape(HttpHelper.Jq(res, "\"intro\":\"", "\""));
        }

        public static List<Pluser> members(int page, string kw)
        {
            //http://tieba.baidu.com/bawu2/platform/listMemberInfo?word=%E9%99%86%E9%9B%AA%E7%90%AA&ie=utf-8
            string url = "http://tieba.baidu.com/bawu2/platform/listMemberInfo?ie=utf-8&word=" + kw + "&pn=" + page;

            string res = HttpHelper.HttpGet(url, Encoding.GetEncoding("GBK"), null, false, true);

            Regex rg = new Regex(@"title=""([^""]+?)"">\1</a><span class=""forum-level-bawu bawu-info-lv([1]?[0-8])", RegexOptions.Singleline);

            MatchCollection mcs = rg.Matches(res);

            List<Pluser> list = new List<Pluser>();
            for (int i = 0; i < mcs.Count; i++)
            {
                list.Add(new Pluser(mcs[i].Groups[1].Value, int.Parse(mcs[i].Groups[2].Value)));

            }

            return list;

        }

        //  &_client_id=wappc_1478022768867_350&_client_type=102&_client_version=1.3.1&_phone_imei=865335023934460&appid=bazhu&cuid=4424084304231382863E466761BC06CE|064439320533568&fid=" + fid + "&from=1006294s&isban=0&model=2014501&stoken=&subapp_type=admin&tbs=" + user.tbs + "&tids=" + tid + "_";

        public static string scantidcount(string kw)
        {
            //List<string[]> listc = new List<string[]>();

            string data = "_client_id=wappc_1568832181855_350&_client_type=2&_client_version=5.2.2&_phone_imei=fbce298035372371ff25decc0951c03&from=tieba&kw=" + kw+"&pn=1&q_type=2&rn=30&with_group=1";

            data = data + "&sign=" + HttpHelper.GetMD5HashFromFile(data.Replace("&", "") + "tiebaclient!!!");

            string res = HttpHelper.HttpPost("http://c.tieba.baidu.com/c/f/frs/page", data, null, null);

            return res;
        }

    }


}
