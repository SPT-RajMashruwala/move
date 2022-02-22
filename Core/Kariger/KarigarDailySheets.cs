using KarkhanaBook.Models.Common;
using KarkhanaBookContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static KarkhanaBook.Models.Common.Result;

namespace KarkhanaBook.Core.Kariger 
{
    public class KarigarDailySheets
    {
        public Result Add(Models.Kariger.KarigarDailySheet value) 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext()) 
            {
                KarigerDailySheet dbkarigerDailySheet = new KarigerDailySheet();
                var dblist = (from obj in value.machine
                              select new KarigerDailySheet()
                              {
                                  
                                  UserName=value.UserName,
                                  AVGOfMachine=obj.AVGOfMachine,
                                  MachineNumber=obj.MachineNumber,
                                  Shift=value.Shift,
                                  Date=value.Date.ToLocalTime()
                                 
                              }).ToList();

                context.KarigerDailySheets.InsertAllOnSubmit(dblist);
                context.SubmitChanges();

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
        public async Task<IEnumerable> View() 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext()) 
            {
                var dbobj = (from obj in context.KarigerDailySheets
                             select obj).ToList();
                return dbobj;
            }
        }
        public async Task<IEnumerable> ViewById(int ID) 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.KarigerDailySheets
                             where obj.IndexNumber==ID
                             select obj).ToList();
                if (dbobj.Count() > 0)
                {
                    return dbobj;
                }
                else 
                {
                    throw new Exception("Your Entered ID Doesnt Exist in Database");
                }
            }
        }
        public Result Update(Models.Kariger.KarigarDailySheet value,int ID) 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext()) 
            {
                var db = (from obj in value.machine
                          select obj).SingleOrDefault();
                var dbobj = (from obj in context.KarigerDailySheets
                             where obj.IndexNumber == ID
                             select obj).ToList();

                if (dbobj.Count() > 0)
                {
                    foreach (var item in dbobj) 
                    {
                        item.UserName = value.UserName;
                        item.AVGOfMachine = db.AVGOfMachine;
                        item.MachineNumber = db.MachineNumber;
                        item.Shift = value.Shift;
                        item.Date = value.Date.ToLocalTime();


                         context.SubmitChanges();
                    }
                    var result = new Result()
                    {
                        Message = "KarigerDaily Sheet Updated Successfully",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK

                    };
                    return result;

                }
                else
                {
                    throw new Exception("Your Entered ID Doesnt Exist in Database");
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
