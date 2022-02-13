using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.ProductDetail
{
    public class Category
    {
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Please Enter Character or Letter.")]
        [Required(ErrorMessage = "CategoryName Required.")]
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
