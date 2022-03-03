using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Taka
{
    public class TakaSheet
    {
        [Required(ErrorMessage ="Sloat Number is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
        public int SloatNumber { get; set; }
        public List<Taka> takas { get; set; } = new List<Taka>();

    }
    public class Taka
    {
        [JsonIgnore]
        public int TakaSheetIndex { get; set; }
        [Required(ErrorMessage = "MachineNumber required ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
        public int TakaID { get; set; }


        [Required(ErrorMessage = "MachineNumber required ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
        public int MachineNumber { get; set; }


        [Required(ErrorMessage = "Meter required ! ")]
        [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
        public float Meter { get; set; }


        [Required(ErrorMessage = "Weight required ! ")]
        [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
        public float Weight { get; set; }

        public DateTime Date { get; set; }


    }
}
