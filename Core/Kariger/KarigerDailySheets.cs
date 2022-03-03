using KarKhanaBook.Model.Common;
using KarKhanaBook.Model.Search;
using KarkhanaBookContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static KarKhanaBook.Model.Common.Result;

namespace KarKhanaBook.Core.Kariger
{
    public class KarigerDailySheets
    {
            Core.Common.Shift shift = new Common.Shift();


        public Result Search(DataTable table)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
               
                List<SearchKarigerDailySheet> karigersheet = new List<SearchKarigerDailySheet>();
                
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow dr = table.Rows[i];
                 
                    
                    karigersheet.Add(new SearchKarigerDailySheet()
                    {
                        IndexNumber = Int16.Parse(dr["IndexNumber"].ToString()),
                        UserName = new IntegerNullString() { ID = Int16.Parse(dr["UserID"].ToString()), Text = dr["UserName"].ToString() },
                        Shift = new IntegerNullString() { ID = Int16.Parse(dr["ShiftID"].ToString()), Text = dr["Shift"].ToString() },
                        Date = Convert.ToDateTime(dr["Date"].ToString()),
                        AVGOfMachine = Int16.Parse(dr["AVGOfMachine"].ToString()),
                        MachineNumber= Int16.Parse(dr["MachineNumber"].ToString()),
                        Remark= dr["Remark"].ToString(),



                    });

                 }
                    return new Result()
                    {
                        Message = "Search Successfully",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK,
                        Data =karigersheet ,
                    };
            }
        }


        public Result GetUser()
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                return new Result()
                {
                    Message = "UserName List get Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = (from obj in context.Users
                            select new { obj.UserID, obj.UserName }).ToList()
                };
            };

        }
        public Result GetShift()
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                Core.Common.Shift shift = new Common.Shift();

                return new Result()
                {
                    Message = "Shift get Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = (from obj in shift.shifts
                            select new { obj.ID, obj.Text }).ToList()

                };
            };

        }
        public Result Add(Model.Kariger.KarigerDailySheet value)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                KarigerDailySheet dbkarigerDailySheet = new KarigerDailySheet();

                var dblist = (from obj in value.machine
                              select new KarigerDailySheet()
                              {

                                  UserID = (from u in context.Users
                                            where u.UserName == value.UserName.Text
                                            select u.UserID).SingleOrDefault(),
                                  ShiftID = (from s in shift.shifts
                                            where s.Text==value.Shift.Text
                                            select s.ID).SingleOrDefault(),
                                  Date = value.Date.ToLocalTime(),
                                  AVGOfMachine = obj.AVGOfMachine,
                                  MachineNumber = obj.MachineNumber,
                                  Remark=value.Remark,
                              }).ToList();
                foreach (var item in dblist) 
                {
                    var q = (from obj in context.KarigerDailySheets
                             where obj.UserID == item.UserID && obj.ShiftID == item.ShiftID && obj.Date == item.Date && obj.MachineNumber == item.MachineNumber && obj.AVGOfMachine == item.AVGOfMachine
                             select obj).ToList();
                    if (q.Count() > 0)
                    {
                        throw new ArgumentException($"Your Entered Detail for UserID : {item.UserID}" +
                            $" and MachineNumber : {item.MachineNumber} already exist!");
                    }
                    else 
                    {
                        var qs = (from obj in context.KarigerDailySheets
                                  where obj.UserID == item.UserID && obj.ShiftID == item.ShiftID && obj.Date == item.Date && obj.MachineNumber == item.MachineNumber
                                  select obj).ToList();
                        if (qs.Count() > 0)
                        {
                            throw new ArgumentException("Please Enter Valid Details for UserID : {item.User.UserName}" +
                            $"and MachineNumber : {item.MachineNumber}");
                        }
                        else 
                        {
                            context.KarigerDailySheets.InsertAllOnSubmit(dblist);
                            context.SubmitChanges();
                        }
                    }
                }

               

                var result = new Result()
                {
                    Message = "KarigerDaily Sheet Added Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK

                };
                return result;



            }
        }
        public Result View()
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {

                return new Result()
                {
                    Message = "KarigerDaily Sheet Viewd Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = (from obj in context.KarigerDailySheets
                            select new
                            {
                                UserName = (from u in context.Users
                                            where u.UserID == obj.UserID
                                            select u.UserName).SingleOrDefault(),
                                Shift = new IntegerNullString() { ID=(int)obj.ShiftID,Text= (from s in shift.shifts
                                                                                             where s.ID == obj.ShiftID
                                                                                             select s.Text).SingleOrDefault(),
                                },
                                
                                        
                                Date = obj.Date,
                                AVGOfMachine = obj.AVGOfMachine,
                                MachineNumber = obj.MachineNumber,
                                Remark = obj.Remark,

                            }).ToList(),

                };
            }
        }
        public Result ViewById(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.KarigerDailySheets
                             where obj.IndexNumber == ID
                             select obj).ToList();

                if (dbobj.Count() > 0)
                {


                    return new Result()
                    {
                        Message = "KarigerDaily Sheet Viewd Successfully",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK,
                        Data = (from obj in dbobj
                                select new
                                {
                                    UserName = (from u in context.Users
                                                where u.UserID == obj.UserID
                                                select u.UserName).SingleOrDefault(),
                                    Shift = new IntegerNullString()
                                    {
                                        ID = (int)obj.ShiftID,
                                        Text = (from s in shift.shifts
                                                where s.ID == obj.ShiftID
                                                select s.Text).SingleOrDefault()
                                    },
                                    Date = obj.Date,
                                    AVGOfMachine = obj.AVGOfMachine,
                                    MachineNumber = obj.MachineNumber,
                                    Remark = obj.Remark,

                                }).ToList(),

                    };

                }
                else
                {
                    throw new Exception($"Your Entered ID :{ID} Doesnt Exist in Database");
                }
            }
        }
        public Result Update(Model.Kariger.KarigerDailySheet value, int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var sID = (from s in shift.shifts
                           where s.Text == value.Shift.Text
                           select s.ID).SingleOrDefault();
                var db = (from obj in value.machine
                          select obj).SingleOrDefault();
                var dbobj = (from obj in context.KarigerDailySheets
                             where obj.IndexNumber == ID
                             select obj).SingleOrDefault();
                var uId = (from obj in context.Users
                           where obj.UserName == value.UserName.Text
                           select obj.UserID).SingleOrDefault();

                if (dbobj != null)
                {
                    var qs = (from obj in context.KarigerDailySheets
                              where obj.UserID == uId && obj.ShiftID == sID && obj.Date == value.Date && obj.MachineNumber == db.MachineNumber
                              select obj).ToList();
                    var q = (from obj in context.KarigerDailySheets
                              where obj.UserID == uId && obj.ShiftID == sID && obj.Date == value.Date && obj.MachineNumber == db.MachineNumber && obj.AVGOfMachine==db.AVGOfMachine
                              select obj).ToList();
                    if (qs.Count() > 0)
                    {
                        throw new ArgumentException("Please Enter Valid Details !");
                    }
                    else 
                    {
                        if (q.Count() > 0) 
                        {
                            return new Result()
                            {
                                Message = "KarigerDaily Sheet Updated Successfully! No changes are done",
                                Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                                , true))).ToString(),
                                StatusCode = (int)HttpStatusCode.OK,
                               
                            };
                        }
                        else 
                        {
                            dbobj.UserID = uId;
                            dbobj.ShiftID = (from s in shift.shifts
                                             where s.Text == value.Shift.Text
                                             select s.ID).SingleOrDefault();
                            dbobj.Date = value.Date.ToLocalTime();
                            dbobj.AVGOfMachine = db.AVGOfMachine;
                            dbobj.MachineNumber = db.MachineNumber;
                            dbobj.Remark = value.Remark;


                            context.SubmitChanges();

                            var result = new Result()
                            {
                                Message = "KarigerDaily Sheet Updated Successfully",
                                Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                                , true))).ToString(),
                                StatusCode = (int)HttpStatusCode.OK,
                                Data = dbobj,

                            };
                            return result;
                        }
                    }


                       
                    





                }
                else
                {
                    throw new Exception($"Your Entered ID: {ID} Doesnt Exist in Database");
                }

            }
        }
        public Result Delete(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.KarigerDailySheets
                             where obj.IndexNumber == ID
                             select obj).ToList();
                if (dbobj.Count() > 0)
                {
                    context.KarigerDailySheets.DeleteAllOnSubmit(dbobj);
                    context.SubmitChanges();
                    var result = new Result()
                    {
                        Message = "KarigerDaily Sheet Deleted Successfully",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK

                    };
                    return result;
                }
                else
                {
                    throw new Exception("Your Entered Id Doesnt Exist in Database");
                }
            }
        }
    }
}