using Agriculture.Core.Login;
using Agriculture.Models.Common;
using Agriculture.Services;
using Devart.Data.Linq.Mapping.Fluent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

using Microsoft.Extensions.Configuration;


namespace Agriculture.Controllers.Login
{
    [Route("Login")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Accounts _accounts;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
    

        public AccountController(Accounts accounts , Microsoft.Extensions.Configuration.IConfiguration configuration, ITokenServices tokenServices)
        {
            _accounts = accounts;
            _configuration = configuration;
            _tokenServices = tokenServices;
         
        }
        [HttpPost]
        [Route("Role/add")]
        public IActionResult Add([FromBody] Models.Login.Role value)
        {

            return Ok(_accounts.AddRole(value));
        }


        [HttpPost]
        [Route("Account/signUP")]
        public IActionResult SignUP([FromBody] Models.Login.User value) 
        {
            return Ok(_accounts.SignUP(value));
        }


        [HttpPost]
        [Route("Accont/signIN")]
        public IActionResult SignIN(Models.Login.SignIN value) 
        {
            return Ok(_accounts.SignIN(value));
        }


        [HttpGet("gettoken")]
        public ActionResult<string> GetToken()
        {
            string rname = (string)HttpContext.Items["Rolename"];//context.Items["Rolename"] = RName;
          /*  int uid = (int)HttpContext.Items["UserId"];*/
            return Ok(rname);
            //var isClaim = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals(Claimstype.StringComparison.InvariantCultureIgnoreCase));
            //if (isClaim != null)
            //{
            //    var id = isClaim.Value;
            //   // var name1 = User.Identity.Name;

            //    return Ok(id);
            //}
            //else
            //{
            //    return BadRequest("no");
            //}
        }


     /*   [HttpPut]
        [Route("Account/update/{ID}")]
        [HttpDelete]
        [Route("Account/delete/{ID}")]*/

    }
}
