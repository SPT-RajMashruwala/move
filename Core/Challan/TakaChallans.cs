using KarKhanaBook.Model.Common;
using KarkhanaBookContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static KarKhanaBook.Model.Common.Result;

namespace KarKhanaBook.Core.Challan
{
    public class TakaChallans
    {
        public Result Add(Model.Challan.TakaChallan value)
        {
            float TotalMeter = 0;
            float TotalWeight = 0;

            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                TakaChallan dbtakaChallan = new TakaChallan();
                var dblist = (from obj in context.TakaIssues
                              where obj.TakaChallanNumber == value.TakaChallanNumber
                              select obj.TakaID).ToList();
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

                dbtakaChallan.TakaChallanNumber = value.TakaChallanNumber;
                dbtakaChallan.TotalTakaQuantity = dblist.Count();
                dbtakaChallan.TotalMeter = TotalMeter;
                dbtakaChallan.TotalWeight = TotalWeight;
                dbtakaChallan.RsPerMeter = value.RsPerMeter;
                dbtakaChallan.TotalBillValue = (TotalMeter * value.RsPerMeter);

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
        public async Task<IEnumerable> View()
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.TakaChallans
                             select obj).ToList();
                return dbobj;
            }
        }
        public async Task<IEnumerable> ViewById(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.TakaChallans
                             where obj.TakaChallanIndex == ID
                             select obj).ToList();
                return dbobj;
            }
        }
        public Result Update(Model.Challan.TakaChallan value, int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.TakaChallans
                             where obj.TakaChallanIndex == ID
                             select obj).SingleOrDefault();




                dbobj.RsPerMeter = value.RsPerMeter;
                dbobj.TotalBillValue = (dbobj.TotalMeter * value.RsPerMeter);


                context.SubmitChanges();

                var result = new Result()
                {
                    Message = "TakaChallan Updated Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK

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
