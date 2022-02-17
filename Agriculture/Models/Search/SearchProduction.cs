using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.Search
{
    public class SearchProduction
    {
        public int ProductionID { get; set; }
           public string MainAreaName { get; set; }
            public string SubAreaName { get; set; }
            public string VegetableName { get; set; }
            public float Quantity { get; set; }
            public string Remark { get; set; }
            public string UserName { get; set; }
            public DateTime DateTime { get; set; }
    }
}
