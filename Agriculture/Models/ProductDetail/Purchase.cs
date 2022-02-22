using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.ProductDetail
{
    public class Purchase
    {
        public List<PurchaseList> purchaseList { get; set; } = new List<PurchaseList>();
    }

    public class PurchaseList
    {
        [Required(ErrorMessage = "Date Is Required.")]
        public DateTime Purchasedate { get; set; }
        public int PurchaseID { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "Total Quantity Required.")]
        public float totalquantity { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "Cost Required.")]
        public float totalcost { get; set; }
        public string remarks { get; set; }
        public string vendorname { get; set; }
        public Models.Common.IntegerNullString LoginDetail { get; set; } = new Common.IntegerNullString();
        public Models.Common.IntegerNullString Type { get; set; } = new Common.IntegerNullString();
        public DateTime DateTime { get; set; }

        public Models.Common.IntegerNullString productname { get; set; } = new Models.Common.IntegerNullString();
    }
}
