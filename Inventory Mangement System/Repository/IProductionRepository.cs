using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{ 
    public interface IProductionRepository
    {
        Result AddProductionDetails(ProductionModel value);
        Task<IEnumerable> GetProductionDetails();
        Task<IEnumerable> GetProductionDetailsById(int id);
        Result UpdateProductionDetails(ProductionModel productionModel, int productionID);
    }
}