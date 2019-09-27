using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC.Models.sqllite
{
    public class WUserItem
    {

        public string openid { get; set; }
        public string nickname { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public int sex { get; set; }
        public DateTime createtime { get; set; }
    }
}