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
                           Unit = (string)p.type.Text,
                           TotalProductQuantity = 0
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
                Message = string.Format($"{product.ProductName} Added Successfully."),
                Status = Result.ResultStatus.success,
                Data = product.ProductName,
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
                                Unit = x.Unit,
                                Quantity = (float)x.TotalProductQuantity,
                                Text = x.ProductName,
                                Id = x.ProductID
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
                            join catid in context.Categories
                            on x.CategoryID equals catid.CategoryID
                            select new
                            {
                                ProductID = x.ProductID,
                                ProductName = x.ProductName,
                                Varitey = x.Variety,
                                Company = x.Company,
                                Description = x.Description,
                                Unit = x.Unit,
                                CategoryName = catid.CategoryName
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
                            select new
                            {
                                ProductID = x.ProductID,
                                ProductName = x.ProductName,
                                Varitey = x.Variety,
                                Company = x.Company,
                                Description = x.Description,
                                Unit = x.Unit,
                                CategoryName = catid.CategoryName
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
                Product product = new Product();
                product = context.Products.SingleOrDefault(x => x.ProductID == id);
                if (product == null)
                {
                    throw new ArgumentException("Product doesn't exist");
                }

                if (product.ProductName != dbobj.ProductName)
                {
                    var _product = context.Products.SingleOrDefault(name => name.ProductName == dbobj.ProductName);
                    if (_product != null)
                    {
                        throw new Exception("Product Alredy Exits.");
                    }
                }

                product.ProductName = dbobj.ProductName;
                product.Variety = dbobj.Variety;
                product.Company = dbobj.Company;
                product.CategoryID = dbobj.categorytype.Id;
                product.Unit = dbobj.type.Text;
                product.Description = dbobj.Description;
                context.Products.InsertOnSubmit(product);
                context.SubmitChanges();
                return new Result()
                {
                    Message = "Updated Successfully",
                    Data = product.ProductName,
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
