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
    public class PurchaseRepository : IPurchaseRepository
    {
        public Result AddPurchaseDetails(PurchaseModel purchaseModel)
        {
            using(ProductInventoryDataContext context = new ProductInventoryDataContext ())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();
                UserLoginDetails login = new UserLoginDetails();
         

                
               var UserMACAddress = login.GetMacAddress().Result;
                var LoginID = context.LoginDetails.FirstOrDefault(c => c.SystemMac == UserMACAddress);
                var purchaselist = (from obj in purchaseModel.purchaseList
                                    select new PurchaseDetail()
                                    {
                                        ProductID = obj.productname.Id,
                                        Unit = (from u in context.Products
                                                where u.ProductID == obj.productname.Id
                                                select u.Unit).SingleOrDefault(),
                                        PurchaseDate = obj.Purchasedate.ToLocalTime(),
                                        TotalQuantity = obj.totalquantity,
                                        TotalCost = obj.totalcost,
                                        Remark = obj.remarks,
                                        VendorName = obj.vendorname,
                                        LoginID = LoginID.LoginID,
                                        DateTime = DateTime.Now


                                    }).ToList();
                foreach (var item in purchaselist) 
                {
                    var qs = (from obj in context.Products
                              where obj.ProductID == item.ProductID
                              select obj).SingleOrDefault();
                    qs.TotalProductQuantity = (int)qs.TotalProductQuantity + item.TotalQuantity;
                    context.SubmitChanges();
                }
                
                context.PurchaseDetails.InsertAllOnSubmit(purchaselist);
                context.SubmitChanges();
               
                return new Result()
                {
                    Message = string.Format($" Purchase successfully!"),
                    Status = Result.ResultStatus.success,
                    Data = DateTime.Now,
                };
            }
        }
        public async Task<IEnumerable> GetPurchaseDetails()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();

                var qs = (from obj in context.PurchaseDetails
                          join obj2 in context.Products
                          on obj.ProductID equals obj2.ProductID into JoinTablePN
                          from PN in JoinTablePN.DefaultIfEmpty()
                          select new 
                          {
                            PurchaseID=obj.PurchaseID,
                            ProductName=PN.ProductName,
                            TotalQuantiry=obj.TotalQuantity,
                            TotalCost=obj.TotalCost,
                            Unit=obj.Unit,
                            Remark=obj.Remark,
                            VendorName=obj.VendorName,
                            PurchaseDate=obj.PurchaseDate,
                          }).ToList();
                return qs;
            }

        }
        public async Task<IEnumerable> GetPurchaseDetailsById(int Id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();

                var qs = (from obj in context.PurchaseDetails
                          join obj2 in context.Products
                          on obj.ProductID equals obj2.ProductID into JoinTablePN
                          from PN in JoinTablePN.DefaultIfEmpty()
                          where obj.PurchaseID==Id
                          select new
                          {
                              PurchaseID = obj.PurchaseID,
                              ProductName = PN.ProductName,
                              TotalQuantiry = obj.TotalQuantity,
                              TotalCost = obj.TotalCost,
                              Unit = obj.Unit,
                              Remark = obj.Remark,
                              VendorName = obj.VendorName,
                              PurchaseDate = obj.PurchaseDate,
                          }).ToList();
                return qs;
            }

        }
        public Result UpdatePurchaseDetails(PurchaseModel purchaseModel, int purchaseID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var funit = (from obj in purchaseModel.purchaseList
                             from u in context.Products
                             where obj.productname.Id==u.ProductID
                             select u.Unit).SingleOrDefault();
                var qs = (from obj in context.PurchaseDetails
                          where obj.PurchaseID == purchaseID
                          select obj).SingleOrDefault();
                var q = (from obj in purchaseModel.purchaseList
                         select obj).SingleOrDefault();
                qs.ProductID = q.productname.Id;
                qs.TotalQuantity = q.totalquantity;
                qs.TotalCost = q.totalcost;
                qs.Unit = funit;
                qs.Remark = q.remarks;
                qs.VendorName = q.vendorname;
                qs.PurchaseDate = q.Purchasedate.ToLocalTime();

                context.SubmitChanges();
                return new Result()
                {
                    Message = "Purchase Updated Successfully",
                    Status = Result.ResultStatus.success,
                    Data = q.productname.Text,
                };
            }



        }


    }    
}
