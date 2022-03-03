
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Challan
{
    public class ChallanPayment
    {
        [JsonIgnore]
        public int PaymentSlipIndex { get; set; }
        [Required(ErrorMessage = "ChallanSlipNumber re quired ! ")]
       /* [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit ")]*/
        public string ChallanSlipSerialNumber { get; set; }


        [Required(ErrorMessage = "BillSerialNumber required ! ")]
        public string BillSerialNumber { get; set; }

        [Required(ErrorMessage = "You need to Enter Payment Details ! ")]
        [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
        public float TotalWeight { get; set; }
        public float Payment { get; set; }
        public string Remark { get; set; }
    }
}
