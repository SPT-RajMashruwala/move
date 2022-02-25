using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Challan
{
    public class ChallanSlip
    {
        [JsonIgnore]
        public int ChallanSlipIndex { get; set; }
        public string SellerName { get; set; }
        [Required(ErrorMessage = "ChallanSlip Serial Number Required ! ")]
        public string ChallanSlipSerialNumber { get; set; }


        [Required(ErrorMessage = "Range of Cartoon Serial Number required ! ")]
        public string RangeCartoonSerialNumber { get; set; }


        [Required(ErrorMessage = "TotalCartoons Required ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
        public int TotalCartoons { get; set; }


        [Required(ErrorMessage = "Price for Cartton - per Kg required ! ")]
        [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
        public float RsPerKG { get; set; }

        [Required(ErrorMessage = "TotalWeight is Required ! ")]
        [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
        public float TotalWeight { get; set; }


        [Required(ErrorMessage = "Date of Purchase required ! ")]
        public DateTime DateOfPurchase { get; set; }


        public string Remark { get; set; }
    }
}
