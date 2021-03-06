using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface ICategoryRepository
    {
        //Result AddCategory(CategoryModel categoryModel, int Uid);
        Result AddCategory(CategoryModel categoryModel);
        Task<IEnumerable> GetCategory();

    }
}