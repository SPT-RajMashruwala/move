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
    public class ProductionRepository : IProductionRepository
    {
        public Result AddProductionDetails(ProductionModel productionModel)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                ProductionDetail productionDetail = new ProductionDetail();
                UserLoginDetails login = new UserLoginDetails();
                ProductionModel pm = new ProductionModel();
                var UserMACAddress = login.GetMacAddress().Result;

              
                var vn1 = (from m in productionModel.productionLists
                           from y in context.Vegetables
                           where m.vegetablenm.ToLower() == y.VegetableName.ToLower()
                           select new
                           {
                               VegetableName = y.VegetableName
                           }).ToList();
                if (vn1.Count() == 0)
                {
                    var vegetablename = (from m in productionModel.productionLists
                                         select new Vegetable()
                                    {
                                        VegetableName=m.vegetablenm
                                    }).ToList();
                    context.Vegetables.InsertAllOnSubmit(vegetablename);
                    context.SubmitChanges();
                }
              
                var mac = context.LoginDetails.FirstOrDefault(c => c.SystemMac == UserMACAddress);
                var pd = (from obj in pm.productionLists
                          select obj).ToList();
         
                var productionlist = (from m in productionModel.productionLists

                                      select new ProductionDetail()
                                      {
                                          MainAreaID = m.mainAreaDetails.Id,
                                          SubAreaID = m.subAreaDetails.Id,
                                          VegetableID = (from obj in context.Vegetables
                                                         where obj.VegetableName == m.vegetablenm
                                                         select obj.VegetableID).SingleOrDefault(),
                                          QuantityOfVegetable = m.Quantity,
                                          Remark = m.Remark,
                                          DateTime = DateTime.Now,
                                          UserLoginID = mac.LoginID

                                      }).ToList();
                context.ProductionDetails.InsertAllOnSubmit(productionlist);
                context.SubmitChanges();



                    return new Result()
                {
                    Message = string.Format($"Production details Added Successfully."),
                    Status = Result.ResultStatus.success,
                    Data = DateTime.Now,
                };
            }
        }
        public async Task<IEnumerable> GetProductionDetails()
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            ProductionDetail p = new ProductionDetail();
            MainArea m = new MainArea();

       /*     var man = (from obj in context.ProductionDetails
                       join obj2 in context.MainAreas
                       on obj.MainAreaID equals obj2.MainAreaID into JoinTable
                       from jt in JoinTable.DefaultIfEmpty()
                       select jt.MainAreaName).ToList();
                      */
     
            var qs = (from obj in context.ProductionDetails
                      join obj2 in context.MainAreas
                      on obj.MainAreaID equals obj2.MainAreaID into JoinTableMA
                      from MA in JoinTableMA.DefaultIfEmpty()
                      join obj3 in context.SubAreas
                      on obj.SubAreaID equals obj3.SubAreaID into JoinTableSA
                      from SA in JoinTableSA.DefaultIfEmpty()
                      join obj4 in context.Vegetables
                      on obj.VegetableID equals obj4.VegetableID into JoinTableVN
                      from VN in JoinTableVN.DefaultIfEmpty()
                      select new
                      {
                       
                          ProductionID=obj.ProductID,
                          MainAreaName = MA.MainAreaName,

                          SubAreaName = SA.SubAreaName,
                          VegetableName = VN.VegetableName,
                          QuantityOfVegetable = obj.QuantityOfVegetable

                      }).ToList();
            return qs;


        }
        public async Task<IEnumerable> GetProductionDetailsById(int id)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            
            var qs = (from obj in context.ProductionDetails
                      join obj2 in context.MainAreas
                      on obj.MainAreaID equals obj2.MainAreaID into JoinTableMA
                      from MA in JoinTableMA.DefaultIfEmpty()
                      join obj3 in context.SubAreas
                      on obj.SubAreaID equals obj3.SubAreaID into JoinTableSA
                      from SA in JoinTableSA.DefaultIfEmpty()
                      join obj4 in context.Vegetables
                      on obj.VegetableID equals obj4.VegetableID into JoinTableVN
                      from VN in JoinTableVN.DefaultIfEmpty()
                      where obj.ProductID == id
                      select new
                      {
                          
                          MainAreaName = MA.MainAreaName,

                          SubAreaName = SA.SubAreaName,
                          VegetableName = VN.VegetableName,
                          QuantityOfVegetable = obj.QuantityOfVegetable

                      }).ToList();
            return qs;


        }
        public Result UpdateProductionDetails(ProductionModel productionModel, int productionID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                UserLoginDetails login = new UserLoginDetails();
                ProductionDetail productionDetail = new ProductionDetail();
                var UserMACAddress = login.GetMacAddress().Result;
                var mac = context.LoginDetails.FirstOrDefault(c => c.SystemMac == UserMACAddress);
                var vn1 = (from m in productionModel.productionLists
                           from y in context.Vegetables
                           where m.vegetablenm.ToLower() == y.VegetableName.ToLower()
                           select new
                           {
                               VegetableName = y.VegetableName
                           }).ToList();
                if (vn1.Count() == 0)
                {
                    var vegetablename = (from m in productionModel.productionLists
                                         select new Vegetable()
                                         {
                                             VegetableName = m.vegetablenm
                                         }).ToList();
                    context.Vegetables.InsertAllOnSubmit(vegetablename);
                    context.SubmitChanges();
                }

                var vg = (from m in productionModel.productionLists
                          from y in context.Vegetables
                          where m.vegetablenm.ToLower() == y.VegetableName.ToLower()
                          select new
                          {
                              VegetableID=y.VegetableID
                          }).ToList();


                var qs = (from obj in productionModel.productionLists
                          select obj).SingleOrDefault();
                
                var pd = (from obj in context.ProductionDetails
                          where obj.ProductID == productionID
                          select obj).SingleOrDefault();

                if (pd.SubAreaID == qs.subAreaDetails.Id)
                {

                    pd.MainAreaID = qs.mainAreaDetails.Id;
                    pd.SubAreaID = qs.subAreaDetails.Id;
                    pd.VegetableID = (from obj in vg
                                      select obj.VegetableID).SingleOrDefault();
                    pd.QuantityOfVegetable = qs.Quantity;
                    pd.DateTime = DateTime.Now;
                    pd.UserLoginID = mac.LoginID;
                    pd.Remark = qs.Remark;



                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = "Production Updated Successfully",
                        Status = Result.ResultStatus.success,
                        Data = qs.vegetablenm,
                    };
                }
                else 
                {
                    var q = (from obj2 in productionModel.productionLists
                             from obj in context.ProductionDetails
                              where obj2.subAreaDetails.Id == obj.SubAreaID
                              select obj).ToList();
                    if (q.Count() > 0)
                    {
                        throw new ArgumentException($"Main Area already under production");
                    }
                    else 
                    {
                        pd.MainAreaID = qs.mainAreaDetails.Id;
                        pd.SubAreaID = qs.subAreaDetails.Id;
                        pd.VegetableID = (from obj in vg
                                          select obj.VegetableID).SingleOrDefault();
                        pd.QuantityOfVegetable = qs.Quantity;
                        pd.DateTime = DateTime.Now;
                        pd.UserLoginID = mac.LoginID;
                        pd.Remark = qs.Remark;



                        context.SubmitChanges();
                        return new Result()
                        {
                            Message = "Production Updated Successfully",
                            Status = Result.ResultStatus.success,
                            Data = qs.vegetablenm,
                        };
                    }
                
                }
            }
        }




    }
}


