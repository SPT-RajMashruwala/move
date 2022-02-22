using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KarkhanaBook.Models.Challan
{
    public class TakaChallan
    {
        [Required(ErrorMessage = "TakaChallan Number is Require ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Digit Allow ")]
        public int TakaChallanNumber { get; set; }
        [Required(ErrorMessage = "Per Meter Price is require ! ")]
        [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
        public float RsPerMeter{get;set;}
    }
}
