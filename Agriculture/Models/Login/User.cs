﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.Login
{
    public class User
    {
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Please Enter Character or Letter")]
        [Required(ErrorMessage = "UserName Required")]
        public string UserName { get; set; }

        [RegularExpression(@"[a-zA-Z0-9.-_]{1,}@[a-zA-Z.-]{2,}[.]{1}[a-zA-Z]{2,}$", ErrorMessage = "Please Enter Valid EmailAddress")]
        [Required(ErrorMessage = "EmailAddress Required")]
        public string EmailAddress { get; set; }

        [RegularExpression(@"[A-Za-z0-9][a-z0-9@#_ .]{6,}[a-z0-9@#_.]$", ErrorMessage = "Minimum 8 character," +
            "First letter not start special character ,atleast one lower case,one number and one special symbol")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
    }
}
