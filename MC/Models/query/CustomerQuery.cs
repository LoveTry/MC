using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC.Models.query
{
    public class CustomerQuery
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Num { get; set; }
        public string Sex { get; set; }
    }
}