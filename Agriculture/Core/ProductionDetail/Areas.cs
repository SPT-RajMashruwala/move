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
    public class Areas
    {
        public List<SearchArea> searchAreas = new List<SearchArea>();
        //New Main Area Add 
        public Result Add(Models.ProductionDetail.Area value)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                MAC mac = new MAC();
                var MacObj = mac.GetMacAddress().Result;
                var macAddress = context.LoginDetails.FirstOrDefault(x => x.SystemMac == MacObj);

                MainArea mainArea = new MainArea();
                var mn1 = (from m in value.arealist
                           from y in context.MainAreas
                           where m.mainArea.Text == y.MainAreaName
                           select new
                           {
                               MainAreaID = y.MainAreaID,
                               MainAreaname = y.MainAreaName
                           }).ToList();
                foreach (var item in mn1)
                {
                    if (mn1.Count() > 0)
                    {
                        throw new ArgumentException($"MainAreaName {item.MainAreaname} already Exist");
                    }
                };

                var mainarea = (from m in value.arealist
                                select new MainArea()
                                {
                                    MainAreaName = m.mainArea.Text,
                                   
                                    LoginID=macAddress.LoginID,
                                    DateTime=DateTime.Now
                                    
                                }).ToList();
                context.MainAreas.InsertAllOnSubmit(mainarea);
                context.SubmitChanges();

                var mainarea2 = (from m in mainarea
                                 select new
                                 {
                                     MainAreaID = m.MainAreaID,
                                     MainAreaname = m.MainAreaName
                                 }).ToList();

                foreach (var item in mainarea2)
                {
                    var SD1 = (from m in value.arealist
                             
                               from y in m.subarealist
                               where m.mainArea.Text == item.MainAreaname
                               select new SubArea()
                               {
                                   MainAreaID = item.MainAreaID,
                                   
                                   SubAreaName = y.subArea.Text,
                                   LoginID = macAddress.LoginID,
                                   DateTime = DateTime.Now

                               }).ToList();
                    context.SubAreas.InsertAllOnSubmit(SD1);
                    context.SubmitChanges();
                }
                return new Result()
                {
                    Message = string.Format($"Area Added Successfully."),
                    Status = Result.ResultStatus.success,
                };

                //foreach (var item in mn1)
                //{
                //    var SA = (from m in context.MainAreas
                //              join s in context.SubAreas
                //              on m.MainAreaID equals s.MainAreaID
                //              where m.MainAreaID == item.MainAreaID
                //              select new
                //              {
                //                  MainAreaID = m.MainAreaID,
                //                  SubAreaName = s.SubAreaName
                //              }).ToList();

                //    var sd = (from m in areaModel.arealist
                //              from y in m.subarea
                //              where m.mname == item.MainAreaname
                //              select new
                //              {
                //                  MainAreaID = item.MainAreaID,
                //                  SubAreaName = y.sname
                //              }).ToList().Except(SA);

                //    var _sd = (from m in sd
                //               from y in areaModel.arealist
                //               where y.mname == item.MainAreaname
                //               select new SubArea()
                //               {
                //                   MainAreaID = item.MainAreaID,
                //                   SubAreaName = m.SubAreaName
                //               }).ToList();

                //    if (_sd.Count() == 0)
                //    {
                //        throw new ArgumentException($"MainAreaName {item.MainAreaname} already have SubAreaN{_sd}");
                //    }
                //    context.SubAreas.InsertAllOnSubmit(_sd);
                //    context.SubmitChanges();
                //}
                //return new Result()
                //{
                //    Message = string.Format($"SubArea Added Successfully."),
                //    Status = Result.ResultStatus.success,
                //}; 
            }
        }
        //GetMainArea Dropdown
        public Result GetMainArea()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Get MainArea",
                    Data = (from x in context.MainAreas
                            select new IntegerNullString()
                            {
                                Id = x.MainAreaID,
                                Text = x.MainAreaName,
                            }).ToList(),
                };
                return result;
            }
        }

        //GetSubArea Dropdown
        public Result GetSubArea(int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var result = new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "GetSubArea By MainAreaId",
                    Data = (from x in context.SubAreas
                            where x.MainAreaID == ID
                            select new IntegerNullString()
                            {
                                Id = x.SubAreaID,
                                Text = x.SubAreaName,
                            }).ToList(),
                };
                return result;
            }
        }
        public Result ViewSubArea() 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Models.ProductionDetail.MainAreaModel subarea = new Models.ProductionDetail.MainAreaModel()
                {
                    subarealist = new List<Models.ProductionDetail.SubAreaModel>()
                };
             

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "View SubArea Successful",
                    Data = (from obj in context.SubAreas
                            select new 
                            {
                                SubArea=new IntegerNullString() {Id=obj.SubAreaID,Text=obj.SubAreaName },
                                MainArea= new IntegerNullString() {Id=(from maID in context.MainAreas
                                                                       where maID.MainAreaID==obj.MainAreaID
                                                                       select maID.MainAreaID).SingleOrDefault(),
                                                                   Text= (from ma in context.MainAreas
                                                                          where ma.MainAreaID == obj.MainAreaID
                                                                          select ma.MainAreaName).SingleOrDefault()

                                                                        }, 
                                Remark = obj.Remark,
                                UserName = (from ld in context.LoginDetails
                                            where ld.LoginID == obj.LoginID
                                            select ld.UserName).SingleOrDefault()
                            }).ToList(),
                };
            }
        }
        public Result ViewSearch(DataTable table) 
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow dr = table.Rows[i];
                    searchAreas.Add(new Models.Search.SearchArea
                    {
                       
                        SubAreaID= Int16.Parse(dr["SubAreaID"].ToString()),
                        SubAreaName= dr["SubAreaName"].ToString(),
                        MainAreaName= dr["MainAreaName"].ToString(),
                        Remark = dr["Remark"].ToString(),
                        UserName = dr["UserName"].ToString(),
                        DateTime = Convert.ToDateTime(dr["DateTime"].ToString()),
                        


                    });

                }
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "View SubArea Successful",
                    Data = searchAreas,
                };
            }
        }
        public Result ViewSubAreaByID(int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "View SubArea By ID Successful",
                    Data = (from obj in context.SubAreas
                            where obj.SubAreaID==ID
                            select new
                            {
                                SubArea = new IntegerNullString() { Id = obj.SubAreaID, Text = obj.SubAreaName },
                                MainArea = new IntegerNullString()
                                {
                                    Id = (from maID in context.MainAreas
                                          where maID.MainAreaID == obj.MainAreaID
                                          select maID.MainAreaID).SingleOrDefault(),
                                    Text = (from ma in context.MainAreas
                                            where ma.MainAreaID == obj.MainAreaID
                                            select ma.MainAreaName).SingleOrDefault()

                                },
                                Remark = obj.Remark,
                                UserName = (from ld in context.LoginDetails
                                            where ld.LoginID == obj.LoginID
                                            select ld.UserName).SingleOrDefault()
                            }).ToList(),
                };
            }
        }
        public Result ViewMainArea()
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "View MainArea Successful",
                    Data = (from obj in context.MainAreas
                            select new
                            {
                                MainArea=new IntegerNullString() { Id=obj.MainAreaID,Text=obj.MainAreaName},
                                Remark=obj.Remark,
                                UserName=(from ld in context.LoginDetails
                                         where ld.LoginID==obj.LoginID
                                         select ld.UserName).SingleOrDefault(),
                                DateTime=obj.DateTime,
                                
                            }).ToList(),
                };
            }
        }
      /*  public Result Update(Models.ProductionDetail.Area value, int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var sadbobj = (from obj in context.SubAreas
                               where obj.SubAreaID == ID
                               select obj).SingleOrDefault();
                var qs = (from obj in value.arealist
                          select obj).SingleOrDefault();
                var q = (from obj in qs.subarea
                         select obj).SingleOrDefault();
                var maID = (from obj in context.MainAreas
                              where obj.MainAreaName == qs.mname
                              select obj.MainAreaID).SingleOrDefault();
                var saID = (from obj in context.SubAreas
                            from obj1 in context.MainAreas
                            where obj.SubAreaName == q.sname && obj1.MainAreaName == qs.mname
                            select obj.SubAreaID).SingleOrDefault();
                if (sadbobj.MainAreaID == maID)
                {
               *//*     if (sadbobj.SubAreaID==saID) 
                    {
                        
                        sadbobj.Remark=value.
                    }*//*
                }
            }

        }*/
        public Result DeleteSubArea(int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext()) 
            {
                var qs = (from obj in context.SubAreas
                          where obj.SubAreaID == ID
                          select obj).SingleOrDefault();
                context.SubAreas.DeleteOnSubmit(qs);
                context.SubmitChanges();
                return new Result()
                {
                    Status=Result.ResultStatus.success,
                    Message="Area Deleted Succesfully",
                    Data=$"SubArea : {qs.SubAreaName} Deleted Successfully ",
                };
            }
        }
        public Result DeleteMainArea(int ID)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                var qs = (from obj in context.MainAreas
                          where obj.MainAreaID == ID
                          select obj).SingleOrDefault();
                var dblist = (from obj in context.SubAreas
                              where obj.MainAreaID == qs.MainAreaID
                              select obj).ToList();
                context.MainAreas.DeleteOnSubmit(qs);
                context.SubAreas.DeleteAllOnSubmit(dblist);
                context.SubmitChanges();

                return new Result()
                {
                    Status=Result.ResultStatus.success,
                    Message="Main Area Along With Allocated Subarea succesfully Deleted",
                    Data=$"Main Area : {qs.MainAreaID} and its all Subarea Deleted ",
                };
            }
        }
    }
}
