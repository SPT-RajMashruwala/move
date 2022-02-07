using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public interface IIssuenewRepository

    {
        Task<string> Add(IssueModel issueModel);
    /*    Task<string> GetQuantity(IntegerNullString model,float getQuantity);*/

    }
}
