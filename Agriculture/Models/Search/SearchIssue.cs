using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.Search
{
    public class SearchIssue
    { 
        public int IssueID { get; set; }
           public DateTime IssueDate { get; set; }
            public string MainAreaName { get; set; }
            public string SubAreaName { get; set; }
            public string ProductName { get; set; }
            public float PurchaseQuantity { get; set; }
            public string Remark { get; set; }
            public string UserName { get; set; }
            public DateTime DateTime { get; set; }
    }
}
