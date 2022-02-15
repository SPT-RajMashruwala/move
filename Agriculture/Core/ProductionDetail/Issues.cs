using Agriculture.Middleware;
using Agriculture.Models.Common;
using ProductInventoryContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Core.ProductionDetails
{
    public class Issues
    {
        public Result Add(Models.ProductionDetail.Issue value)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Issue i = new Issue();
                MAC login = new MAC();
                var MacAddress = login.GetMacAddress().Result;
                var LoginID = context.LoginDetails.FirstOrDefault(c => c.SystemMac == MacAddress);
                var qs = (from obj in value.issueDetails
                          select new Issue()
                          {

                              ProductID = obj.Product.Id,
                              MainAreaID = value.MainArea.Id,
                              SubAreaID = value.SubArea.Id,
                              Remark = obj.Remark,
                              LoginID = LoginID.LoginID,
                              DateTime = DateTime.Now,
                              PurchaseQuantity = obj.IssueQuantity

                          }).ToList();
                var mainArea = (from obj in context.MainAreas
                                where obj.MainAreaID == value.MainArea.Id
                                select obj.MainAreaName).SingleOrDefault();
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

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Issue Products Successful",
                    Data = $"Total {qs.Count()} Product Issue For Main Area : {mainArea} ",
                };

            }
        }
        public Result ViewById(int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {

                var qs = (from obj in context.Issues
                          join obj2 in context.MainAreas
                          on obj.MainAreaID equals obj2.MainAreaID into JoinTableMA
                          from MA in JoinTableMA.DefaultIfEmpty()
                          join obj3 in context.SubAreas
                          on obj.SubAreaID equals obj3.SubAreaID into JoinTableSA
                          from SA in JoinTableSA.DefaultIfEmpty()
                          join obj4 in context.Products
                          on obj.ProductID equals obj4.ProductID into JoinTablePD
                          from PD in JoinTablePD.DefaultIfEmpty()
                          where obj.IssueID == ID
                          select new
                          {
                              IssueID = obj.IssueID,
                              ProductName = PD.ProductName,
                              MainAreaName = MA.MainAreaName,
                              SubAreaName = SA.SubAreaName,
                              IssueQuantity = obj.PurchaseQuantity,
                              Remark = obj.Remark


                          }).ToList();
                return new Result()
                {
                    Status=Result.ResultStatus.success,
                    Message="Issue ViewByID",
                    Data=qs,
                };

            }
        }
        public Result View()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var qs = (from obj in context.Issues
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
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Issue View",
                    Data = qs,
                };
            }
        }
    }
}
