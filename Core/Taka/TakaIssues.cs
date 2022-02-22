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
    public class TakaIssues
    {
        public Result Add(Models.Taka.TakaIssue value) 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext()) 
            {
                TakaIssue dbtakaIssue = new TakaIssue();
                var db = (from obj in value.takaid
                          select obj).SingleOrDefault();
                var dbobjcount = (from obj in context.TakaSheets
                                  where obj.TakaID == db.TakaID
                                  select obj).ToList();
                if (dbobjcount.Count() > 0)
                {
                    var dbobjlist = (from obj in value.takaid
                                     select new TakaIssue()
                                     {
                                         TakaID = obj.TakaID,
                                         TakaChallanNumber = value.TakaChallanID,

                                     }).ToList();
                    context.TakaIssues.InsertAllOnSubmit(dbobjlist);
                    context.SubmitChanges();
                    var result = new Result()
                    {
                        Message = "TakaIssue Add Successful",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK

                    };
                    return result;
                }
                else 
                {
                    throw new Exception("Your entered TakaID already assign for another challan");
                }

            }
        }
        public async Task<IEnumerable> View() 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext()) 
            {
                var dbobjlist = (from obj in context.TakaIssues
                                 select obj).ToList();
                return dbobjlist;
            }
        }
        public async Task<IEnumerable> ViewByID(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobjlist = (from obj in context.TakaIssues
                                 where obj.TakaChallanNumber==ID
                                 select obj).ToList();
                if (dbobjlist.Count() > 0)
                {

                    return dbobjlist;
                }
                else 
                {
                    throw new Exception("Your Entered TakaChallanId doesnt exist in Database");
                }
            }
        }
        public Result Update(Models.Taka.TakaIssue value,int ID) 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext()) 
            {
                var dbobj = (from obj in context.TakaIssues
                             where obj.ID == ID
                             select obj).SingleOrDefault();
                var db = (from obj in context.TakaIssues
                          where obj.TakaChallanNumber == value.TakaChallanID
                          select obj).ToList();
                var d = (from obj in value.takaid
                         select obj).SingleOrDefault();

                if (db.Count() > 0)
                {
                    if (dbobj.TakaID == d.TakaID)
                    {

                        var result = new Result()
                        {
                            Message = "Some Changes Needed",
                            Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                            , true))).ToString(),
                            StatusCode = (int)HttpStatusCode.OK

                        };
                        return result;

                    }
                    else
                    {
                        foreach (var item in db)
                        {
                            if (item.TakaID == d.TakaID)
                            {
                                throw new Exception($"Your entered TakaID : {d.TakaID}" +
                                    $"already exist in ChallanID : {value.TakaChallanID}");
                            }
                            else
                            {
                                var qs = (from obj in context.TakaIssues
                                          where obj.TakaID == d.TakaID
                                          select new
                                          {
                                              TakaID = obj.TakaID,
                                              TakaChallanID = obj.TakaChallanNumber,
                                          }).SingleOrDefault();
                                if (qs != null)
                                {
                                    throw new Exception($"Your Entered TakaID : {qs.TakaID}" +
                                        $"exist in TakaChallanID : {qs.TakaChallanID}");

                                }
                                else
                                {
                                    dbobj.TakaID = d.TakaID;
                                    context.SubmitChanges();
                                    var result = new Result()
                                    {
                                        Message = "TakaIssue Update Successful",
                                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                                         , true))).ToString(),
                                        StatusCode = (int)HttpStatusCode.OK

                                    };
                                    return result;
                                }
                            }
                        }


                    }


                }
                
                
                    throw new Exception("error..............................");
            }
        }
      /*  public Result Delete(int ID) 
        {
            var dbobj
        }*/
    }

}
