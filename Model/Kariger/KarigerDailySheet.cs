using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Kariger
{
    public class KarigerDailySheet
    {
        public int IndexNumber{get;set;}
       /* [Required(ErrorMessage = "UserName required ! ")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Only Letter")]*/
        public Model.Common.IntegerNullString UserName { get; set; } = new Common.IntegerNullString();
        /*[Required(ErrorMessage = "Shift required ! ")]*/
        public Model.Common.IntegerNullString Shift { get; set; } = new Model.Common.IntegerNullString();
        [Required(ErrorMessage = "Date Must required ! ")]

        public DateTime Date { get; set; }
        public string Remark { get; set; }

        public List<Machine> machine { get; set; } = new List<Machine>();
        /* [Required(ErrorMessage = "Average of Machine required ! ")]
         [RegularExpression(@"[+-]?([0-9]*[.])?[0-9]+$", ErrorMessage = "Enter Only Digit Ex.Float-Value 300.00 ")]
         public float AVGOfMachine { get; set; }


         [Required(ErrorMessage = "MachineNumber  required ! ")]
         [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
         public int MachineNumber { get; set; }*/



       


     
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
