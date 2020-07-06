using System;
using System.Collections.Generic;
using System.Text;

namespace Tieba
{
    public class Accention
    {
        public string intime;

        public string level;

        public string black;

        public string tbname;


        public Accention() { }

        public Accention(string tbname, string level, string black, string intime)
        {
            this.tbname = tbname;

            this.level = level;

            this.black = black;

            this.intime = intime;
        
        
        
        }


    }
}
