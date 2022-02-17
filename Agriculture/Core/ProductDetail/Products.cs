using Agriculture.Middleware;
using Agriculture.Models.Common;
using Agriculture.Models.ProductDetail;
using Microsoft.Extensions.Configuration;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Core.ProductDetail
{
    public class Products
    {
    
        private readonly IConfiguration _configuration;

        public Products(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public Products() 
        {
        }


        public Result Add(Models.ProductDetail.Product value)
        {

            ProductInventoryDataContext context = new ProductInventoryDataContext();
            MAC mac = new MAC();
            var macObj = mac.GetMacAddress().Result;
            var MacAddress = context.LoginDetails.FirstOrDefault(x => x.SystemMac == macObj);
           /* ProductInveCategory category = new Category();
            Product product = new Product();*/

            var pro = (from p in value.Productlist
                       select new ProductInventoryContext.Product()
                       {
                           ProductName = p.productDetail.Text,
                           Variety = p.Variety,
                           Company = p.Company,
                           Description = p.Description,
                           CategoryID = (int)p.categorytype.Id,
                           UnitID = (int)p.Type.Id,
                           TotalProductQuantity = 0,
                           LoginID = MacAddress.LoginID,
                           DateTime = DateTime.Now,
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
                Data = $"Total {pro.Count()} Product Added",
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
                          

                                Id = x.ProductID,
                                Text = x.ProductName,
                                Type = new IntegerNullString() { Id = x.ProductUnit.UnitID, Text = x.ProductUnit.Type },
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
                Models.ProductDetail.Product product = new Models.ProductDetail.Product()
                {
                    Productlist = new List<ProductListClass>()
                };
                var qs = (from obj in context.Products
                          select obj).ToList();
                foreach (var x in qs) {
                    product.Productlist.Add(new Models.ProductDetail.ProductListClass
                    {
                        
                        productDetail = new IntegerNullString() { Id=x.ProductID,Text=x.ProductName},
                        categorytype= new IntegerNullString() { Id=x.Category_CategoryID.CategoryID,Text=x.Category_CategoryID.CategoryName},
                        Company=x.Company,
                        Description=x.Description,
                        Type=new IntegerNullString() { Id=x.ProductUnit.UnitID,Text=x.ProductUnit.Type},
                        Variety=x.Variety,
                        
                    });
                    product.LoginDetail = new IntegerNullString() { Id = x.LoginDetail_LoginID.LoginID, Text = x.LoginDetail_LoginID.UserName };
                }
                
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "ViewProduct Successful",
                    Data = product,
                   
                };
                return result;
            }
        }
        public Result ViewSearch(DataTable table)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Models.ProductDetail.Product product = new Models.ProductDetail.Product()
                {
                    Productlist = new List<ProductListClass>()
                };

                
                for (int i=0; i<table.Rows.Count;i++) 
                {
                    DataRow dr = table.Rows[i];
                
                    product.Productlist.Add(new ProductListClass 
                    {
                      
                        categorytype = new IntegerNullString() { Id = Int16.Parse(dr["CategoryID"].ToString()), Text = dr["CategoryName"].ToString() },
                        Description = dr["Description"].ToString(),
                        Type = new IntegerNullString() { Id = Int16.Parse(dr["UnitID"].ToString()), Text = dr["Type"].ToString() },
                        Variety = dr["Variety"].ToString(),
                        Company= dr["Company"].ToString(),
                        
                    });
                    product.LoginDetail.Text= dr["UserName"].ToString();
                }

                var result = new Result()
                {


                    Status = Result.ResultStatus.success,
                    Message = "View Product Successful",
                    Data = product,
                        
                };
                return result;
            }
        }

        public Result ViewById(int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Models.ProductDetail.Product product = new Models.ProductDetail.Product()
                {
                    Productlist = new List<ProductListClass>()
                };
                var qs = (from obj in context.Products
                          where obj.ProductID==ID
                          select obj).ToList();
                foreach (var x in qs)
                {
                    product.Productlist.Add(new Models.ProductDetail.ProductListClass
                    {
                        productDetail = new IntegerNullString() { Id = x.ProductID, Text = x.ProductName },
                        categorytype = new IntegerNullString() { Id = x.Category_CategoryID.CategoryID, Text = x.Category_CategoryID.CategoryName },
                        Company = x.Company,
                        Description = x.Description,
                        Type = new IntegerNullString() { Id = x.ProductUnit.UnitID, Text = x.ProductUnit.Type },
                        Variety = x.Variety,

                    });
                }

                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "ViewProduct Successful",
                    Data = product,

                };
                return result;
            }
        }

        public Result Update(Models.ProductDetail.Product value, int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var dbobj = (from obj in value.Productlist
                             select obj).SingleOrDefault();

                var pn = (from obj in context.Products
                          where obj.ProductID == id
                          select obj).SingleOrDefault();

                if (pn == null)
                {
                    throw new ArgumentException("Product doesn't exist");
                }

                if (pn.ProductName != dbobj.productDetail.Text)
                {
                    var _product = context.Products.SingleOrDefault(name => name.ProductName == dbobj.productDetail.Text);
                    if (_product != null)
                    {
                        throw new Exception("Product Alredy Exits.");
                    }
                }

                pn.ProductName = dbobj.productDetail.Text;
                pn.Variety = dbobj.Variety;
                pn.Company = dbobj.Company;
                pn.CategoryID = dbobj.categorytype.Id;
                pn.UnitID = dbobj.Type.Id;
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
                    Status = Result.ResultStatus.success,
                    Message = "Product Deleted Succesfully",
                    Data = qs.ProductName,
                };
            }
        }
    }
}
