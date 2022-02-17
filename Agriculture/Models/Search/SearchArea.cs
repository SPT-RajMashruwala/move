using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.Search
{
    public class SearchArea
    {
        public int SubAreaID { get; set; }
        public string SubAreaName { get; set; }
        public string MainAreaName { get; set; }
        public string Remark { get; set; }
            public DateTime DateTime { get; set; }
            public string UserName { get; set; }
    }
}
