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
    public class ChallanPayments
    {
        public Result Add(Model.Challan.ChallanPayment value)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var TotalWeight = (from obj in context.ChallanSlips
                                   where obj.ChallanSlipSerialNumber == value.ChallanSlipSerialNumber
                                   select obj.TotalWeight).SingleOrDefault();

                var dbChallanSlipNumber = (from obj in context.ChallanSlips
                                           where obj.ChallanSlipSerialNumber == value.ChallanSlipSerialNumber
                                           select obj).ToList();
                if (dbChallanSlipNumber.Count() > 0)
                {
                    PaymentSlip dbpaymentSlip = new PaymentSlip();
                    dbpaymentSlip.ChallanSlipSerialNumber = value.ChallanSlipSerialNumber;
                    dbpaymentSlip.BillSerialNumber = value.BillSerialNumber;
                    dbpaymentSlip.TotalWeight = TotalWeight;
                    dbpaymentSlip.Payment = value.Payment;

                    context.PaymentSlips.InsertOnSubmit(dbpaymentSlip);
                    context.SubmitChanges();

                    var result = new Result()
                    {
                        Message = "PaymentSlip Added Successfully",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                      , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK

                    };
                    return result;
                }
                else
                {
                    var result = new Result()
                    {
                        Message = "Your Entered ChallanSlipNumber Not Match",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.info.ToString()
                         , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.NotFound

                    };
                    return result;
                }
            }
        }
        public async Task<IEnumerable> View()
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.PaymentSlips
                             select obj).ToList();
                return dbobj;
            }
        }
        public async Task<IEnumerable> ViewByID(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.PaymentSlips
                             where obj.PaymentSlipIndex == ID
                             select obj).ToList();
                if (dbobj.Count > 0)
                {
                    return dbobj;
                }
                else
                {
                    throw new ArgumentException($"ChallanPayment Database have no record for corrosponding ID : { ID} ");

                }
            }
        }
        public Result Update(Model.Challan.ChallanPayment value, int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var TotalWeight = (from obj in context.ChallanSlips
                                   where obj.ChallanSlipSerialNumber == value.ChallanSlipSerialNumber
                                   select obj.TotalWeight).SingleOrDefault();
                var dbobj = (from obj in context.PaymentSlips
                             where obj.PaymentSlipIndex == ID
                             select obj).SingleOrDefault();
                var dbobjlist = (from obj in context.PaymentSlips
                                 where obj.PaymentSlipIndex == ID
                                 select obj).ToList();
                if (dbobjlist.Count > 0)
                {
                    dbobj.ChallanSlipSerialNumber = value.ChallanSlipSerialNumber;
                    dbobj.BillSerialNumber = value.BillSerialNumber;
                    dbobj.TotalWeight = TotalWeight;
                    dbobj.Payment = value.Payment;

                    context.SubmitChanges();
                    var result = new Result()
                    {
                        Message = "PaymentSlip Updated Successfully",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK

                    };
                    return result;

                }
                else
                {
                    throw new ArgumentException($"ChallanPayment Database have no record for corrosponding ID : { ID} ");

                }

            }

        }
        public Result Delete(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.PaymentSlips
                             where obj.PaymentSlipIndex == ID
                             select obj).SingleOrDefault();
                context.PaymentSlips.DeleteOnSubmit(dbobj);
                context.SubmitChanges();

                var result = new Result()
                {
                    Message = "PaymentSlip Deleted Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                  , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK

                };
                return result;
            }
        }
    }
}
