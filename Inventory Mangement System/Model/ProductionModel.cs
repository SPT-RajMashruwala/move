using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Model
{
    public class ProductionModel
    {
        public List<ProductionList> productionLists { get; set; } = new List<ProductionList>();
    }
    public class ProductionList 
    {
        public string vegetablenm { get; set; }
        public float Quantity { get; set; }
        public string Remark { get; set; }
        public Model.Common.IntegerNullString mainAreaDetails { get; set; } = new Model.Common.IntegerNullString();
        public Model.Common.IntegerNullString subAreaDetails { get; set; } = new Model.Common.IntegerNullString();

    }
}
