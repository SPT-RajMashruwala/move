using Agriculture.Middleware;
using Agriculture.Models.Common;
using Agriculture.Models.Search;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.Data;
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
                           where m.vegetable.Text.ToLower() == y.VegetableName.ToLower()
                           select new
                           {
                               VegetableName = y.VegetableName
                           }).ToList();
                if (vn1.Count() == 0)
                {
                    var vegetablename = (from m in value.ProductionLists
                                         select new Vegetable()
                                         {
                                             VegetableName = m.vegetable.Text
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
                                                         where obj.VegetableName == m.vegetable.Text
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
                Models.ProductionDetail.Production production = new Models.ProductionDetail.Production()
                {
                    ProductionLists=new List<Models.ProductionDetail.ProductionList>()
                };
                var qs = (from obj in context.ProductionDetails
                          select obj).ToList();
                foreach(var x in qs) 
                {
                    production.ProductionLists.Add(new Models.ProductionDetail.ProductionList()
                    {
                        mainAreaDetails=new IntegerNullString() { Id=x.MainArea.MainAreaID,Text=x.MainArea.MainAreaName},
                        Quantity=(float)x.Quantity,
                        Remark=x.Remark,
                        subAreaDetails= new IntegerNullString() { Id = x.SubArea.SubAreaID, Text = x.SubArea.SubAreaName },
                        vegetable = new IntegerNullString() { Id = x.Vegetable.VegetableID, Text = x.Vegetable.VegetableName },

                    });
                    production.DateTime =Convert.ToDateTime(x.DateTime);
                    production.LoginDetail = new IntegerNullString() { Id = x.LoginDetail.LoginID, Text = x.LoginDetail.UserName };
                }

                
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "View Production Details",
                    Data = production,
                };
                return result;
            }
        }
        public Result ViewSearch(DataTable table) 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext()) 
            {
                Models.ProductionDetail.Production production = new Models.ProductionDetail.Production()
                {
                    ProductionLists=new List<Models.ProductionDetail.ProductionList>()
                };
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow dr = table.Rows[i];
                    production.ProductionLists.Add(new Models.ProductionDetail.ProductionList()
                    {
                        mainAreaDetails = new IntegerNullString(){ Id = Int16.Parse(dr["MainAreaID"].ToString()), Text = dr["MainAreaName"].ToString() },
                        Quantity = float.Parse(dr["Quantity"].ToString()),
                        Remark = dr["Remark"].ToString(),
                        subAreaDetails = new IntegerNullString() { Id = Int16.Parse(dr["SubAreaID"].ToString()), Text = dr["SubAreaName"].ToString() },
                        vegetable = new IntegerNullString() { Id = Int16.Parse(dr["VegetableID"].ToString()), Text = dr["VegetableName"].ToString() },

                    });
                    production.DateTime = Convert.ToDateTime(dr["DateTime"].ToString());
                    production.LoginDetail = new IntegerNullString() { Id = Int16.Parse(dr["LoginID"].ToString()), Text = dr["UserName"].ToString() };

                }
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "View Production Details",
                    Data = production,
                };
                return result;
            }
        }
        public Result ViewById(int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Models.ProductionDetail.Production production = new Models.ProductionDetail.Production()
                {
                    ProductionLists = new List<Models.ProductionDetail.ProductionList>()
                };
                var qs = (from obj in context.ProductionDetails
                          where obj.ProductionID==id 
                          select obj).ToList();
                foreach (var x in qs)
                {
                    production.ProductionLists.Add(new Models.ProductionDetail.ProductionList()
                    {
                        mainAreaDetails = new IntegerNullString() { Id = x.MainArea.MainAreaID, Text = x.MainArea.MainAreaName },
                        Quantity = (float)x.Quantity,
                        Remark = x.Remark,
                        subAreaDetails = new IntegerNullString() { Id = x.SubArea.SubAreaID, Text = x.SubArea.SubAreaName },
                        vegetable = new IntegerNullString() { Id = x.Vegetable.VegetableID, Text = x.Vegetable.VegetableName },

                    });
                    production.DateTime = Convert.ToDateTime(x.DateTime);
                    production.LoginDetail = new IntegerNullString() { Id = x.LoginDetail.LoginID, Text = x.LoginDetail.UserName };
                }


                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "View Production Details",
                    Data = production,
                };
                return result;
            }
        }

        public Result Update(Models.ProductionDetail.Production value, int id)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                MAC login = new MAC();
                ProductionDetail productionDetail = new ProductionDetail();
                var UserMACAddress = login.GetMacAddress().Result;
                var mac = context.LoginDetails.FirstOrDefault(c => c.SystemMac == UserMACAddress);
                var vn1 = (from m in value.ProductionLists
                           from y in context.Vegetables
                           where m.vegetable.Text.ToLower() == y.VegetableName.ToLower()
                           select new
                           {
                               VegetableName = y.VegetableName
                           }).ToList();
                if (vn1.Count() == 0)
                {
                    var vegetablename = (from m in value.ProductionLists
                                         select new Vegetable()
                                         {
                                             VegetableName = m.vegetable.Text
                                         }).ToList();
                    context.Vegetables.InsertAllOnSubmit(vegetablename);
                    context.SubmitChanges();
                }

                var vg = (from m in value.ProductionLists
                          from y in context.Vegetables
                          where m.vegetable.Text.ToLower() == y.VegetableName.ToLower()
                          select new
                          {
                              VegetableID = y.VegetableID
                          }).ToList();


                var qs = (from obj in value.ProductionLists
                          select obj).SingleOrDefault();

                var pd = (from obj in context.ProductionDetails
                          where obj.ProductionID == id
                          select obj).SingleOrDefault();

                if (pd.SubAreaID == qs.subAreaDetails.Id)
                {

                    pd.MainAreaID = qs.mainAreaDetails.Id;
                    pd.SubAreaID = qs.subAreaDetails.Id;
                    pd.VegetableID = (from obj in vg
                                      select obj.VegetableID).SingleOrDefault();
                    pd.Quantity = qs.Quantity;
                    pd.DateTime = DateTime.Now;
                    pd.LoginID = mac.LoginID;
                    pd.Remark = qs.Remark;



                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = "Production Updated Successfully",
                        Status = Result.ResultStatus.success,
                        Data = qs.vegetable.Text
                    };
                }
                else
                {
                    var q = (from obj2 in value.ProductionLists
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
                        pd.Quantity = qs.Quantity;
                        pd.DateTime = DateTime.Now;
                        pd.LoginID = mac.LoginID;
                        pd.Remark = qs.Remark;



                        context.SubmitChanges();
                        return new Result()
                        {
                            Message = "Production Updated Successfully",
                            Status = Result.ResultStatus.success,
                            Data = qs.vegetable.Text
                        };
                    }

                }
            }

        }
    }
}
