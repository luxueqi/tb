using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Tieba
{
    class TbInfo
    {


        public List<string> Titles = new List<string>(); 

        public List<string> Authors = new List<string>();

        public List<string> Replay = new List<string>();

        public List<string> Tids = new List<string>();

        public List<string> Uids = new List<string>();

        // public string[] Himgs { get; set; }

        // public string html;

        public TbInfo(string tbname) 
        {
          
           string res= Common.scantidcount(tbname);

           MatchCollection mcs= new Regex(@"""thread_id"":""([^""]+)"",""original_tid"":""0"",""title"":""([^""]+)"".+?author"":\{""id"":""([^""]+)"",""name"":""([^""]*)"",""sex"":""[^""]*"",""name_show"":""([^""]+)"".+?abstract"":\[\{""type"":""0"",""text"":""([^""]*)").Matches(res);
            if (mcs.Count == 0) throw new Exception("页面获取错误");
            for (int i = 0,count=mcs.Count; i < count; i++)
            {
                Tids.Add(mcs[i].Groups[1].Value);
                Titles.Add(Regex.Unescape(mcs[i].Groups[2].Value));
                Uids.Add(mcs[i].Groups[3].Value);
                Authors.Add(Regex.Unescape(mcs[i].Groups[4].Value==""? "昵称:" + mcs[i].Groups[5].Value:mcs[i].Groups[4].Value));
                Replay.Add(Regex.Unescape(mcs[i].Groups[6].Value));
            }

        }

       /* public void GetHtml()
        {
           // TbName = "诛仙";
            string url = "http://tieba.baidu.com/f?ie=utf-8&kw="+TbName;

            //http://imgsrc.baidu.com/forum/wh%3D160%2C90/sign=71e33fab9a13b07ebde858093ae7bd1a/20ee4936acaf2edd29ddf163841001e938019377.jpg
            //try
            //{
                html = HttpHelper.HttpGet(url, Encoding.UTF8,null,false,true);
                int i = html.LastIndexOf("title=\"置顶\"")+700;
                Titles = HttpHelper.P_jq(html, "class=\"j_th_tit \">", "</a>",false,i);
                if (Titles.Length == 0) Titles = HttpHelper.P_jq(html, "class=\"j_th_tit\">", "</a>",false,i);

                Tids = HttpHelper.P_jq(html, "'{&quot;id&quot;:", ",", false, i);
                                               //&quot;author_name&quot;:null,&quot
                //Authors = HttpHelper.P_jq(html, "&quot;author_name&quot;:&quot;", "&quot;",true,i);
                Authors = HttpHelper.P_jq(html, "title=\"主题作者: ", "\"", false, i);
                
                Replay = HttpHelper.P_jq(html, "onlyline \">", "</div>");

               // Himgs = HttpHelper.P_jq(html, "data-tb-lazyload=\"", "\"");

                if (!((Titles.Length == Tids.Length) && (Tids.Length == Authors.Length) && (Authors.Length == Replay.Length))) throw new Exception("TbInfo->GetHtml()页面获取失败");
               // LastReplay = HttpHelper.P_jq(html, "最后回复人: ", "\"");
                Filter();
            //}
            //catch (Exception ee)
            //{

               
            //}
        
        }*/

      /*  private void Filter()
        {
            html = "";

            for (int i = 0; i < this.Replay.Length; i++)
            {
              
                //Replay[i] = Regex.Replace(Replay[i], "<img.*src=\"([^\"]+)\".*?>", "$1", RegexOptions.IgnoreCase);

                //Replay[i] = Regex.Replace(Replay[i], "<a.*href=\"([^\"]+)\"[^>]*>([^<]*)</a>", "$1$2", RegexOptions.IgnoreCase);

                //Replay[i] = Regex.Replace(Replay[i].Replace("<br>", "\r\n"), "</?\\w+?[^>]*>", "", RegexOptions.IgnoreCase);

                //Replay[i] = Replay[i].Replace("&nbsp;", " ").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&");
                Replay[i] = Regex.Replace(Replay[i], "<img.*?class=\"BDE_Smiley\".*?src=\"([^\"]+)\".*?>", "$1", RegexOptions.IgnoreCase);

                //Replay[i] = Regex.Replace(Replay[i], "<img.*?src=\"([^\"]+)\".*?>", "$1", RegexOptions.IgnoreCase);
                Replay[i] = Regex.Replace(Replay[i], @"<a.*?href=""(http://jump\.bdimg\.com/safecheck/index\?url=.+?)?""[^<]+?>([^<]*?)</a>", "$2", RegexOptions.IgnoreCase);

                Replay[i] = Regex.Replace(Replay[i], "<a.*?href=\"([^\"]+?)\"[^>]*?>([^<]*?)</a>", "$1$2", RegexOptions.IgnoreCase);
                //new Regex(@"<a href=""http://jump\.bdimg\.com/safecheck/index\?url=.+?""[^<]+>([^<]*?)</a>", RegexOptions.Singleline)

                // Replay[i] = Regex.Replace(Replay[i], @"http://jump\.bdimg\.com/safecheck/index\?url=[^=]+=", "");
                //Replay[i] = Regex.Replace(Replay[i].Replace("<br>", "\r\n"), "</?\\w+?[^>]*>", "", RegexOptions.IgnoreCase);

                Replay[i] = Replay[i].Replace("&nbsp;", " ").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&").Replace("<br>", "\r\n");
                html += Replay[i] + "\r\n";
            }

        }*/
    }
}
