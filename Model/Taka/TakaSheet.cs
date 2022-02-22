using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Taka
{
    public class TakaSheet
    {
        public List<Taka> Taka { get; set; } = new List<Taka>();

        public DateTime Date { get; set; }

    }
    public class Taka
    {
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



    }
}
