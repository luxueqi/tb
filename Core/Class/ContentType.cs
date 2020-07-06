using System;
using System.Collections.Generic;
using System.Text;

namespace Tieba
{
   public  class ContentType
    {
        public string content;

        public bool iszz;

        public bool isshou;

        public string type;

        public ContentType() { }
        public ContentType(string content,bool iszz,bool isshou=false,string type="")
        {

            this.content = content;

            this.type = type;

            this.iszz = iszz;

            this.isshou = isshou;
        }


    }
}
