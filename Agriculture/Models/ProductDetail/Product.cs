using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.ProductDetail
{
    public class Product
    {
        public List<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }
    public class ProductDetail
    {
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Please Enter Character or Letter.")]
        [Required(ErrorMessage = "Product Name is Required")]
        public string ProductName { get; set; }
        public string Variety { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Please Enter Only Character.")]
        [Required(ErrorMessage = "Company Name is Required")]
        public string Company { get; set; }
        public string Description { get; set; }
        public Models.Common.IntegerNullString categorytype { get; set; } = new Models.Common.IntegerNullString();
        public Models.Common.IntegerNullString type { get; set; } = new Models.Common.IntegerNullString();
    }
}
