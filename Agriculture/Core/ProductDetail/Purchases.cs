using Agriculture.Middleware;
using Agriculture.Models.Common;
using Agriculture.Models.ProductDetail;
using Agriculture.Models.Search;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Data;
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
                                        UnitID = (from u in context.Products
                                                where u.ProductID == obj.productname.Id
                                                select u.ProductUnit.UnitID).SingleOrDefault(),
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

                Models.ProductDetail.Purchase purchase = new Models.ProductDetail.Purchase()
                {
                    purchaseList = new List<Models.ProductDetail.PurchaseList>(),
                    
                    
                      
                };
                var qs = (from obj in context.PurchaseDetails
                         select obj).ToList();
                foreach (var x in qs) 
                {
                    purchase.purchaseList.Add(new Models.ProductDetail.PurchaseList
                    {
                        productname=new IntegerNullString() { Id=x.Product.ProductID,Text=x.Product.ProductName},
                        totalcost=(float)x.TotalCost,
                        totalquantity=(float)x.TotalQuantity,
                        remarks=x.Remark,
                        Purchasedate=Convert.ToDateTime(x.PurchaseDate),
                        vendorname=x.VendorName,
                        DateTime=Convert.ToDateTime(x.DateTime),
                        LoginDetail=new IntegerNullString() { Id=x.LoginDetail.LoginID,Text=x.LoginDetail.UserName},
                        Type=new IntegerNullString() { Id=x.ProductUnit.UnitID,Text=x.ProductUnit.Type},
                        
                    });
                }
                var result = new Result()
                {
                    Status=Result.ResultStatus.success,
                    Message="Purchse Details",
                    Data=purchase,
                };
                return result;
            }

        }
        public Result ViewSearch(DataTable table)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {

                Models.ProductDetail.Purchase purchase = new Models.ProductDetail.Purchase()
                {
                    purchaseList = new List<Models.ProductDetail.PurchaseList>(),



                };

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow dr = table.Rows[i];
                    purchase.purchaseList.Add(new Models.ProductDetail.PurchaseList
                    {

                       
                        
                        


                        PurchaseID = Int16.Parse(dr["PurchaseID"].ToString()),
                        productname = new IntegerNullString(){ Id=Int16.Parse(dr["ProductID"].ToString()),Text=dr["ProductName"].ToString()},
                        totalquantity = float.Parse(dr["TotalQuantity"].ToString()),
                        totalcost = float.Parse(dr["TotalCost"].ToString()),
                        vendorname = dr["VendorName"].ToString(),
                        Purchasedate = Convert.ToDateTime(dr["PurchaseDate"].ToString()),
                        remarks = dr["Remark"].ToString(),
                        LoginDetail = new IntegerNullString(){Id=Int16.Parse(dr["LoginID"].ToString()),Text=dr["UserName"].ToString() },
                        DateTime = Convert.ToDateTime(dr["DateTime"].ToString()),
                        Type = new IntegerNullString(){Id=Int16.Parse(dr["UnitID"].ToString()),Text=dr["Type"].ToString() },


                    });

                }

                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Purchse Details",
                    Data = purchase,
                };
                return result;
            }
        }
        public Result ViewById(int Id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail();

                Models.ProductDetail.Purchase purchase = new Models.ProductDetail.Purchase()
                {
                    purchaseList = new List<Models.ProductDetail.PurchaseList>()

                };
                var qs = (from obj in context.PurchaseDetails
                          where obj.PurchaseID==Id
                          select obj).ToList();
                foreach (var x in qs)
                {
                    purchase.purchaseList.Add(new Models.ProductDetail.PurchaseList
                    {
                        productname = new IntegerNullString() { Id = x.Product.ProductID, Text = x.Product.ProductName },
                        totalcost = (float)x.TotalCost,
                        totalquantity = (float)x.TotalQuantity,
                        remarks = x.Remark,
                        Purchasedate = Convert.ToDateTime(x.PurchaseDate),
                        vendorname = x.VendorName,
                        DateTime = Convert.ToDateTime(x.DateTime),
                        LoginDetail = new IntegerNullString() { Id = x.LoginDetail.LoginID, Text = x.LoginDetail.UserName },
                        Type = new IntegerNullString() { Id = x.ProductUnit.UnitID, Text = x.ProductUnit.Type },

                    });
                }
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Purchse Details",
                    Data = purchase,
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
                             select u.ProductUnit.UnitID).SingleOrDefault();
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
                    qs.UnitID = funit;
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
