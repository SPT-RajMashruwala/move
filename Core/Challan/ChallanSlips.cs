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
    public class ChallanSlips
    {
        
        public Result Search(DataTable table) 
        {
            List<Model.Challan.ChallanSlip> challanSlips = new List<Model.Challan.ChallanSlip>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];
                challanSlips.Add(new Model.Challan.ChallanSlip()
                {
                    ChallanSlipIndex = Int16.Parse(dr["ChallanSlipIndex"].ToString()),
                    ChallanSlipSerialNumber = dr["ChallanSlipSerialNumber"].ToString(),
                    RangeCartoonSerialNumber = dr["RangeCartoonSerialNumber"].ToString(),
                    DateOfPurchase = Convert.ToDateTime(dr["DateOfPurchase"].ToString()),
                    Remark = dr["Remark"].ToString(),
                    RsPerKG = float.Parse(dr["RsPerKG"].ToString()),
                    SellerName = dr["SellerName"].ToString(),
                    TotalCartoons = Int16.Parse(dr["TotalCartoons"].ToString()),
                    TotalWeight = float.Parse(dr["TotalWeight"].ToString()),
                });


            }
           
            return new Result()
            {
                Message = "ChallanSlip Added Successfully",
                Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                StatusCode = (int)HttpStatusCode.OK,
                Data=challanSlips,
            };
        }
        public Result Add(Model.Challan.ChallanSlip value)
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
        public Result View()
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                return new Result()
                {
                    Message = "ChallanSlip View Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data=(from obj in context.ChallanSlips
                         select new Model.Challan.ChallanSlip() 
                         {
                             ChallanSlipIndex=obj.ChallanSlipIndex,
                             ChallanSlipSerialNumber=obj.ChallanSlipSerialNumber,
                             SellerName=obj.SellerName,
                             RangeCartoonSerialNumber=obj.RangeCartoonSerialNumber,
                             TotalCartoons=   (int)obj.TotalCartoons,
                             RsPerKG=(float)obj.RsPerKG,
                             TotalWeight=(float)obj.TotalWeight,
                             DateOfPurchase=Convert.ToDateTime(obj.DateOfPurchase),
                             Remark=obj.Remark,
                             
                             
                         }).ToList(),
                    
                };
            }
        }
        public Result ViewByID(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                return new Result()
                {
                    Message = "ChallanSlip View Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = (from obj in context.ChallanSlips
                            where obj.ChallanSlipIndex==ID
                            select new Model.Challan.ChallanSlip()
                            {
                                ChallanSlipIndex = obj.ChallanSlipIndex,
                                ChallanSlipSerialNumber = obj.ChallanSlipSerialNumber,
                                SellerName = obj.SellerName,
                                RangeCartoonSerialNumber = obj.RangeCartoonSerialNumber,
                                TotalCartoons = (int)obj.TotalCartoons,
                                RsPerKG = (float)obj.RsPerKG,
                                TotalWeight = (float)obj.TotalWeight,
                                DateOfPurchase = Convert.ToDateTime(obj.DateOfPurchase),
                                Remark = obj.Remark,


                            }).ToList(),

                };
            }
        }
        public Result Update(Model.Challan.ChallanSlip value, int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.ChallanSlips
                             where obj.ChallanSlipIndex == ID
                             select obj).SingleOrDefault();
                dbobj.ChallanSlipSerialNumber = value.ChallanSlipSerialNumber;
                dbobj.SellerName = value.SellerName;
                dbobj.RangeCartoonSerialNumber = value.RangeCartoonSerialNumber;
                dbobj.TotalCartoons = value.TotalCartoons;
                dbobj.RsPerKG = Math.Round(value.RsPerKG, 2);
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
                    StatusCode = (int)HttpStatusCode.OK,
                    Data = dbobj,

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
