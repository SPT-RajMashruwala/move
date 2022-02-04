using Inventory_Mangement_System.Model;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class IssuenewRepository : IIssuenewRepository
    {
        public float receivequantity;
        public async Task<string> GetQuantity(IssueModel issueModel,float getQuantity) 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext()) 
            {
                this.receivequantity = getQuantity;
                var pq = (from obj in context.Products
                          from obj2 in issueModel.issueList
                          where obj.ProductID == obj2.Product.Id
                          select obj.TotalProductQuantity).SingleOrDefault();
                if (this.receivequantity > pq)
                {
                    throw new ArgumentException("Quantity of Product is less then you enter ");
                }
                else
                {
                    return "";
                }
            
            }


        }
        public async Task<string> Add(IssueModel issueModel) 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext()) 
            {
                Issue i = new Issue();
                
            
                    var p = (from obj in context.Products
                              from obj2 in issueModel.issueList
                              where obj.ProductID == obj2.Product.Id
                              select obj.TotalProductQuantity).SingleOrDefault();
                    if (this.receivequantity < p)
                    {

                        var qs = (from obj in issueModel.issueList
                                  select new Issue()
                                  {
                                      ProductID = obj.Product.Id,
                                      PurchaseQuantity = obj.IssueQuantity

                                  }).ToList();
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
                    else {
                        throw new ArgumentException("Quantity of Product is less then you enter");
                    }



                   

                


            }
        }
    }
}
