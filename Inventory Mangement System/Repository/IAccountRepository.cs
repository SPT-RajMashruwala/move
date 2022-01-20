using Inventory_Mangement_System.Model;
using System.Collections;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IAccountRepository
    {
        Task<string> AddRole(RoleModel roleModel);
        Task<IEnumerable> RegisterUser(UserModel userModel);
        Task<string> LoginUser(LoginModel loginModel);
    }
}