using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.Login
{
    public class SignIN
    {
        [Required(ErrorMessage = "EmailAddress Required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
    }
}
