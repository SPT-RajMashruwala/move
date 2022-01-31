using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{ 
    public interface IProductRepository
    {
        Result AddProduct(ProductModel productModel);
        public Result UpdateProduct(JsonPatchDocument productModel, int productID);
        Task<IEnumerable> GetUnit();
    }
}