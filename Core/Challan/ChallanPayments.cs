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
    public class ChallanPayments
    {
        public Result Search(DataTable table ) 
        {
            List<Model.Challan.ChallanPayment> challanPayment = new List<Model.Challan.ChallanPayment>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];


                challanPayment.Add(new Model.Challan.ChallanPayment()
                {
                    BillSerialNumber=dr["BillSerialNumber"].ToString(),
                    ChallanSlipSerialNumber= dr["ChallanSlipSerialNumber"].ToString(),
                    Payment= float.Parse(dr["Payment"].ToString()),
                    PaymentSlipIndex= Int16.Parse(dr["PaymentSlipIndex"].ToString()),
                    TotalWeight= float.Parse(dr["TotalWeight"].ToString()),
                    Remark= dr["Remark"].ToString()

                });

            }
            return new Result()
            {
                Message = "PaymentSlip Added Successfully",
                Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                      , true))).ToString(),
                StatusCode = (int)HttpStatusCode.OK,
                Data=challanPayment,
            };
        }
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
                    dbpaymentSlip.DateTime = DateTime.Now;
                    dbpaymentSlip.Remark = value.Remark;

                    context.PaymentSlips.InsertOnSubmit(dbpaymentSlip);
                    context.SubmitChanges();

                    var result = new Result()
                    {
                        Message = "PaymentSlip Added Successfully",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                      , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK,
                   

                    };
                    return result;
                }
                else
                {
                    var result = new Result()
                    {
                        Message = "Your Entered ChallanSlipSerialNumber Not Match",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.info.ToString()
                         , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.NotFound

                    };
                    return result;
                }
            }
        }
        public Result View()
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                return new Result()
                {
                    Message = "Your Entered ChallanSlipSerialNumber Not Match",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.info.ToString()
                         , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Data=(from obj in context.PaymentSlips
                             select new Model.Challan.ChallanPayment()
                             {
                                 PaymentSlipIndex=obj.PaymentSlipIndex,
                                 ChallanSlipSerialNumber=obj.ChallanSlipSerialNumber,
                                 BillSerialNumber=obj.BillSerialNumber,
                                 TotalWeight=(float)obj.TotalWeight,
                                 Payment=(float)obj.Payment,


                             }).ToList(),
                };
            }
        }
        public Result ViewByID(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.PaymentSlips
                             where obj.PaymentSlipIndex == ID
                             select obj).ToList();
                if (dbobj.Count > 0)
                {
                    return new Result()
                    {
                        Message = "Your Entered ChallanSlipSerialNumber Not Match",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.info.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Data = (from obj in context.PaymentSlips
                                where obj.PaymentSlipIndex == ID
                                select new Model.Challan.ChallanPayment()
                                {
                                    PaymentSlipIndex = obj.PaymentSlipIndex,
                                    ChallanSlipSerialNumber = obj.ChallanSlipSerialNumber,
                                    BillSerialNumber = obj.BillSerialNumber,
                                    TotalWeight = (float)obj.TotalWeight,
                                    Payment = (float)obj.Payment,


                                }).ToList(),
                    };
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
                        StatusCode = (int)HttpStatusCode.OK,
                        Data = new Model.Challan.ChallanPayment()
                        {
                            PaymentSlipIndex = ID,
                            ChallanSlipSerialNumber = value.ChallanSlipSerialNumber,
                            BillSerialNumber = value.BillSerialNumber,
                            TotalWeight = (float)value.TotalWeight,
                            Payment = (float)value.Payment,


                        },


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
