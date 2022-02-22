using KarkhanaBook.Models.Common;
using KarkhanaBookContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static KarkhanaBook.Models.Common.Result;

namespace KarkhanaBook.Core.Callan
{
    public class ChallanSlips
    {
        public Result Add(Models.Challan.ChallanSlip value) 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext()) 
            {
                ChallanSlip dbchallanSlip = new ChallanSlip();
                dbchallanSlip.ChallanSlipSerialNumber = value.ChallanSlipSerialNumber;
                dbchallanSlip.SellerName = value.SellerName;
                dbchallanSlip.RangeCartoonSerialNumber = value.RangeCartoonSerialNumber;
                dbchallanSlip.TotalCartoons = value.TotalCartoons;
                dbchallanSlip.RsPerKG = Math.Round(value.RsPerKG, 2);
                dbchallanSlip.TotalWeight = value.TotalWeight;
                dbchallanSlip.DateOfPurchase = value.DateOfPurchase.ToLocalTime();
                dbchallanSlip.Remark = value.Remark;
                dbchallanSlip.DateTime = DateTime.Now;

                context.ChallanSlips.InsertOnSubmit(dbchallanSlip);
                context.SubmitChanges();

                var result = new Result()
                {
                    Message = "ChallanSlip Added Successfully",
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
                var dbobj = (from obj in context.ChallanSlips
                             select obj).ToList();
                return dbobj;
            }
        }
        public async Task<IEnumerable> ViewByID(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.ChallanSlips
                             where obj.ChallanSlipIndex==ID
                             select obj).ToList();
                return dbobj;
            }
        }
        public Result Update(Models.Challan.ChallanSlip value,int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.ChallanSlips
                             where obj.ChallanSlipIndex == ID
                             select obj).SingleOrDefault();
                dbobj.SellerName = value.SellerName;
                dbobj.RangeCartoonSerialNumber = value.RangeCartoonSerialNumber;
                dbobj.TotalCartoons = value.TotalCartoons;
                dbobj.RsPerKG = Math.Round(value.RsPerKG,2);
                dbobj.TotalWeight = value.TotalWeight;
                dbobj.DateOfPurchase = value.DateOfPurchase.ToLocalTime();
                dbobj.Remark = value.Remark;
                dbobj.DateTime = DateTime.Now;

                context.SubmitChanges();
                var result = new Result()
                {
                    Message = "ChallanSlip Updated Successfully",
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
                var dbobj = (from obj in context.ChallanSlips
                             where obj.ChallanSlipIndex == ID
                             select obj).SingleOrDefault();
                
                context.ChallanSlips.DeleteOnSubmit(dbobj);
                context.SubmitChanges();
                var result = new Result()
                {
                    Message = "ChallanSlip Deleted Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                 , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK

                };
                return result;


            }
        }
    }
}
