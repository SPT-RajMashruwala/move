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
                /*purchaseDetail.ProductID = purchaseModel.productname.Id;
                var funit = (from u in context.Products
                             where u.ProductID == purchaseModel.productname.Id
                             select u.Unit).SingleOrDefault ();
                purchaseDetail.Unit = funit;
                purchaseDetail.PurchaseDate = purchaseModel.Purchasedate.ToLocalTime();
                purchaseDetail.TotalQuantity = purchaseModel.totalquantity;
                purchaseDetail.TotalCost  = purchaseModel.totalcost;
                purchaseDetail.Remark  = purchaseModel.remarks;
                purchaseDetail.VendorName = purchaseModel.vendorname;

                
*/
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
                                        UserLoginID = LoginID.LoginID,
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
                //return "Product Purchase Successfully";
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
       /* public Result UpdatePurchaseDetails(PurchaseModel purchaseModel, int purchaseID) 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext()) 
            {
                var funit = (from u in context.Products
                             where u.ProductID == purchaseModel.productname.Id
                             select u.Unit).SingleOrDefault();
                var qs = (from obj in context.PurchaseDetails
                          where obj.PurchaseID == purchaseID
                          select obj).SingleOrDefault();

                qs.ProductID = purchaseModel.productname.Id;
                qs.TotalQuantity = purchaseModel.totalquantity;
                qs.TotalCost = purchaseModel.totalcost;
                qs.Unit = funit;
                qs.Remark = purchaseModel.remarks;
                qs.VendorName = purchaseModel.vendorname;
                qs.PurchaseDate = purchaseModel.Purchasedate.ToLocalTime();

                context.SubmitChanges();
                return new Result()
                {
                    Message = "Purchase Updated Successfully",
                    Status = Result.ResultStatus.success,
                    Data = purchaseModel.productname.Text,
                };
            }



        }*/


    }    
}
