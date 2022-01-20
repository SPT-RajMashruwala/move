using Inventory_Mangement_System.Model;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IIssueRepository
    {
        Task<IEnumerable> GetMainArea();
        Task<IEnumerable> GetSubArea(int id);
        Task<IEnumerable> GetProduct();
        Task<string> IssueProduct(IssueModel issueModel);
        Task<string> total(IssueModel issueModel);
    }
}