using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC.Models.query
{
    public class OrderQuery
    {
        public int ID { get; set; }
        public string OrderNo { get; set; }
        public decimal ProMoney { get; set; }
        public string State { get; set; }
        public string StateInfo { get; set; }
        public string CrTime { get; set; }
        public string CusName { get; set; }
        public string CusPhone { get; set; }
        public string UserName { get; set; }
        public string TrueName { get; set; }
        public string Name { get; set; }
    }
}