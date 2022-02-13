using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.ProductionDetail
{
    public class Area
    {
        public List<MainAreaModel> arealist { get; set; } = new List<MainAreaModel>();
    }

    public class MainAreaModel
    {
        public string mname { get; set; }
        public List<SubAreaModel> subarea { get; set; } = new List<SubAreaModel>();
    }

    public class SubAreaModel
    {
        public string sname { get; set; }
    }
}
