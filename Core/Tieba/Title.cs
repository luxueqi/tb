using System;
using System.Collections.Generic;
using System.Text;
using BaiduHelper;
using System.Text.RegularExpressions;

namespace Tieba
{
    class Title
    {

        private string Url { set; get; }
      //  public string CidUrl { set; get; }
        public string title { set; get; }

        public List<string> Content = new List<string>();

       // public List<string> Times = new List<string>();

        public List<string> Authors = new List<string>();

        public List<string> Pids = new List<string>();

        public List<string> Level = new List<string>();

        public List<string> Himgs = new List<string>();

        public List<string> Uids = new List<string>();

        public int maxPn { set; get; }

        private int pn;

        private string tid;

        

       // private Lcid lcid;
        public Title(string tid,int pn=1)
        {
            if (tid.StartsWith("http"))
            {
                this.Url = tid;
            }
            else
            {
                this.tid = tid;

                this.pn = pn;

                this.Url = "https://tieba.baidu.com/p/" + tid + "?pn=" + pn+"&fid="+Common.Fid;
            }
            
           
            GetHtml();
        
        }

        //public Title(string url)
        //{


        //    this.Url = url;

        //    GetHtml();

        //}

        private void GetHtml()
        {
            //try
            //{

            
          
                string html = HttpHelper.HttpGet(this.Url, Encoding.UTF8,null,false,true);
                if (html.Contains("PageData.forum"))
                {
                     maxPn = int.Parse(HttpHelper.Jq(html, "共<span class=\"red\">", "<"));

                    html= html.Replace("j_d_post_content \" style=\"display:none;\">            ", "j_d_post_content \" style=\"display:;\">            ");


                     Content.AddRange(HttpHelper.P_jq(html, "d_post_content j_d_post_content \" style=\"display:;\">            ", "</div><br>"));


                     if (Content.Count == 0) Content.AddRange(HttpHelper.P_jq(html, "d_post_content j_d_post_content  clearfix\">            ", "</div><br>"));

                    Authors.AddRange(HttpHelper.P_jq(html, "<img username=\"", "\""));

                  
                    Pids.AddRange(HttpHelper.P_jq(html, "id=\"post_content_", "\""));

                    //Times.AddRange(HttpHelper.P_jq(html, "楼</span><span class=\"tail-info\">", "<"));
                    //if (Times.Count == 0) Times.AddRange(HttpHelper.P_jq(html, "&quot;date&quot;:&quot;", "&quot;"));

                    Level.AddRange(HttpHelper.P_jq(html, "d_badge_lv\">", "<"));


                    Uids.AddRange(HttpHelper.P_jq(html, "class=\"d_name\" data-field='{&quot;user_id&quot;:", "}'"));
                    //<img username="凡心如雪" class="" src="http://tb.himg.baidu.com/sys/portrait/item/be28e587a1e5bf83e5a682e99baa750c"/>
                    //<img username.*?(http://tb.himg.baidu.com/sys/portrait/item/[0-9][a-z][^"]+?)"/>
                    //<img username="缓和感觉" class="" src="//tb2.bdstatic.com/tb/static-pb/img/head_80.jpg" data-tb-lazyload="http://tb.himg.baidu.com/sys/portrait/item/d651e7bc93e5928ce6849fe8a789dc20"/>
                    //Himgs.AddRange(HttpHelper.P_jq(html, "http://tb.himg.baidu.com/sys/portrait/item/3c5d14f80a4f78f09261e7fe48043a79", "\""));
                    //http://h.hiphotos.baidu.com/forum/sign=3c5d14f80a4f78f09261e7fe48043a79/a6efce1b9d16fdfa68e14c42bd8f8c5495ee7b89.jpg
                    title = HttpHelper.Jq(html, "threadTitle': '", "'").Replace("回复：","");

                    MatchCollection mc = new Regex(@"<img username.*?(https?://[^""]+)""/>", RegexOptions.Singleline).Matches(html);

                   for (int i = 0; i < mc.Count; i++)
                   {
                       Himgs.Add(mc[i].Groups[1].Value);
                   }
                //https://gss0.bdstatic.com/6LZ1dD3d1sgCo2Kml5_Y_D3/sys/portrait/item/6465e58dabe7bb83e7949fe4b880e5a086e592af22d3?t=1531718435
                if (!((Content.Count == Authors.Count) && (Pids.Count == Authors.Count) && (Pids.Count == Himgs.Count) && (Uids.Count == Himgs.Count)))
                   {
                       //int ssss = 0;
                       throw new Exception(this.tid + "title获取出现问题"); 
                   }
                   if (new Regex(@"comment_num&quot;:[1-9]\d{0,},").IsMatch(html))
                   {
                       Lcid lcid = new Lcid();
                       try
                       {
                           // Common.Fid = "190294";
                           if (Common.Fid==null)
                           {
                               lcid = new Lcid(tid, HttpHelper.Jq(html, "fid:'", "'"), pn);
                           }
                           else
                           {
                               lcid = new Lcid(tid, Common.Fid, pn);
                           }
                           

                       }
                       catch (Exception)
                       {


                       }

                       if (lcid.lun.Count > 0)
                       {
                           Content.AddRange(lcid.lcontent);
                           Authors.AddRange(lcid.lun);
                           Pids.AddRange(lcid.lcid);
                           //Times.AddRange(lcid.ltime);
                           Level.AddRange(lcid.llevel);
                          // Himgs.AddRange(lcid.lhimg);
                           Uids.AddRange(lcid.luid);
                       }
                   }
                  

                    
                   
                    contentFilter();
                }
                //else
                //{
                //    if (maxPn == 0)
                //    {
                //        maxPn = 1;
                //    }
            //}http://tieba.baidu.com/p/291067672?pn=64905

            //}
            //catch (Exception)
            //{


            //}
            
        
        }
      
        private void contentFilter()
        {
            //4974066835http://jump.bdimg.com/safecheck/index?url=x+Z5mMbGPAsY/M/Q/im9DR3tEqEFWbC4Yzg89xsWivQZw2INiDucbIDZlVMh9aDcZInIi4k8KEu5449mWp1SxBADVCHPuUFSTGH+WZuV+ecUBG6CY6mAz/Zq1mzxbFxzAG+4Cm4FSU0=
            //http://jump.bdimg.com/safecheck/index?url=x+Z5mMbGPAsY/M/Q/im9DR3tEqEFWbC4Yzg89xsWivTyR3FakJDyZvZF6dLMMtUVK4qTjgNeQuFPhkSMA4BCOOm/fZdF8lDP40ePTcjE3P+qplBjQfoaAchMZgfTf+uS
           //<img class="BDE_Smiley" width="30" height="30" changedsize="false" src="http://static.tieba.baidu.com/tb/editor/images/client/image_emoticon22.png" >
           // //4974066835
            for (int i = 0; i < this.Content.Count; i++)
            {
               // Content[i] = Regex.Replace(Content[i], "<img.*?class=\"BDE_Smiley\".*?src=\"([^\"]+)\".*?>", "$1", RegexOptions.IgnoreCase);
              
                //Content[i] = Regex.Replace(Content[i], "<img.*?src=\"([^\"]+)\".*?>", "$1", RegexOptions.IgnoreCase);
                Content[i] = Regex.Replace(Content[i], @"<a.*?href=""(http://jump\d?\.bdimg\.com/safecheck/index\?url=.+?)?""[^<]+?>([^<]*?)</a>", "$2", RegexOptions.IgnoreCase);

               Content[i] = Regex.Replace(Content[i], "<a.*?href=\"([^\"]+?)\"[^>]*?>([^<]*?)</a>", "$1$2", RegexOptions.IgnoreCase);
                //new Regex(@"<a href=""http://jump\.bdimg\.com/safecheck/index\?url=.+?""[^<]+>([^<]*?)</a>", RegexOptions.Singleline)
               
               // Content[i] = Regex.Replace(Content[i], @"http://jump\.bdimg\.com/safecheck/index\?url=[^=]+=", "");
                //Content[i] = Regex.Replace(Content[i].Replace("<br>", "\r\n"), "</?\\w+?[^>]*>", "", RegexOptions.IgnoreCase);

                Content[i] = Content[i].Replace("&nbsp;", " ").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&").Replace("<br>", "\r\n");

            }
        
        }
    }
}
