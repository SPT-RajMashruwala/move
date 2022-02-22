using Inventory_Mangement_System.Middleware;
using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using ProductInventoryContext;
using System;
using System.Collections;
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
                              MainAreaID = issueModel.MainArea.Id,
                              SubAreaID = issueModel.SubArea.Id,
                              Remark =obj.Remark,
                              LoginID=LoginID.LoginID,
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
        public async Task<IEnumerable> ViewById( int issueID) 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
               
                var ID = (from obj in context.Issues
                          join obj2 in context.MainAreas
                          on obj.MainAreaID equals obj2.MainAreaID into JoinTableMA
                          from MA in JoinTableMA.DefaultIfEmpty()
                          join obj3 in context.SubAreas
                          on obj.SubAreaID equals obj3.SubAreaID into JoinTableSA
                          from SA in JoinTableSA.DefaultIfEmpty()
                          join obj4 in context.Products
                          on obj.ProductID equals obj4.ProductID into JoinTablePD
                          from PD in JoinTablePD.DefaultIfEmpty()
                          where obj.IssueID == issueID
                          select new
                          {
                              IssueID=obj.IssueID,
                              ProductName=PD.ProductName,
                              MainAreaName=MA.MainAreaName,
                              SubAreaName=SA.SubAreaName,
                              IssueQuantity=obj.PurchaseQuantity,
                              Remark=obj.Remark


                          }).ToList();
                return ID;

            }
        }
        public async Task<IEnumerable> View()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext()) {
                var ID = (from obj in context.Issues
                          join obj2 in context.MainAreas
                          on obj.MainAreaID equals obj2.MainAreaID into JoinTableMA
                          from MA in JoinTableMA.DefaultIfEmpty()
                          join obj3 in context.SubAreas
                          on obj.SubAreaID equals obj3.SubAreaID into JoinTableSA
                          from SA in JoinTableSA.DefaultIfEmpty()
                          join obj4 in context.Products
                          on obj.ProductID equals obj4.ProductID into JoinTablePD
                          from PD in JoinTablePD.DefaultIfEmpty()
                          select new
                          {
                              IssueID = obj.IssueID,
                              ProductName = PD.ProductName,
                              MainAreaName = MA.MainAreaName,
                              SubAreaName = SA.SubAreaName,
                              IssueQuantity = obj.PurchaseQuantity,
                              Remark = obj.Remark


                          }).ToList();
                return ID;
            }
        }

        public async Task<string> Update(IssueModel issueModel, int issueID)  
        {
            float RemainQuantity=0 ;
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                UserLoginDetails login = new UserLoginDetails();
                var MacAddress = login.GetMacAddress().Result;
                var mac = context.LoginDetails.FirstOrDefault(c => c.SystemMac == MacAddress);
                var qs = (
                         from obj in context.Issues
                         where obj.IssueID == issueID
                         select obj).SingleOrDefault();
                var p = (from obj in issueModel.issueList
                         select obj).SingleOrDefault();
                var pd = (from obj in context.Products
                                 where obj.ProductID == qs.ProductID
                                 select obj).SingleOrDefault();
                var ps = (from obj in context.Products
                          where obj.ProductID == p.Product.Id
                          select obj).SingleOrDefault();
                var pid = (from obj in context.Issues
                           where obj.SubAreaID == issueModel.SubArea.Id
                           select new 
                           {
                           ProductID=obj.ProductID
                           }).ToList();


                if (qs.SubAreaID == issueModel.SubArea.Id)
                {
                    if (qs.ProductID == p.Product.Id)
                    {
                        var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                        RemainQuantity = (float)temp - p.IssueQuantity;
                        qs.DateTime = DateTime.Now;
                        qs.ProductID = p.Product.Id;
                        qs.MainAreaID = issueModel.MainArea.Id;
                        qs.SubAreaID = issueModel.SubArea.Id;
                        qs.LoginID = mac.LoginID;
                        qs.Remark = p.Remark;
                        qs.PurchaseQuantity = p.IssueQuantity;

                        ps.TotalProductQuantity = RemainQuantity;
                        context.SubmitChanges();
                        return "Issue Update Successfully";
                    }
                    else
                    {
                        foreach (var item in pid)
                        {
                            if (p.Product.Id == item.ProductID)
                            {
                                throw new Exception($"Entered Product : {item.ProductID} already issued for given sub" +
                                    $" area : {issueModel.SubArea.Text}");

                            }

                            

                            

                        }
                                var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                                qs.DateTime = DateTime.Now;
                                qs.ProductID = p.Product.Id;
                                qs.MainAreaID = issueModel.MainArea.Id;
                                qs.SubAreaID = issueModel.SubArea.Id;
                                qs.LoginID = mac.LoginID;
                                qs.Remark = p.Remark;
                                qs.PurchaseQuantity = p.IssueQuantity;
                                RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                                ps.TotalProductQuantity = RemainQuantity;
                                pd.TotalProductQuantity = temp;
                                context.SubmitChanges();
                                return "Issue Update Successfully";
                    }



                }
                else 
                {
                    foreach(var item in pid)
                        {
                        if (p.Product.Id == item.ProductID)
                        {
                            throw new Exception($"Entered Product : {item.ProductID} already issued for given sub" +
                                $" area : {issueModel.SubArea.Text}");

                        }
                       

                        

                    }
                            var temp = pd.TotalProductQuantity + qs.PurchaseQuantity;
                            pd.TotalProductQuantity = temp;
                            qs.DateTime = DateTime.Now;
                            qs.ProductID = p.Product.Id;
                            qs.MainAreaID = issueModel.MainArea.Id;
                            qs.SubAreaID = issueModel.SubArea.Id;
                            qs.LoginID = mac.LoginID;
                            qs.Remark = p.Remark;
                            qs.PurchaseQuantity = p.IssueQuantity;
                            RemainQuantity = (float)ps.TotalProductQuantity - p.IssueQuantity;
                            ps.TotalProductQuantity = RemainQuantity;
                            context.SubmitChanges();
                            return "Issue Update Successfully";

                }

             /*   qs.DateTime = DateTime.Now;
                qs.ProductID = p.Product.Id;
                qs.MainAreaID = issueModel.MainArea.Id;
                qs.SubAreaID = issueModel.SubArea.Id;
                qs.UserLoginID = mac.LoginID;
                qs.Remark = p.Remark;
                qs.PurchaseQuantity = p.IssueQuantity;

                ps.TotalProductQuantity = RemainQuantity;
                context.SubmitChanges();

                return "";
*/


            }
        
        }
    }
}
