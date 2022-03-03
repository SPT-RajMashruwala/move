using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Taka
{
    public class TakaIssue
    {
        /*    [Required(ErrorMessage = "TakaID required ! ")]
     [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
     public int TakaID { get; set; }*/

        [JsonIgnore]
        public int TakaIssueIndex { get; set; }

        [Required(ErrorMessage = "TakaChallanNumber is required ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
        public int TakaChallanNumber { get; set; }
        public List<Takas> takadetails { get; set; } = new List<Takas>();

    }

    public class Takas
    {
        [Required(ErrorMessage = "TakaID required ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
        public int TakaID { get; set; }

        [Required(ErrorMessage = "SlotNumber is required ! ")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter Only Digit")]
        public int SlotNumber { get; set; }

    }
}
