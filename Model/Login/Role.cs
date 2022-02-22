using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Login
{
    public class Role
    {
        [Required(ErrorMessage = "RoleName required ! ")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter Only Letter")]
        public string RoleName { get; set; }
    }
}
