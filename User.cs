using System;
using System.Collections.Generic;
using System.Text;

namespace Tieba
{
    public class User
    {
        public string un { set; get; }

        public string psd { set; get; }

        public string cookie { set; get; }

        public string tbs { set; get; }

        public string uid { set; get; }

        public User(){}
        public User(string un="",string psd="",string cookie="",string tbs="",string uid="")
        {

            this.un = un;

            this.psd = psd;

            this.cookie = cookie;

            this.tbs = tbs;

            this.uid = uid;
        
        }
    }
}
