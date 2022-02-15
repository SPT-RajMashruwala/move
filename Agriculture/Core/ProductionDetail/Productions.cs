using Agriculture.Middleware;
using Agriculture.Models.Common;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Core.ProductionDetails
{
    public class Productions
    {
        public Result Add(Models.ProductionDetail.Production value)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
               /* ProductionDetail productionDetail = new ProductionDetail();*/
                MAC mac = new MAC();
               /* ProductionModel pm = new ProductionModel();*/
                var UserMACAddress = mac.GetMacAddress().Result;

                var vn1 = (from m in value.ProductionLists
                           from y in context.Vegetables
                           where m.Vegetablenm.ToLower() == y.VegetableName.ToLower()
                           select new
                           {
                               VegetableName = y.VegetableName
                           }).ToList();
                if (vn1.Count() == 0)
                {
                    var vegetablename = (from m in value.ProductionLists
                                         select new Vegetable()
                                         {
                                             VegetableName = m.Vegetablenm
                                         }).ToList();
                    context.Vegetables.InsertAllOnSubmit(vegetablename);
                    context.SubmitChanges();
                }

                var macaddress = context.LoginDetails.FirstOrDefault(c => c.SystemMac == UserMACAddress);
                var pd = (from obj in value.ProductionLists
                          select obj).ToList();
             
                
                var productionlist = (from m in value.ProductionLists
                                      select new ProductionDetail()
                                      {
                                          
                                          
                                          MainAreaID = m.mainAreaDetails.Id,
                                          SubAreaID = m.subAreaDetails.Id,
                                          VegetableID = (from obj in context.Vegetables
                                                         where obj.VegetableName == m.Vegetablenm
                                                         select obj.VegetableID).SingleOrDefault(),
                                          Quantity = m.Quantity,
                                          Remark = m.Remark,
                                          DateTime = DateTime.Now,
                                          LoginID = macaddress.LoginID,
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

            //ProductionDetail productionDetail = new ProductionDetail();
            //var v = (from x in context.Vegetables
            //         where x.VegetableName.ToLower() == productionModel.vegetablenm.ToLower()
            //         select x).FirstOrDefault();
            //if(v == null)
            //{
            //    v = new Vegetable()
            //    {
            //        VegetableName = productionModel.vegetablenm
            //    };
            //    context.Vegetables.InsertOnSubmit(v);
            //    context.SubmitChanges();
            //}
            //int vg = (from obj in context.Vegetables
            //          where obj.VegetableName == productionModel.vegetablenm
            //          select obj.VegetableID).SingleOrDefault();

            //productionDetail.MainAreaID = productionModel.mainAreaDetails.Id;
            //productionDetail.SubAreaID = productionModel.subAreaDetails.Id;
            //productionDetail.VegetableID = vg;
            //productionDetail.Quantity = productionModel.Quantity;
            //context.ProductionDetails.InsertOnSubmit(productionDetail);
            //context.SubmitChanges();
        }

        public Result View()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var qs = (from pd in context.ProductionDetails
                          join m in context.MainAreas
                          on pd.MainAreaID equals m.MainAreaID into JoinTableMA
                          from MA in JoinTableMA.DefaultIfEmpty()
                          join s in context.SubAreas
                          on pd.SubAreaID equals s.SubAreaID into JoinTableSA
                          from SA in JoinTableSA.DefaultIfEmpty()
                          join v in context.Vegetables
                          on pd.VegetableID equals v.VegetableID into JoinTableVN
                          from VN in JoinTableVN.DefaultIfEmpty()
                          select new
                          {
                              ProductionID = pd.ProductionID,
                              MainAreaName = MA.MainAreaName,
                              SubAreaName = SA.SubAreaName,
                              VegetableName = VN.VegetableName,
                              QuantityOfVegetable = pd.Quantity
                          }).ToList();
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "View Production Details",
                    Data = qs,
                };
                return result;
            }
        }

        public Result ViewById(int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var qs = (from pd in context.ProductionDetails
                          join m in context.MainAreas
                          on pd.MainAreaID equals m.MainAreaID into JoinTableMA
                          from MA in JoinTableMA.DefaultIfEmpty()
                          join s in context.SubAreas
                          on pd.SubAreaID equals s.SubAreaID into JoinTableSA
                          from SA in JoinTableSA.DefaultIfEmpty()
                          join v in context.Vegetables
                          on pd.VegetableID equals v.VegetableID into JoinTableVN
                          from VN in JoinTableVN.DefaultIfEmpty()
                          where pd.ProductionID == id
                          select new
                          {
                              MainAreaName = MA.MainAreaName,
                              SubAreaName = SA.SubAreaName,
                              VegetableName = VN.VegetableName,
                              QuantityOfVegetable = pd.Quantity
                          }).ToList();
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "View Production by ID Details",
                    Data = qs,
                };
                return result;
            }
        }

        public Result Update(Models.ProductionDetail.Production value, int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                MAC mac = new MAC();
     /*           ProductionDetail productionDetail = new ProductionDetails();*/
                var UserMACAddress = mac.GetMacAddress().Result;
                var macaddress = context.LoginDetails.FirstOrDefault(c => c.SystemMac == UserMACAddress);

                var veg = (from p in value.ProductionLists
                           from v in context.Vegetables
                           where p.Vegetablenm.ToLower() == v.VegetableName.ToLower()
                           select new
                           {
                               VegetableName = v.VegetableName
                           }).ToList();
                if (veg.Count() == 0)
                {
                    var vegetablename = (from p in value.ProductionLists
                                         select new Vegetable()
                                         {
                                             VegetableName = p.Vegetablenm
                                         }).ToList();
                    context.Vegetables.InsertAllOnSubmit(vegetablename);
                    context.SubmitChanges();
                }

                var vg = (from m in value.ProductionLists
                          from y in context.Vegetables
                          where m.Vegetablenm.ToLower() == y.VegetableName.ToLower()
                          select new
                          {
                              VegetableID = y.VegetableID
                          }).ToList();


                var qs = (from obj in value.ProductionLists
                          select obj).SingleOrDefault();

                var pd = (from obj in context.ProductionDetails
                          where obj.ProductionID == id
                          select obj).SingleOrDefault();

                return new Result()
                {
                    Message = string.Format($"Production details Added Successfully."),
                    Status = Result.ResultStatus.success,
                    Data = DateTime.Now,
                };
            }
        }
    }
}
