using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IPurchaseRepository
    {
        Result AddPurchaseDetails(PurchaseModel purchaseModel);
        Task<IEnumerable> GetPurchaseDetails();
        Task<IEnumerable> GetPurchaseDetailsById(int Id);
        Result UpdatePurchaseDetails(PurchaseModel purchaseModel, int purchaseID);
    }
}