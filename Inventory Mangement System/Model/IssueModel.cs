using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class IssueModel
    {
        public float PurchaseQuantity { get; set; }
        public DateTime Date { get; set; }
        public Model.IntegerNullString Product { get; set; } = new Model.IntegerNullString();
        public Model.IntegerNullString MainArea { get; set; } = new Model.IntegerNullString();
        public Model.IntegerNullString SubArea { get; set; } = new Model.IntegerNullString();

    }
}
