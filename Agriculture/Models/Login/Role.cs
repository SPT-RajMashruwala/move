using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.Login
{
    public class Role
    {
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Please Enter Character or Letter")]
        [Required(ErrorMessage = "RoleName Required")]
        public string RoleName { get; set; }
    }
}
