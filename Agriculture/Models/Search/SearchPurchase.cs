using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.Search
{
    public class SearchPurchase
    {
        public int PurchaseID { get; set; }
        public string ProductName { get; set; }
        public float TotalQuantity { get; set; }
        public float TotalCost { get; set; }
        public string VendorName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Remark { get; set; }
            public string UserName { get; set; }
            public DateTime DateTime { get; set; }
            public string Type { get; set; }


    }
}
