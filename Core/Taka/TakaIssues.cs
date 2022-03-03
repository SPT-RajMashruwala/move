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

namespace KarKhanaBook.Core.Taka
{
    public class TakaIssues
    {
        public Result Search(DataTable table) 
        {
            List<SearchTakaIssue> takaIssue = new List<SearchTakaIssue>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];


                takaIssue.Add(new SearchTakaIssue()
                {
                    TakaIssueIndex= Int16.Parse(dr["TakaIssueIndex"].ToString()),
                    TakaChallanNumber= Int16.Parse(dr["TakaChallanNumber"].ToString()),
                    TakaID= Int16.Parse(dr["TakaID"].ToString()),
                    SlotNumber= Int16.Parse(dr["SlotNumber"].ToString()),
                });

            }
            return new Result()
            {
                Message = "TakaIsuue search Successful",
                Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                          , true))).ToString(),
                StatusCode = (int)HttpStatusCode.OK,
                Data = takaIssue,
            };
        }
        public Result GetTakaID() 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var qs = (from ob in context.TakaSheets
                          select ob.TakaID).ToList();
                return new Result()
                {
                    Message = "TakaId list get Successful",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                           , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = (from obj in context.TakaSheets
                            select new { obj.TakaID}).ToList().Distinct(),
                };
            }
               
        }

        public Result GetSlotNumber(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {

                return new Result()
                {
                    Message = "SlotNumbers for entered TakaID  was got Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                           , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = (from obj in context.TakaSheets
                            where obj.TakaID==ID
                            select new { obj.SlotNumber }).ToList(),
                };
            }

        }
        public Result Add(Model.Taka.TakaIssue value)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                TakaIssue dbtakaIssue = new TakaIssue();
                var db = value.takadetails.FirstOrDefault();

                var dbobjcount = (from obj in context.TakaIssues
                                  where obj.TakaID == db.TakaID && obj.SlotNumber == db.SlotNumber
                                  select obj).SingleOrDefault();
                if (dbobjcount == null)
                {
                    var dbobjlist = (from obj in value.takadetails
                                     select new TakaIssue()
                                     {
                                        
                                         TakaChallanNumber = value.TakaChallanNumber,
                                         TakaID = obj.TakaID,
                                         SlotNumber=obj.SlotNumber,

                                     }).ToList();
                    foreach (var item in dbobjlist)
                    {

                        var slotexist_takasheet = (from obj in context.TakaSheets
                                                   where obj.SlotNumber == item.SlotNumber
                                                   select obj).ToList();
                        if (slotexist_takasheet.Count() > 0)
                        {
                            var takalistinslot = (from obj in context.TakaSheets
                                                  where obj.SlotNumber == item.SlotNumber
                                                  select obj).ToList();
                            var c = takalistinslot.FirstOrDefault(x=>x.TakaID==item.TakaID);
                            
                                if (c == null)
                                {
                                    throw new ArgumentException($"Entered TakaID : {item.TakaID} not exist under slotNumber : " +
                                        $"{item.SlotNumber}");

                                }
                             
                            
                            
                            
                        }
                        else
                        {
                            throw new ArgumentException($"Entered SlotNumber : {item.SlotNumber} not exist in TakaSheet Table.");
                        }
                    }
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
                    throw new Exception($"Your entered TakaID : {db.TakaID} under Slot Number : {db.SlotNumber} already assign for another challan : {dbobjcount.TakaChallanNumber}");
                }

            }
        }
        public Result View()
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                return new Result()
                {
                    Message = "TakaIssue Add Successful",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data= (from obj in context.TakaIssues
                           select  obj).ToList(),

                };
                
            }
        }
        public Result ViewByID(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobjlist = (from obj in context.TakaIssues
                                 where obj.TakaIssueIndex == ID
                                 select obj).ToList();
                if (dbobjlist.Count() > 0)
                {
                    return new Result()
                    {
                        Message = "TakaIssue Add Successful",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                       , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK,
                        Data = (from obj in dbobjlist
                                select obj).ToList(),

                    };
                }
                else
                {
                    throw new Exception("Your Entered Taka ChallanId doesnt exist in Database");
                }
            }
        }
        public Result Update(Model.Taka.TakaIssue value, int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var db = value.takadetails.FirstOrDefault();
                var dbobjcount = (from obj in context.TakaIssues
                                  where obj.TakaID == db.TakaID && obj.SlotNumber == db.SlotNumber
                                  select obj).SingleOrDefault();
                var dbobj = (from obj in context.TakaIssues
                             where obj.TakaIssueIndex == ID
                             select obj).SingleOrDefault();
                if (dbobj == null)
                {
                    throw new ArgumentException("Entered ID doent exist in Databse");
                }
                if (dbobjcount == null)
                {
                   
                    dbobj.TakaChallanNumber = value.TakaChallanNumber;
                    dbobj.TakaID = db.TakaID;
                    dbobj.SlotNumber = db.SlotNumber;
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = "TakaIssue Add Successful",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK,
                        Data=dbobj,

                    };
                }
                else
                {
                    throw new Exception($"Your entered TakaID : {db.TakaID} under Slot Number : {db.SlotNumber} already assign for another challan : {dbobjcount.TakaChallanNumber}");
                }
            }

            }
        }
}

            /*   using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
               {
                   var dbobj = (from obj in context.TakaIssues
                                where obj.TakaIssueIndex == ID
                                select obj).SingleOrDefault();
                   var db = (from obj in context.TakaIssues
                             where obj.TakaChallanNumber == value.TakaChallanNumber
                             select obj).ToList();
                   var d = (from obj in value.takadetails
                            select obj).SingleOrDefault();

                   if (db.Count() > 0)
                   {
                       if (dbobj.TakaID == d.TakaID)
                       {

                           return new Result()
                           {
                               Message = "Some Changes Needed",
                               Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                               , true))).ToString(),
                               StatusCode = (int)HttpStatusCode.OK
                           };

                       }
                       else
                       {
                           foreach (var item in db)
                           {
                               if (item.TakaID == d.TakaID)
                               {
                                   throw new Exception($"Your entered TakaID : {d.TakaID}" +
                                       $"already exist in ChallanID : {value.TakaChallanNumber}");
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

               }*/
