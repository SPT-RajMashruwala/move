using Inventory_Mangement_System.Model;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IProductRepository
    {
        Task<string> AddProduct(ProductModel productModel);
        Task<IEnumerable> GetUnit();
    }
}