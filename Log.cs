using System;
using System.Collections.Generic;
using System.Text;

namespace Tieba
{
    public class Log
    {

        public string index;

        public string author;

        public string tid;

        public string title;

        public string type;

        public string result;

        public string tbname;

        public string fid;
        public Log() { }

        public Log(string index, string author, string title, string tid, string type, string result,string tbname,string fid)
        {


            this.index = index;
            this.author = author;
            this.tid = tid;
            this.title = title;
            this.type = type;
            this.result = result;
            this.tbname = tbname;
            this.fid = fid;
        
        
        }
    }
}
