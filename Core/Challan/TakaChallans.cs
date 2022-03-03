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

namespace KarKhanaBook.Core.Challan
{
    public class TakaChallans
    {
        public Result Search(DataTable table) 
        {
            List<Model.Challan.TakaChallan> takaChallans = new List<Model.Challan.TakaChallan>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];


                takaChallans.Add(new Model.Challan.TakaChallan()
                {
                    TakaChallanIndex= Int16.Parse(dr["TakaChallanIndex"].ToString()),
                    TakaChallanNumber= Int16.Parse(dr["TakaChallanNumber"].ToString()),
                    TotalTakaQuantity= Int16.Parse(dr["TotalTakaQuantity"].ToString()),
                    TotalMeter= float.Parse(dr["TotalMeter"].ToString()),
                    TotalWeight= float.Parse(dr["TotalWeight"].ToString()),
                    RsPerMeter= float.Parse(dr["RsPerMeter"].ToString()),
                    TotalBillValue= float.Parse(dr["TotalBillValue"].ToString()),
                    Remark= dr["Remar"].ToString(),
                   

                });
            }
            return new Result()
            {
            };
        }
        public Result Add(Model.Challan.TakaChallan value)
        {
            float TotalMeter = 0;
            float TotalWeight = 0;

            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                TakaChallan dbtakaChallan = new TakaChallan();
                var dblist = (from obj in context.TakaIssues
                              where obj.TakaChallanNumber == value.TakaChallanNumber
                              select obj).ToList();
                if (dblist.Count() == 0) 
                {
                    throw new ArgumentException("Entered ChallanNumber doesnt exist First Isuue Takas on this" +
                        "ChllanNumber.");
                }
                foreach (var Taka in dblist)
                {
                    var TakaDetails = (from obj in context.TakaSheets
                                       where obj.TakaID == Taka.TakaID && obj.SlotNumber == Taka.SlotNumber
                                       select new
                                       {
                                           Meter = obj.Meter,
                                           Weight = obj.Weight,

                                       }).SingleOrDefault();

                    TotalMeter = TotalMeter + (float)TakaDetails.Meter;
                    TotalWeight = TotalWeight + (float)TakaDetails.Weight;

                }

                dbtakaChallan.TakaChallanNumber = value.TakaChallanNumber;
                dbtakaChallan.TotalTakaQuantity = dblist.Count();
                dbtakaChallan.TotalMeter = TotalMeter;
                dbtakaChallan.TotalWeight = TotalWeight;
                dbtakaChallan.RsPerMeter = Math.Round(value.RsPerMeter,2);
                dbtakaChallan.TotalBillValue = Math.Round((TotalMeter * value.RsPerMeter),2);
                dbtakaChallan.Remark = value.Remark;

                context.TakaChallans.InsertOnSubmit(dbtakaChallan);
                context.SubmitChanges();

                var result = new Result()
                {
                    Message = "TakaChallan Add Successfully",
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
                    Message = "TakaChallan View Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data=from obj in context.TakaChallans
                         select new Model.Challan.TakaChallan() 
                         {
                             TakaChallanIndex=obj.TakaChallanIndex,
                             TakaChallanNumber=(int)obj.TakaChallanNumber,
                             TotalTakaQuantity=(int)obj.TotalTakaQuantity,
                             TotalMeter=(float)obj.TotalMeter,
                             TotalWeight=(float)obj.TotalWeight,
                             RsPerMeter=(float)obj.RsPerMeter,
                             TotalBillValue=(float)obj.TotalBillValue,
                             Remark=obj.Remark,
                             
                         },

                };
               
            }
        }
        public Result ViewById(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                return new Result()
                {
                    Message = "TakaChallan View by its ID Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = from obj in context.TakaChallans
                           where obj.TakaChallanIndex==ID
                           select new Model.Challan.TakaChallan()
                           {
                               TakaChallanIndex = obj.TakaChallanIndex,
                               TakaChallanNumber = (int)obj.TakaChallanNumber,
                               TotalTakaQuantity = (int)obj.TotalTakaQuantity,
                               TotalMeter = (float)obj.TotalMeter,
                               TotalWeight = (float)obj.TotalWeight,
                               RsPerMeter = (float)obj.RsPerMeter,
                               TotalBillValue = (float)obj.TotalBillValue,
                               Remark = obj.Remark,

                           },

                };

            }
        }
        public Result Update(Model.Challan.TakaChallan value, int ID)
        {
            float TotalMeter = 0;
            float TotalWeight = 0;
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.TakaChallans
                             where obj.TakaChallanIndex == ID
                             select obj).SingleOrDefault();


                var challanMatch = (from obj in context.TakaChallans
                                    where obj.TakaChallanNumber == value.TakaChallanNumber
                                    select obj).ToList();
                if (challanMatch.Count() > 0) 
                {
                    throw new ArgumentException("Your updated ChallanNumber Already Issued.");
                }


                var dblist = (from obj in context.TakaIssues
                              where obj.TakaChallanNumber == value.TakaChallanNumber
                              select obj.TakaID).ToList();
                if (dblist.Count() == 0)
                {
                    throw new ArgumentException("Entered ChallanNumber doesnt exist First Isuue Takas on this" +
                        "ChllanNumber.");
                }
                foreach (var Taka in dblist)
                {
                    var TakaDetails = (from obj in context.TakaSheets
                                       where obj.TakaID == Taka
                                       select new
                                       {
                                           Meter = obj.Meter,
                                           Weight = obj.Weight,

                                       }).SingleOrDefault();

                    TotalMeter = TotalMeter + (float)TakaDetails.Meter;
                    TotalWeight = TotalWeight + (float)TakaDetails.Weight;

                }
                dbobj.TakaChallanNumber = value.TakaChallanNumber;
                dbobj.TotalTakaQuantity = dblist.Count();
                dbobj.TotalMeter = TotalMeter;
                dbobj.TotalWeight = TotalWeight;
                dbobj.RsPerMeter =Math.Round(value.RsPerMeter,2);
                dbobj.TotalBillValue = Math.Round((TotalMeter * value.RsPerMeter), 2);
                dbobj.Remark = value.Remark;


                context.SubmitChanges();

                var result = new Result()
                {
                    Message = "TakaChallan Updated Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data=dbobj,

                };
                return result;


            }


        }
        public Result Delete(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.TakaChallans
                             where obj.TakaChallanIndex == ID
                             select obj).SingleOrDefault();
                context.TakaChallans.DeleteOnSubmit(dbobj);
                context.SubmitChanges();

                var result = new Result()
                {
                    Message = "TakaChallan Deleted Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK

                };
                return result;

            }
        }

    }
}
