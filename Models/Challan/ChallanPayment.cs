using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KarkhanaBook.Models.Challan
{
    public class ChallanPayment
    {
        [Required(ErrorMessage = "ChallanSlipNumber required ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit ")]
        public string ChallanSlipSerialNumber { get; set; }


        [Required(ErrorMessage = "BillSerialNumber required ! ")]
        public string BillSerialNumber { get; set; }

        [Required(ErrorMessage ="You need to Enter Payment Details ! ")]
        [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
        public float Payment { get; set; }
    }
}
