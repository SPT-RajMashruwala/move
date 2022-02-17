using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.ProductDetail
{
    public class Category
    {
       /* [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Please Enter Character or Letter.")]
        [Required(ErrorMessage = "CategoryName Required.")]*/

        public Models.Common.IntegerNullString categoryType { get; set; } = new Common.IntegerNullString();
        public Models.Common.IntegerNullString loginDetail { get; set; } = new Common.IntegerNullString();
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }
}
