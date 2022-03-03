using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Challan
{
    public class TakaChallan
    {
        [JsonIgnore]
        public int TakaChallanIndex { get; set; }
        [Required(ErrorMessage = "TakaChallan Number is Require ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only Digit Allow ")]
        public int TakaChallanNumber { get; set; }
        [JsonIgnore]
        public float TotalTakaQuantity { get; set; }
        [JsonIgnore]
        public float TotalMeter { get; set; }
        [JsonIgnore]
        public float TotalWeight { get; set; }

        [Required(ErrorMessage = "Per Meter Price is require ! ")]
        [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
        public float RsPerMeter { get; set; }
        public float TotalBillValue { get; set; }
        public string Remark { get; set; }
    }

}
