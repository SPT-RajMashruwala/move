using KarKhanaBook.Model.Common;
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
    public class TakaSheets
    {
        Model.Taka.TakaSheet takasheets = new Model.Taka.TakaSheet()
        {
            takas = new List<Model.Taka.Taka>()
        };

        public Result FilterSorting(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var qs = (from obj in context.TakaSheets
                          orderby obj.TakaID ascending
                          select obj).ToList();
                return new Result()
                {
                    Message = "TakaSheet Added Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = qs,
                };
            }
        }
        public Result Search(DataTable table)
        {
            List<Model.Search.SearchTakaSheet> takaSheet = new List<Model.Search.SearchTakaSheet>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];


                takaSheet.Add(new Model.Search.SearchTakaSheet()
                {
                    TakaSheetIndex = Int16.Parse(dr["TakaSheetIndex"].ToString()),
                    SloatNumber = Int16.Parse(dr["SlotNumber"].ToString()),
                    Date = Convert.ToDateTime(dr["Date"].ToString()),
                    TakaID = Int16.Parse(dr["TakaID"].ToString()),
                    MachineNumber = Int16.Parse(dr["MachineNumber"].ToString()),
                    Meter = float.Parse(dr["Meter"].ToString()),
                    Weight = float.Parse(dr["Weight"].ToString()),

                });
            }

            return new Result()
            {
                Message = "TakaSheet Added Successfully",
                Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                StatusCode = (int)HttpStatusCode.OK,
                Data = takaSheet,
            };
        }
        public Result Add(Model.Taka.TakaSheet value)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                //constraints to be added

                //Machine Number from Dropdown
                //Meter value not exceed limit ex 130meter
                //weight value not exceed limit ex --kg
               
                TakaSheet dbtakaSheet = new TakaSheet();
                var db = value.takas.FirstOrDefault();
                var takaCount = (from obj1 in value.takas
                                 from obj in context.TakaSheets
                                 where obj.TakaID ==obj1.TakaID && obj.SlotNumber==value.SloatNumber
                                 select obj).ToList();
                if (takaCount.Count() > 0)
                {
                    throw new Exception($"Entered TakaId : {db.TakaID} Already Exist in Sloat : {value.SloatNumber}");
                }
                else
                {
                    var dbobjlist = (from obj in value.takas
                                     select new TakaSheet()
                                     {
                                         SlotNumber=value.SloatNumber,
                                         TakaID = obj.TakaID,
                                         MachineNumber = obj.MachineNumber,
                                         Meter = obj.Meter,
                                         Weight = obj.Weight,
                                         Date = obj.Date.ToLocalTime(),

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
        public Result View()
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var qs = (from obj in context.TakaSheets
                          select new
                          {
                              TakaSheetIndex = obj.TakaSheetIndex,
                              SloatNumber = obj.SlotNumber,
                              TakaID = obj.TakaID,
                              MachineNumber = obj.MachineNumber,
                              Meter = obj.Meter,
                              Weight = obj.Weight,
                              Date = obj.Date,
                          }).ToList();
                return new Result()
                {
                    Message = "TakaSheet View Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = qs,
                };

            
              
            }
        }
        public Result ViewByID(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {

                var dbobj = (from obj in context.TakaSheets
                             where obj.TakaSheetIndex == ID
                             select obj).ToList();
                if (dbobj.Count() > 0)
                {
                    var qs = (from obj in dbobj
                              select new
                              {
                                  TakaSheetIndex = obj.TakaSheetIndex,
                                  SloatNumber = obj.SlotNumber,
                                  TakaID = obj.TakaID,
                                  MachineNumber = obj.MachineNumber,
                                  Meter = obj.Meter,
                                  Weight = obj.Weight,
                                  Date = obj.Date,
                              }).ToList();

                    return new Result()
                    {
                        Message = "TakaSheet View Successfully",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                            , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK,
                        Data = qs,
                    };


                }
                else
                {
                    throw new Exception("Your Entered ID Doesnt Exist in Database");
                }
            }
        }
        public Result Update(Model.Taka.TakaSheet value, int Id)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var db = (from obj in value.takas
                          select obj).SingleOrDefault();

                var dbobj = (from obj in context.TakaSheets
                             where obj.TakaSheetIndex == Id
                             select obj).ToList();

                var takaCount = (from obj in context.TakaSheets
                                 where obj.TakaID == db.TakaID && obj.SlotNumber == value.SloatNumber
                                 select obj).ToList().Except(dbobj);

                if (dbobj.Count() > 0)
                {
                    if (takaCount.Count() > 0)
                    {
                        throw new Exception($"Your Entered TakaId : {db.TakaID} Already Exist in Sloat : {value.SloatNumber}");
                    }
                    else
                    {
                        foreach (var item in dbobj)
                        {
                            item.SlotNumber = value.SloatNumber;
                            item.TakaID = db.TakaID;
                            item.MachineNumber = db.MachineNumber;
                            item.Meter = db.Meter;
                            item.Weight = db.Weight;
                            item.Date = db.Date.ToLocalTime();

                            context.SubmitChanges();
                        }
                        var result = new Result()
                        {
                            Message = "TakaSheet Update Successfully",
                            Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                             , true))).ToString(),
                            StatusCode = (int)HttpStatusCode.OK,
                            Data=dbobj,

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
                             where obj.TakaSheetIndex == ID
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
                        StatusCode = (int)HttpStatusCode.OK,
                        Data =dbobj,

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
