using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KarkhanaBook.Models.Login
{
    public class SignIN
    {
        [Required(ErrorMessage = "Email required ! ")]
       
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password required ! ")]

        public string Password { get; set; }
    }
}
