using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class UserModel
    {
        [Required(ErrorMessage = "UserName required")]
        public string  UserName { get; set; }

        [Required(ErrorMessage = "EmailAddress required")]
        public string EmailAddress { get; set; }

        [RegularExpression(@"[A-Za-z][a-z0-9@#_]{6,}[a-z0-9]$", ErrorMessage = "Minimum 8 character," +
            "Start first letter uppercase,atleast one lower case,one number and one special symbol")]
        [Required(ErrorMessage = "Password required")]
        public string  Password { get; set; }
    }
}
