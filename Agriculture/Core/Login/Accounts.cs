using Agriculture.Middleware;
using Agriculture.Models.Common;
using Agriculture.Services;
using Microsoft.Extensions.Configuration;
using ProductInventoryContext;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Agriculture.Core.Login
{
    public class Accounts 
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;

        public Accounts(IConfiguration configuration, ITokenServices tokenServices)
        {
            _configuration = configuration;
            _tokenServices = tokenServices;
        }
        

        //To add new role
        public Result AddRole(Models.Login.Role value)
        {
            using (ProductInventoryDataContext context = new ProductInventoryDataContext())
            {
                Role role = new Role();
                role.RoleName = value.RoleName;
                var check = context.Roles.FirstOrDefault(x => x.RoleName == value.RoleName);

                if (check != null)
                {
                    return new Result()
                    {
                        Message = string.Format($"Role already exist"),
                        Status = Result.ResultStatus.none,
                    };
                }
                else
                {
                    context.Roles.InsertOnSubmit(role);
                    context.SubmitChanges();
                    return new Result()
                    {
                        Message = string.Format($"New Role Added Successfully"),
                        Status = Result.ResultStatus.success,
                        Data = value.RoleName,
                    };
                }
            }
        }

        //To register user details
        public Result SignUP(Models.Login.User value)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            User user = new User();
            Role role = new Role();
            MAC mac = new MAC();
            var UserMacAddress = mac.GetMacAddress().Result;
            var query = (from user1 in context.Users
                         join r1 in context.Roles
                         on user1.RoleID equals r1.RoleID
                         where user1.EmailAddress ==value.EmailAddress 
                         select new
                         {
                             r1.RoleName
                         }).Count();
            if (query != 0)
            {
                throw new ArgumentException("User Already Exists");
            }
            else
            {
                user.UserName = value.UserName;
                user.Password = value.Password;
                user.RoleID = 1;//default user   1.Admin 2.User
                user.EmailAddress = value.EmailAddress;
                user.SystemMAC = UserMacAddress;
                user.DateTime = DateTime.Now;
                context.Users.InsertOnSubmit(user);
                context.SubmitChanges();
                return new Result()
                {
                    Message = string.Format($"{value.UserName}  Register as User Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = value.UserName,
                };
            }
        }

        //public Result LoginUser(LoginModel loginModel )
        //{
        //    ProductInventoryDataContext context = new ProductInventoryDataContext();
        //    User user = new User();
        //    Role role = new Role();
        //    var res = (from u1 in context.Users
        //               where u1.EmailAddress == loginModel.EmailAddress && u1.Password == loginModel.Password
        //               select new
        //               {
        //                   UserName = u1.UserName ,
        //                   UserID = u1.UserID,
        //                   RoleID = u1.RoleID,
        //                   RoleName = u1.Role.RoleName
        //               }).FirstOrDefault();
        //    if(res != null)
        //    {
        //        var authclaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name,loginModel.EmailAddress),
        //            new Claim (ClaimTypes.Role,res.RoleName),
        //            new Claim (ClaimTypes .Sid ,res.UserID .ToString()),
        //            new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid ().ToString ()),
        //        };
        //        var jwtToken = _tokenService.GenerateAccessToken(authclaims);
        //        var refreshToken = _tokenService.GenerateRefreshToken();
        //        RefreshToken refreshToken1 = new RefreshToken();
        //        refreshToken1.RToken  = refreshToken;
        //        context.RefreshTokens.InsertOnSubmit(refreshToken1);
        //        context.SubmitChanges();

        //        UserRefreshToken userRefreshToken = new UserRefreshToken();
        //        userRefreshToken.UserID = res.UserID;
        //        userRefreshToken.RefreshID = refreshToken1.RefreshID;
        //        context.UserRefreshTokens.InsertOnSubmit(userRefreshToken);
        //        context.SubmitChanges();

        //        return new Result()
        //        {
        //            Message = string.Format($"Login Successfully"),
        //            Status = Result.ResultStatus.success,
        //            Data = new {
        //            token = jwtToken,
        //            refreshToken = refreshToken,
        //            UserName = res.UserName,
        //            EmailAddress = loginModel .EmailAddress ,
        //            RoleName = res.RoleName,
        //            },
        //        };
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Please Enter valid login details");
        //    }
        //}

        public Result SignIN(Models.Login.SignIN value)
        {
            ProductInventoryDataContext context = new ProductInventoryDataContext();
            User user = new User();
            Role role = new Role();
            MAC mac = new MAC();
            var UserMacAddress = mac.GetMacAddress().Result;
            var res = (from u1 in context.Users
                       where u1.EmailAddress == value.EmailAddress && u1.Password == value.Password
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
                var jwtToken = _tokenServices.GenerateAccessToken(authclaims);
                var refreshToken = _tokenServices.GenerateRefreshToken();
                RefreshToken refreshToken1 = new RefreshToken();
                refreshToken1.RToken = refreshToken;
                context.RefreshTokens.InsertOnSubmit(refreshToken1);
                context.SubmitChanges();

                UserRefreshToken userRefreshToken = new UserRefreshToken();
                userRefreshToken.UserID = res.UserID;
                userRefreshToken.RefreshID = refreshToken1.RefreshID;
                context.UserRefreshTokens.InsertOnSubmit(userRefreshToken);
                context.SubmitChanges();

                var qs = (from obj in context.Users
                          where obj.EmailAddress == value.EmailAddress
                          select obj.UserName).FirstOrDefault();

                LoginDetail l = new LoginDetail();

                var macaddress = (from obj in context.LoginDetails
                           where obj.SystemMac == UserMacAddress
                           select obj).ToList();
                if (macaddress.Count() > 0)
                {
                    var Lid = context.LoginDetails.FirstOrDefault(c => c.SystemMac == UserMacAddress);
                    Lid.DateTime = DateTime.Now;
                    context.SubmitChanges();
                }
                else
                {
                   l.UserName = qs;
                   l.SystemMac = UserMacAddress;
                   l.DateTime = DateTime.Now;
                   context.LoginDetails.InsertOnSubmit(l);
                   context.SubmitChanges();
                }

                return new Result()
                {
                    Message = string.Format($"Login Successfully"),
                    Status = Result.ResultStatus.success,
                    Data = new
                    {
                        token = jwtToken,
                        refreshToken = refreshToken,
                        UserName = res.UserName,
                        EmailAddress = value.EmailAddress,
                        RoleName = res.RoleName,
                    },
                };
            }
            else
            {
                throw new ArgumentException("Please Enter valid login details");
            }
        }
    }
}
