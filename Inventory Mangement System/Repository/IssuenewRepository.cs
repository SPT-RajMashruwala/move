using Inventory_Mangement_System.Middleware;
using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class IssuenewRepository : IIssuenewRepository
    {
       
      
        public async Task<string> Add(IssueModel issueModel) 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext()) 
            {
                Issue i = new Issue();
                UserLoginDetails login = new UserLoginDetails();


           

                var MacAddress = login.GetMacAddress().Result;
                var LoginID = context.LoginDetails.FirstOrDefault(c => c.SystemMac == MacAddress);
                var qs = (from obj in issueModel.issueList
                          select new Issue()
                          {

                              ProductID = obj.Product.Id,
                              MainAreaID=obj.MainArea.Id,
                              SubAreaID=obj.SubArea.Id,
                              Remark=obj.Remark,
                              UserLoginID=LoginID.LoginID,
                              DateTime=DateTime.Now,
                              PurchaseQuantity = obj.IssueQuantity

                          }).ToList();
                foreach (var item in qs) 
                {
                    var p = (from obj in context.Products
                             where obj.ProductID == item.ProductID
                             select obj.TotalProductQuantity).SingleOrDefault();
                    if (p < item.PurchaseQuantity)
                    {
                        throw new ArgumentException($"Product name :{item.ProductID} ," +
                            $"Enter quantity{item.PurchaseQuantity} more than existing quantity{p}");
                    }
                  
                }
                        context.Issues.InsertAllOnSubmit(qs);
                        context.SubmitChanges();
                        foreach (var item in qs)
                        {
                            var updatePQ = context.Products.FirstOrDefault(c => c.ProductID == item.ProductID);
                            updatePQ.TotalProductQuantity = updatePQ.TotalProductQuantity - item.PurchaseQuantity;
                            context.SubmitChanges();

                        }
                    return "Issue successful";
                  
                        
         



                   

                


            }
        }
    }
}
