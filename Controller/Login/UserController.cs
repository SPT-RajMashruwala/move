using KarkhanaBook.Core.Login;
using KarkhanaBook.Models.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarkhanaBook.Controllers.Login
{
    [Route("Login")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpPost]
        [Route("User/SignUP")]
        public  IActionResult SingUP([FromBody] User userModel)
        {
            return Ok(new Users().SignUP(userModel));
        }


        [HttpPost]
        [Route("User/SignIN")]
        public IActionResult SignIN([FromBody] Models.Login.SignIN signINModel)
        {
            return Ok(new Users().SignIN(signINModel));
        }


        [HttpPut]
        [Route("User/AccoutUpdate/{ID}")]
        public IActionResult UpdateAccount([FromBody] User userModel,[FromRoute] int ID)
        {
            
            return Ok(new Users().UpdateAccount(userModel,ID));
        }


        [HttpDelete]
        [Route("User/AccoutDelete/{ID}")]
        public IActionResult DeleteAccount([FromRoute] int ID)
        {

            return Ok(new Users().DeleteAccount(ID));
        }

    }
}
