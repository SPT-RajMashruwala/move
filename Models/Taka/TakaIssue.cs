using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KarkhanaBook.Models.Taka
{
    public class TakaIssue
    {
        /*    [Required(ErrorMessage = "TakaID required ! ")]
            [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
            public int TakaID { get; set; }*/

        public List<TakaId> takaid { get; set; } = new List<TakaId>();

        [Required(ErrorMessage = "TakaChallanID required ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
        public int TakaChallanID { get; set; }
    }
    public class TakaId 
    {
        [Required(ErrorMessage = "TakaID required ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
        public int TakaID { get; set; }

    }
}
