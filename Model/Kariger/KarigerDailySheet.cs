using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Kariger
{
    public class KarigerDailySheet
    {
        [Required(ErrorMessage = "UserName required ! ")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Only Letter")]
        public string UserName { get; set; }


        /* [Required(ErrorMessage = "Average of Machine required ! ")]
         [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
         public float AVGOfMachine { get; set; }


         [Required(ErrorMessage = "MachineNumber  required ! ")]
         [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
         public int MachineNumber { get; set; }*/

        public List<Machine> machine { get; set; } = new List<Machine>();


        [Required(ErrorMessage = "Shift required ! ")]
        public string Shift { get; set; }


        [Required(ErrorMessage = "Date Must required ! ")]
        public DateTime Date { get; set; }
    }
    public class Machine
    {
        [Required(ErrorMessage = "Average of Machine required ! ")]
        [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
        public float AVGOfMachine { get; set; }


        [Required(ErrorMessage = "MachineNumber  required ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
        public int MachineNumber { get; set; }
    }
}
