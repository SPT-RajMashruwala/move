using KarkhanaBook.Models.Common;
using KarkhanaBookContext;
using System;

using System.Linq;
using System.Net;

using static KarkhanaBook.Models.Common.Result;

namespace KarkhanaBook.Core.Login
{
    public class Users
    {
        public Result SignUP(Models.Login.User userModel) 
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                KarkhanaBookContext.User user = new KarkhanaBookContext.User();
                user.UserName = userModel.UserName;
                user.Email = userModel.EmailAddress;
                user.Password = userModel.Password;
                user.ContactNumber = userModel.ContactNumber;
                user.AlternetContactNumber = userModel.AlternetContactNumber;
                user.RoleID = userModel.Role.ID;
                var db = (from obj in context.Users
                          where obj.Email == userModel.EmailAddress
                          select obj).ToList();
                if (db.Count() > 0)
                {
                    var result = new Result()
                    {
                        Message = "Entered Email Address Already Assigned To Some User",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.info.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK

                    };
                    return result;

                }
                else
                {
                    context.Users.InsertOnSubmit(user);
                    context.SubmitChanges();
                    var result = new Result()
                    {
                        Message = "User SignUP Successful",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.OK

                    };
                    return result;
                }
            }
        }
        public Result SignIN(Models.Login.SignIN signINModel)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var password = (from obj in context.Users
                                where obj.Email == signINModel.EmailAddress
                                select obj.Password).SingleOrDefault();
                if (password == signINModel.Password)
                {
                    var result = new Result()
                    {
                        Message = "User SignIN Successful",
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
                        Message = "User SignIN Fail",
                        Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.warning.ToString()
                        , true))).ToString(),
                        StatusCode = (int)HttpStatusCode.Unauthorized

                    };
                    return result;

                }
            }

        }
        public Result UpdateAccount(Models.Login.User userModel, int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.Users
                             where obj.UserID==ID
                             select obj).SingleOrDefault();
                dbobj.UserName = userModel.UserName;
                dbobj.Email = dbobj.Email;
                dbobj.Password = userModel.Password;
                dbobj.AlternetContactNumber = userModel.AlternetContactNumber;
                dbobj.RoleID = userModel.Role.ID;

                context.SubmitChanges();
                var result = new Result()
                {
                    Message = "User Account Update Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK

                };
                return result;

            } 
        }
        public Result DeleteAccount(int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.Users
                             where obj.UserID==ID
                             select obj).SingleOrDefault();
                context.Users.DeleteOnSubmit(dbobj);
                context.SubmitChanges();
                var result = new Result()
                {
                    Message = "Accout Deleted Successfully",
                    Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                    , true))).ToString(),
                    StatusCode = (int)HttpStatusCode.OK

                };
                return result;
            }
        }
    }
}
