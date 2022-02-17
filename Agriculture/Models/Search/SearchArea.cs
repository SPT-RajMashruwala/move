using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.Search
{
    public class SearchArea
    {
        public Models.Common.IntegerNullString mainArea { get; set; } = new Common.IntegerNullString();
        public Models.Common.IntegerNullString subArea { get; set; } = new Common.IntegerNullString();
        public string Remark { get; set; }
            public DateTime DateTime { get; set; }
         public Models.Common.IntegerNullString LoginDetail { get; set; } = new Common.IntegerNullString();
    }
}
