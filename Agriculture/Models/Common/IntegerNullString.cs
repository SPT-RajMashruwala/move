using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.Common
{
    public class IntegerNullString
    {

        public int Id { get; set; }
       /* [Required(ErrorMessage ="Text Field is Require")]*/
        public string Text { get; set; }
    }
}
