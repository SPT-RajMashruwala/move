using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class PurchaseModel
    {
        public List<PurchaseList> purchaseList { get; set; } = new List<PurchaseList>();
    }
    public class PurchaseList
    {
        public DateTime Purchasedate { get; set; }
        public float totalquantity { get; set; }
        public float totalcost { get; set; }
        public string remarks { get; set; }
        public string vendorname { get; set; }
        public Model.Common.IntegerNullString productname { get; set; } = new Model.Common.IntegerNullString();

    }
}
