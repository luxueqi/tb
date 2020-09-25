using System;
using System.Collections.Generic;
using System.Text;

namespace Tieba
{
    public class Mode
    {
        public bool isdel { set; get; }

        public bool isblock { set; get; }

        public bool isblackname{ set; get; }

        public bool istime { set; get; }

        // public bool istbage{ set; get; }

        public bool ispostnum { set; get; }

        public bool iszz { set; get; }

        public bool islz { set; get; }

        public bool isintro { set; get; }

      //  public bool isblack { set; get; }

        public bool islevel { set; get; }

       // public bool isftday { set; get; }

        public bool isimghash { set; get; }

        public bool isimgdct { set; get; }

       // public bool ishimg;
        
        public int blockday = 1;

        public int level = 1;

        //public int ftday = 1;

        public int sctime = 100;

        public int postnum = 0;

        public int pn = 1;

       // public double tbage = 0;

       // public string mode = "";

        public DateTime dt;

       // public string blacks = "";

       // public string keys = "";

        public string mangertb = "";

        public string reason = "";

        public List<ContentType> ctreplaykeys;
        
        public List<ContentType> cttitlekeys;

        public List<ContentType> ctblacknames;

        public List<ContentType> ctwhitenames;

        public List<ContentType> ctwhitecontents;

        public int hashdistance;

        public string[] localimghash;

       

        public Mode() { }

        //public Mode(object[] items)
        //{

        //    mangertb = items[0].ToString();

        //    isdel = (bool)items[1];

        //    isblock = (bool)items[2];

        //   // isshou = (bool)items[3];

        //    iszz = (bool)items[3];

        //    isblack = (bool)items[4];

        //    islevel = (bool)items[5];

        //    isftday = (bool)items[6];

        //    blockday = int.Parse(items[7].ToString());

        //    level = int.Parse(items[8].ToString());

        //    ftday = int.Parse(items[9].ToString());

        //    sctime = int.Parse(items[10].ToString());

        //    mode = items[11].ToString();

        //    blacks = items[12].ToString();

        //    keys = items[13].ToString();

        //   reason = items[14].ToString();

        //   pn = int.Parse(items[15].ToString());

        //     white= items[16].ToString();

        //     istbage = (bool)items[17];

        //     ispostnum = (bool)items[18];

        //    tbage= double.Parse(items[19].ToString());

        //    postnum = int.Parse(items[20].ToString());

        //    isimghash = (bool)items[21];

        //    isblackname = (bool)items[22];

        //    isimgdct = (bool)items[23];

        //}



        //public void setValue(string setk,object setv)
        //{
        //    switch (setk)
        //    {
        //        case "localimghash": localimghash = (string[])setv; break;
        //        case "hashdistance": hashdistance = (int)setv; break;
        //        case "isdel": isdel=(bool)setv; break;
        //        case "isblock": isblock = (bool)setv; break;
        //        case "isblackname": isblackname = (bool)setv; break;
        //        case "isintro": isintro = (bool)setv; break;
        //        case "islz": islz = (bool)setv; break;
        //        case "istbage": istbage = (bool)setv; break;
        //        case "ispostnum": ispostnum = (bool)setv; break;
        //        case "iszz": iszz = (bool)setv; break;
        //        //case "isblack": isblack = (bool)setv; break;
        //        case "islevel": islevel = (bool)setv; break;
        //      //  case "isftday": isftday = (bool)setv; break;
        //        case "isimghash": isimghash = (bool)setv; break;
        //       // case "ishimg": ishimg = (bool)setv; break;
        //        case "isimgdct": isimgdct = (bool)setv; break;
        //        case "blockday": blockday = (int)setv; break;
        //        case "level": level = (int)setv; break;
        //        //case "ftday": ftday = (int)setv; break;
        //        case "sctime": sctime = (int)setv; break;
        //        case "postnum": postnum = (int)setv; break;
        //        case "pn": pn= (int)setv; break;
        //        case "tbage": tbage = (double)setv; break;
        //        case "mode":  mode = setv.ToString(); break;
        //        //case "blacks":  blacks = setv.ToString(); break;
        //       // case "keys":  keys= setv.ToString(); break;
        //        case "mangertb":  mangertb= setv.ToString(); break;
        //        case "reason": reason= setv.ToString(); break;
        //      //  case "white":white= setv.ToString(); break;
        //        default:
        //            throw new Exception("不存在该变量"+setk);
                    

        //    }
        
        //}



    
    }
}
