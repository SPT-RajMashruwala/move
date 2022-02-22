using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Model.Login
{
    public class SingIN
    {
        [Required(ErrorMessage = "Email required ! ")]

        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password required ! ")]

        public string Password { get; set; }
    }
}
