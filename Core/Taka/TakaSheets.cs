using KarkhanaBook.Models.Common;
using KarkhanaBookContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static KarkhanaBook.Models.Common.Result;

namespace KarkhanaBook.Core.Taka
{
    public class TakaSheets
    {
        public Result Add(Models.Taka.TakaSheet value) 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext()) 
            {
                TakaSheet dbtakaSheet = new TakaSheet();
                var db = (from obj in value.Taka
                          select obj).SingleOrDefault();
                var takaCount = (from obj in context.TakaSheets
                                 where obj.TakaID == db.TakaID
                                 select obj).ToList();
                if (takaCount.Count() > 0)
                {
                    throw new Exception($"Entered TakaId : {db.TakaID} Already Exist in Database");
                }
                else
                {
                    var dbobjlist = (from obj in value.Taka
                                     select new TakaSheet()
                                     {
                                         TakaID = obj.TakaID,
                                         MachineNumber = obj.MachineNumber,
                                         Meter = obj.Meter,
                                         Weight = obj.Weight,
                                         Date = value.Date.ToLocalTime(),

                                     }).ToList();
                    context.TakaSheets.InsertAllOnSubmit(dbobjlist);
                    context.SubmitChanges();

                    var result = new Result()
                    {
                        Message = "TakaSheet Added Successfully",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK

                    };
                    return result;
                }
            }
        }
        public async Task<IEnumerable> View() 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {

                var dbobj = (from obj in context.TakaSheets
                             select obj).ToList();
                return dbobj;
            }
        }
        public async Task<IEnumerable> ViewByID(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {

                var dbobj = (from obj in context.TakaSheets
                             where obj.ID==ID
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
        public Result Update(Models.Taka.TakaSheet value, int Id) 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext()) 
            {
                var db = (from obj in value.Taka
                          select obj).SingleOrDefault();
                var dbobj = (from obj in context.TakaSheets
                             where obj.ID == Id
                             select obj).ToList();
                var takaCount = (from obj in context.TakaSheets
                                 where obj.TakaID == db.TakaID
                                 select obj).ToList();
                
                if (dbobj.Count() > 0)
                {
                    if (takaCount.Count() > 0)
                    {
                        throw new Exception($"Your Entered TakaId : {db.TakaID} Already Exist in Database");
                    }
                    else 
                    {
                        foreach (var item in dbobj)
                        {
                            item.TakaID = db.TakaID;
                            item.MachineNumber = db.MachineNumber;
                            item.Meter = db.Meter;
                            item.Weight = db.Weight;
                            item.Date = value.Date.ToLocalTime();

                            context.SubmitChanges();
                        }
                            var result = new Result()
                            {
                                Message = "TakaSheet Update Successfully",
                                Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                                 , true))).ToString(),
                                StatusCode = (int)HttpStatusCode.OK

                            };
                            return result;
                    }
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
                var dbobj = (from obj in context.TakaSheets
                             where obj.ID == ID
                             select obj).ToList();
                if (dbobj.Count() > 0)
                {
                    context.TakaSheets.DeleteAllOnSubmit(dbobj);
                    context.SubmitChanges();
                    var result = new Result()
                    {
                        Message = "TakaSheet Deleted Successfully",
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
    }
}
