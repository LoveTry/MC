using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC.Models.query
{
    public class FeeQuery
    {
        public Guid ID { get; set; }
        public string OrderNo { get; set; }
        public decimal Money { get; set; }
        public int OrderID { get; set; }
        public DateTime? PayDate { get; set; }
    }
}