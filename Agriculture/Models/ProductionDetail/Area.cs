using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.ProductionDetail
{
    public class Area
    {
        public List<MainAreaModel> arealist { get; set; } = new List<MainAreaModel>();
        public string Remark { get; set; }
        public Models.Common.IntegerNullString LoginDetail { get; set; } = new Common.IntegerNullString();
            public DateTime DateTime { get; set; }

      
    }

    public class MainAreaModel
    {
        public Models.Common.IntegerNullString mainArea { get; set; } = new Common.IntegerNullString();
       
        public List<SubAreaModel> subarealist { get; set; } = new List<SubAreaModel>();
    }

    public class SubAreaModel
    {
        public Models.Common.IntegerNullString subArea { get; set; } = new Common.IntegerNullString();

    }
}
