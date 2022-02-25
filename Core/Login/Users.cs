using KarKhanaBook.Model.Common;
using KarKhanaBook.Services;
using KarkhanaBookContext;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using static KarKhanaBook.Model.Common.Result;

namespace KarKhanaBook.Core.Login
{
    public class Users
    {
        private readonly IConfiguration _configuration;
        private readonly Token _token;

        public Users(IConfiguration configuration, Services.Token token)
        {
            _configuration = configuration;
            _token = token;
            
        }
       /* public Users() { }*/
        public Result SignUP(Model.Login.User userModel)
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
        public Result SignIN(Model.Login.SingIN value)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var res = (from u1 in context.Users
                           where u1.Email == value.EmailAddress && u1.Password == value.Password
                           select new
                           {
                               UserName = u1.UserName,
                               UserID = u1.UserID,
                               RoleID = u1.RoleID,
                               RoleName = u1.Role.RoleName
                           }).FirstOrDefault();
             if (res != null)
              {
                    var authclaims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name,value.EmailAddress),
                    new Claim (ClaimTypes.Role,res.RoleName),
                    new Claim (ClaimTypes .Sid ,res.UserID .ToString()),
                    new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid ().ToString ()),
                   };
                    var jwtToken = _token.GenerateAccessToken(authclaims);
                    var refreshToken = _token.GenerateRefreshToken();
                    RefreshToken refreshToken1 = new RefreshToken();
                    refreshToken1.RefreshToken1 = refreshToken;
                    context.RefreshTokens.InsertOnSubmit(refreshToken1);
                    context.SubmitChanges();

                    UserRefreshToken userRefreshToken = new UserRefreshToken();
                    userRefreshToken.UserID = res.UserID;
                    userRefreshToken.RefreshTokenID = refreshToken1.RefreshTokenID;
                    context.UserRefreshTokens.InsertOnSubmit(userRefreshToken);
                    context.SubmitChanges();
                    var password = (from obj in context.Users
                                    where obj.Email == value.EmailAddress
                                    select obj.Password).SingleOrDefault();
                    if (password == value.Password)
                    {
                        var result = new Result()
                        {
                            Message = "User SignIN Successful",
                            Status = ((ResultStatus)(Enum.Parse(typeof(ResultStatus), ResultStatus.success.ToString()
                           , true))).ToString(),
                            StatusCode = (int)HttpStatusCode.OK,
                            Data=jwtToken,

                        };
                        return result;

                    }
                    else
                    {
                        throw new ArgumentException("SingIN failed");
                    }
                  
             }
                else
                {
                    throw new ArgumentException("Please Enter valid login details");
                }
            }

        }
        public Result UpdateAccount(Model.Login.User userModel, int ID)
        {
            using (KarkhanaBookDataContext context = new KarkhanaBookDataContext())
            {
                var dbobj = (from obj in context.Users
                             where obj.UserID == ID
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
                             where obj.UserID == ID
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
