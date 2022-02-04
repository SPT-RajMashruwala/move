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

                /*var v = (from x in context.Vegetables
                         from m in productionModel.productionLists
                         where x.VegetableName.ToLower() == m.vegetablenm
                         select x).ToList();*/
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
                /*if (v==null)
                {
                    var qs = (from obj in pm.productionLists
                              select obj).ToList();
                    foreach (var item in qs) {
                        Vegetable veg = new Vegetable()
                        {

                            VegetableName = item.vegetablenm

                        };
                    context.Vegetables.InsertOnSubmit(veg);
                    context.SubmitChanges();
                }
                }*/
                var mac = context.LoginDetails.FirstOrDefault(c => c.SystemMac == UserMACAddress);
                var pd = (from obj in pm.productionLists
                          select obj).ToList();
                /*  foreach (var item in pd)
                  {
                      var vg = (from obj in context.Vegetables
                                where obj.VegetableName == item.vegetablenm
                                select obj.VegetableID).SingleOrDefault();
                      productionDetail.MainAreaID = item.mainAreaDetails.Id;
                      productionDetail.SubAreaID = item.subAreaDetails.Id;
                      productionDetail.VegetableID = vg;
                      productionDetail.QuantityOfVegetable = item.Quantity;
                      productionDetail.Remark = item.Remark;
                      productionDetail.DateTime = DateTime.Now;
                      productionDetail.UserLoginID = mac.LoginID;
                      context.ProductionDetails.InsertAllOnSubmit(productionDetail);
                      context.SubmitChanges();
                  }*/
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
        /*public Result UpdateProductionDetails(ProductionModel productionModel, int productionID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                ProductionDetail productionDetail = new ProductionDetail();
                var v = (from x in context.Vegetables
                         where x.VegetableName.ToLower() == productionModel.vegetablenm.ToLower()
                         select x).FirstOrDefault();
                if (v == null)
                {
                    v = new Vegetable()
                    {
                        VegetableName = productionModel.vegetablenm
                    };
                    context.Vegetables.InsertOnSubmit(v);
                    context.SubmitChanges();
                }
                int vg = (from obj in context.Vegetables
                          where obj.VegetableName == productionModel.vegetablenm
                          select obj.VegetableID).SingleOrDefault();


                var qs = (from obj in context.ProductionDetails
                          where obj.ProductID == productionID
                          select obj).SingleOrDefault();
                if (qs.SubAreaID == productionModel.subAreaDetails.Id)
                {

                    qs.MainAreaID = productionModel.mainAreaDetails.Id;
                    qs.SubAreaID = productionModel.subAreaDetails.Id;
                    qs.VegetableID = vg;
                    qs.QuantityOfVegetable = productionModel.Quantity;



                    context.ProductionDetails.InsertOnSubmit(productionDetail);
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = "Production Updated Successfully",
                        Status = Result.ResultStatus.success,
                        Data = productionModel.Quantity
                    };
                }
                else ();
            }
        }*/



          
    }
}


