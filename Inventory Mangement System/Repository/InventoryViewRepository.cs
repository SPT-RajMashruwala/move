using Inventory_Mangement_System.Model.Common;
using ProductInventoryContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class InventoryViewRepository: IInventoryViewRepository
    {
        public async Task<IEnumerable> GetInventoryView()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                //return (from x in context.Products
                //            select new { 
                //                ProductName=x.ProductName,
                //                Variety=x.Variety,
                //                Company=x.Company,
                //                Category=x.Category
                //            }).ToList();
                return (from x in context.Products
                        select new IntegerNullString()
                        {
                            Text = x.ProductName,
                            Id = x.ProductID
                        }).ToList();
                //return new Result()
                //{
                //    Message = string.Format("fully!"),
                //    Status = Result.ResultStatus.success,
                //    Data = result,
                //};
            }
        }
    }
}
