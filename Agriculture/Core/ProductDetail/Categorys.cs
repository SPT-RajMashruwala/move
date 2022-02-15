using Agriculture.Models.Common;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Core.ProductDetail
{
    public class Categorys
    {
        public Result Add(Models.ProductDetail.Category value)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            Category category = new Category();
            var res = context.Categories.FirstOrDefault(x => x.CategoryName == value.CategoryName);
            if (res != null)
            {
                throw new ArgumentException("Category Already Exist");
            }
            else
            {
                category.CategoryName = value.CategoryName;
                category.Description = value.Description;
                context.Categories.InsertOnSubmit(category);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Category {value.CategoryName} Added Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = value.CategoryName,
                };
            }
        }

        public  Result Get()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var result = new Result()
                {

                    Status = Result.ResultStatus.success,
                    Message = "GetCategory successful",
                    Data= (from x in context.Categories
                           select new IntegerNullString()
                           {
                               Id = x.CategoryID,
                               Text = x.CategoryName,
                           }).ToList()

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
                    Message = "ViewCategory Successfully",
                    Data = (from x in context.Categories
                            select new
                            {
                                CategoryID = x.CategoryID,
                                CategoryName = x.CategoryName,
                                Description = x.Description
                            }).ToList()
                };
                return result;
            }
        }

        public Result ViewById(int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "CategoryViewByID successfully",
                    Data = (from x in context.Categories
                            where x.CategoryID == ID
                            select new
                            {
                                CategoryName = x.CategoryName,
                                Description = x.Description
                            }).ToList()
                };
                return result;
            }
        }

        public Result update(Models.ProductDetail.Category value, int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var ck = context.Categories.SingleOrDefault(x => x.CategoryID == id);
                if (ck == null)
                {
                    throw new ArgumentException("Category doesn't Exist");
                }
                if (ck.CategoryName != value.CategoryName)
                {
                    var _c = context.Categories.SingleOrDefault(x => x.CategoryName == value.CategoryName);
                    if (_c != null)
                    {
                        throw new ArgumentException("Category already Exist");

                    }
                }

                ck.CategoryName = value.CategoryName;
                ck.Description = value.Description;
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Category {value.CategoryName} Update Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = value.CategoryName,
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
                var data = qs.CategoryName;
                context.Categories.DeleteOnSubmit(qs);
                context.SubmitChanges();
                return new Result()
                {
                    Status=Result.ResultStatus.success,
                    Message="Category Deleted Successful",
                    Data=data,

                };
            }
        }
    }
}
