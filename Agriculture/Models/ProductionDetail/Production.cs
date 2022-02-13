using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Models.ProductionDetail
{
    public class Production
    {
        public List<ProductionList> ProductionLists { get; set; } = new List<ProductionList>();
    }
    public class ProductionList
    {
        [RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "Please Enter Only Character.")]
        [Required(ErrorMessage = "Vegetable Name Required.")]
        public string Vegetablenm { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter Only Letter.")]
        [Required(ErrorMessage = "Quantity Required.")]
        public float Quantity { get; set; }
        public string Remark { get; set; }
        public Models.Common.IntegerNullString mainAreaDetails { get; set; } = new Models.Common.IntegerNullString();
        public Models.Common.IntegerNullString subAreaDetails { get; set; } = new Models.Common.IntegerNullString();
    }
}
