using Agriculture.Middleware;
using Agriculture.Models.Common;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Core.ProductDetail
{
    public class Products
    {
        public Result Add(Models.ProductDetail.Product value)
        {
            
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            MAC mac = new MAC();
            var macObj = mac.GetMacAddress().Result;
            var MacAddress = context.LoginDetails.FirstOrDefault(x => x.SystemMac == macObj);
            Category category = new Category();
            Product product = new Product();

            var pro = (from p in value.Productlist
                       select new Product()
                       {
                           ProductName = p.ProductName,
                           Variety = p.Variety,
                           Company = p.Company,
                           Description = p.Description,
                           CategoryID = (int)p.categorytype.Id,
                           UnitID = (int)p.type.Id,
                           TotalProductQuantity = 0,
                           LoginID=MacAddress.LoginID,
                           DateTime=DateTime.Now,
                       }).ToList();
            foreach (var item in pro)
            {
                var pname = context.Products.Where(name => name.ProductName == item.ProductName).SingleOrDefault();
                if (pname != null)
                {
                    throw new ArgumentException($"{item.ProductName} product Alredy Exits.");
                }
            }

            context.Products.InsertAllOnSubmit(pro);
            context.SubmitChanges();
            return new Result()
            {
                Message = string.Format($"Products Added Successfully."),
                Status = Result.ResultStatus.success,
                Data =$"Total {pro.Count()} Product Added",
            };

            //var pname = context.Products.Where(name => name.ProductName == productModel.ProductName).SingleOrDefault();
            //if (pname != null)
            //{
            //    throw new ArgumentException("Alredy Exits");
            //}
            //product.ProductName = productModel.ProductName;
            //product.Variety = productModel.Variety;
            //product.Company = productModel.Company;
            //product.Description = productModel.Description;
            //product.Unit = (string)productModel.type.Text;
            //product.CategoryID = (int)productModel.categorytype.Id;
            //context.Products.InsertOnSubmit(product);
            //context.SubmitChanges();
            //return $"{productModel.ProductName} Added Successfully";
            //return new Result()
            //{
            //    //Message = string.Format($"{productModel.ProductName} Added successfully!"),
            //    Status = Result.ResultStatus.success,
            //    //Data= productModel.ProductName,
            //};
        }

        //async Task<IEnumerable>
        public Result GetUnit()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "GetUnit Successful",
                    Data = (from x in context.ProductUnits
                            select new IntegerNullString()
                            {
                                Id = x.UnitID,
                                Text = x.Type,
                            }).ToList()
                };
                return result;
            }
        }
        //GetProduct Dropdown
        public Result GetProduct()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "GetProduct Successful",
                    Data = (from x in context.Products
                            select new
                            {
                               /* ProductID=x.ProductID,
                                ProductName=x.ProductName,
                                Variety=x.Variety,
                                Company=x.Company,
                                Description=x.Description,
                                Unit=x.Unit,
                                Category=x.Category,
                                TotalProductQuantity=x.TotalProductQuantity
                                UserName=x.LoginDetail
                                DateTime=*/

                                Id = x.ProductID,
                                Text = x.ProductName,
                                Type=new IntegerNullString() { Id=x.ProductUnit.UnitID,Text=x.ProductUnit.Type},
                                TotalQuantity = (float)x.TotalProductQuantity,
                            }).ToList(),
                };
                return result;
            }
        }

        public Result View()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "ViewProduct Successful",
                    Data = (from x in context.Products
                            
                            select new
                            {
                                Product = new IntegerNullString() { Id = x.ProductID, Text = x.ProductName },
                                Varitey = x.Variety,
                                Company = x.Company,
                                Description = x.Description,
                                Type = new IntegerNullString() {Id=x.ProductUnit.UnitID,Text=x.ProductUnit.Type},
                                CategoryType= new IntegerNullString() { Id = x.Category_CategoryID.CategoryID, Text = x.Category_CategoryID.CategoryName },
                                TotalProductQuantity = x.TotalProductQuantity,
                                UserName = x.LoginDetail_LoginID.UserName,
                                DateTime = x.DateTime,
                            }).ToList()
                };
                return result;
            }
        }

        public Result ViewById(int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Product product = new Product();
                product = context.Products.SingleOrDefault(x => x.ProductID == ID);
                if (product == null)
                {
                    throw new ArgumentException("Product Does Not Exist");
                }
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "View Product ByID Successful",
                    Data = (from x in context.Products
                            join catid in context.Categories
                            on x.CategoryID equals catid.CategoryID
                            where x.ProductID==ID
                            select new
                            {
                                Product=new IntegerNullString() { Id=x.ProductID,Text=x.ProductName},
                                Varitey = x.Variety,
                                Company = x.Company,
                                Description = x.Description,
                                Type = new IntegerNullString() { Id = x.ProductUnit.UnitID, Text = x.ProductUnit.Type },
                                CategoryType = new IntegerNullString() { Id = x.Category_CategoryID.CategoryID, Text = x.Category_CategoryID.CategoryName },
                                TotalProductQuantity = x.TotalProductQuantity,
                                UserName = x.LoginDetail_LoginID.UserName,
                                DateTime = x.DateTime,
                            }).ToList(),
                };
                return result;
            }
        }

        public Result Update(Models.ProductDetail.Product value , int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var dbobj = (from obj in value.Productlist
                             select obj).SingleOrDefault();

                var pn = (from obj in context.Products
                          where obj.ProductID == id
                          select obj).SingleOrDefault();
                
                if (pn==null)
                {
                    throw new ArgumentException("Product doesn't exist");
                }

                if ( pn.ProductName != dbobj.ProductName)
                {
                    var _product = context.Products.SingleOrDefault(name => name.ProductName == dbobj.ProductName);
                    if (_product != null)
                    {
                        throw new Exception("Product Alredy Exits.");
                    }
                }

                pn.ProductName = dbobj.ProductName;
                pn.Variety = dbobj.Variety;
                pn.Company = dbobj.Company;
                pn.CategoryID = dbobj.categorytype.Id;
                pn.UnitID = dbobj.type.Id;
                pn.Description = dbobj.Description;
               
                context.SubmitChanges();
                return new Result()
                {
                    Message = "Updated Successfully",
                    Data = pn.ProductName,
                    Status = Result.ResultStatus.success,
                };
            }
        }

        public Result Delete(int ID) 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext()) 
            {
                var qs = (from obj in context.Products
                          where obj.ProductID == ID
                          select obj).SingleOrDefault();
        
                context.Products.DeleteOnSubmit(qs);
                context.SubmitChanges();
                return new Result()
                {
                    Status=Result.ResultStatus.success,
                    Message="Product Deleted Succesfully",
                    Data=qs.ProductName,
                };
            }
        }
    }
}
