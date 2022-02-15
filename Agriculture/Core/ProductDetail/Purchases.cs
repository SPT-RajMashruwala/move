using Agriculture.Middleware;
using Agriculture.Models.Common;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Core.ProductDetail
{
    public class Purchases
    {
        
        public Result Add(Models.ProductDetail.Purchase value)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();
                MAC login = new MAC();



                var UserMACAddress = login.GetMacAddress().Result;
                var LoginID = context.LoginDetails.FirstOrDefault(c => c.SystemMac == UserMACAddress);
                var purchaselist = (from obj in value.purchaseList
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
                    Data = $"Total {purchaselist.Count()} Product Purchase Successfully",
                };
            }
        }
        public Result View()
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
                              PurchaseID = obj.PurchaseID,
                              ProductName = PN.ProductName,
                              TotalQuantiry = obj.TotalQuantity,
                              TotalCost = obj.TotalCost,
                              Unit = obj.Unit,
                              Remark = obj.Remark,
                              VendorName = obj.VendorName,
                              PurchaseDate = obj.PurchaseDate,
                          }).ToList();
                var result = new Result()
                {
                    Status=Result.ResultStatus.success,
                    Message="Purchse Details",
                    Data=qs,
                };
                return result;
            }

        }
        public Result ViewById(int Id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();

                var qs = (from obj in context.PurchaseDetails
                          join obj2 in context.Products
                          on obj.ProductID equals obj2.ProductID into JoinTablePN
                          from PN in JoinTablePN.DefaultIfEmpty()
                          where obj.PurchaseID == Id
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
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Purchse Details by ID",
                    Data = qs,
                };
                return result;
            }

        }
        public Result Update(Models.ProductDetail.Purchase value, int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var funit = (from obj in value.purchaseList
                             from u in context.Products
                             where obj.productname.Id == u.ProductID
                             select u.Unit).SingleOrDefault();
                var qs = (from obj in context.PurchaseDetails
                          where obj.PurchaseID == ID
                          select obj).SingleOrDefault();
                
                var q = (from obj in value.purchaseList
                         select obj).SingleOrDefault();


                if (qs.ProductID == q.productname.Id)
                {
                    var db = (from obj in context.Products
                              where obj.ProductID == qs.ProductID
                              select obj).SingleOrDefault();
                    var updatedQuantity = db.TotalProductQuantity - qs.TotalQuantity;
                    db.TotalProductQuantity = updatedQuantity + q.totalquantity;
                    context.SubmitChanges();
                }
                else 
                {
                    var db = (from obj in context.Products
                              where obj.ProductID == qs.ProductID
                              select obj).SingleOrDefault();
                    var udb = (from obj in context.Products
                               where obj.ProductID == q.productname.Id
                               select obj).SingleOrDefault();
                     
                    db.TotalProductQuantity = db.TotalProductQuantity - qs.TotalQuantity;
                    udb.TotalProductQuantity = udb.TotalProductQuantity + q.totalquantity;
                    context.SubmitChanges();
                }
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
        public Result Delete(int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext()) 
            {
                var qs = (from obj in context.Categories
                          where obj.CategoryID == ID
                          select obj).SingleOrDefault();
                return new Result()
                {
                    Status=Result.ResultStatus.success,
                    Message="Category Deleted Successful",
                    Data=$"Category : {qs.CategoryName} Deleted Successfully",
                };
            }
        }

    }
}
