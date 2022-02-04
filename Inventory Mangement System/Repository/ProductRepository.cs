using Inventory_Mangement_System.Middleware;
using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using Microsoft.AspNetCore.JsonPatch;
using ProductInventoryContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Repository
{
    public class ProductRepository : IProductRepository
    {
        public Result AddProduct(ProductModel productModel)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            Category category = new Category();
            Product product = new Product();
            UserLoginDetails login = new UserLoginDetails();
            var MacAddress = login.GetMacAddress().Result;

            var pname = context.Products.Where(name => name.ProductName == productModel.ProductName).SingleOrDefault();
            if (pname != null)
            {
                throw new ArgumentException("Alredy Exits");
            }
            product.ProductName = productModel.ProductName;
            product.Variety = productModel.Variety;
            product.Company = productModel.Company;
            product.Description = productModel.Description;
            product.Unit = (string)productModel.type.Text;
            product.TotalProductQuantity = 0;
            product.CategoryID = (int)productModel.categorytype.Id;
            product.Remark = productModel.Remark;
            product.UserLoginID = (from obj in context.LoginDetails
                                   where obj.SystemMac == MacAddress
                                   select obj.LoginID).SingleOrDefault();
            product.DateTime = DateTime.Now;
            context.Products.InsertOnSubmit(product);
            context.SubmitChanges();
            //return $"{productModel.ProductName} Added Successfully";
            return new Result()
            {
                Message = string.Format($"{productModel.ProductName} Added successfully!"),
                Status = Result.ResultStatus.success,
                Data= productModel.ProductName,
            };
        }
        public Result UpdateProduct(JsonPatchDocument productModel,int productID)
        {
            using(ProductInventoryDataContext context=new ProductInventoryDataContext())
            {
                Product product = new Product();
                product = context.Products.SingleOrDefault(id => id.ProductID == productID);
                if (product == null)
                {
                    throw new Exception("");
                }
                productModel.ApplyTo(product);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format("fully!"),
                    Status = Result.ResultStatus.success,
                    //Data = product,
                };
            }
        }
        public async Task<IEnumerable> GetUnit()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                return (from x in context.ProductUnits 
                        select new IntegerNullString()
                        {
                            Text = x.Type ,
                            Id = x.UnitID 
                        }).ToList();
            }
        }
    }
}
