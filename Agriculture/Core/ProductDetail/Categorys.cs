using Agriculture.Middleware;
using Agriculture.Models.Common;
using Agriculture.Models.Search;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Core.ProductDetail
{
    public class Categorys
    {
        public List<Models.ProductDetail.Category> searchcategory { get; set; } = new List<Models.ProductDetail.Category>();
        public Result Add(Models.ProductDetail.Category value)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            MAC mac = new MAC();
            var macObj = mac.GetMacAddress().Result;
            Category category = new Category();
            var MacAddress = context.LoginDetails.FirstOrDefault(x => x.SystemMac == macObj);
            var res = context.Categories.FirstOrDefault(x => x.CategoryName == value.categoryType.Text);
            if (res != null)
            {
                throw new ArgumentException("Category Already Exist");
            }
            else
            {
                category.CategoryName = char.ToUpper(value.categoryType.Text[0]) + value.categoryType.Text.Substring(1).ToLower();
                category.Description = value.Description;
                category.LoginID = MacAddress.LoginID;
                category.DateTime = value.DateTime;
                context.Categories.InsertOnSubmit(category);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Category {value.categoryType.Text} Added Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = value.categoryType.Text,
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
                            select new Models.ProductDetail.Category()
                            {
                                
                                categoryType=new IntegerNullString() { Id=x.CategoryID,Text=x.CategoryName},
                                loginDetail= new IntegerNullString() { Id=x.LoginDetail.LoginID,Text=x.LoginDetail.UserName},
                                
                                Description = x.Description,
                                DateTime=Convert.ToDateTime(x.DateTime),
                            }).ToList()
                };
                return result;
            }
        }
        public Result ViewSearch(DataTable table) 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow dr = table.Rows[i];
                    searchcategory.Add(new Models.ProductDetail.Category
                    {
                    categoryType=new IntegerNullString() { Id= Int16.Parse(dr["CategoryID"].ToString()),Text= dr["CategoryName"].ToString() },
                    DateTime = Convert.ToDateTime(dr["DateTime"].ToString()),
                    Description = dr["Description"].ToString(),
                    loginDetail= new IntegerNullString() { Id = Int16.Parse(dr["LoginID"].ToString()), Text = dr["UserName"].ToString() },
                    });
                        
                  

                }



                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "ViewCategory Successfully",
                    Data = searchcategory,
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
                            select new Models.ProductDetail.Category()
                            {
                                categoryType = new IntegerNullString() { Id = x.CategoryID, Text = x.CategoryName },
                                loginDetail = new IntegerNullString() { Id = x.LoginDetail.LoginID, Text = x.LoginDetail.UserName },

                                Description = x.Description,
                                DateTime = Convert.ToDateTime(x.DateTime),
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
                MAC mac = new MAC();
                var macObj = mac.GetMacAddress().Result;
                var MacAddress = context.LoginDetails.FirstOrDefault(x => x.SystemMac == macObj);
                if (ck == null)
                {
                    throw new ArgumentException("Category doesn't Exist");
                }
                if (ck.CategoryName != value.categoryType.Text)
                {
                    var _c = context.Categories.SingleOrDefault(x => x.CategoryName == value.categoryType.Text);
                    if (_c != null)
                    {
                        throw new ArgumentException("Category already Exist");

                    }
                }
               
                    ck.Description = value.Description;
                

                ck.CategoryName = value.categoryType.Text;
                
                ck.LoginID = MacAddress.LoginID;
                ck.DateTime = value.DateTime;
               
                
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"Category {value.categoryType.Text} Update Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = value.categoryType.Text,
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
