using KarKhanaBook.Core.Login;
using KarKhanaBook.Model.Login;
using KarKhanaBook.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KarKhanaBook.Controllers.Login
{
    [Route("Login")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Users _users;
        private readonly IConfiguration _configuration;
        private readonly Token _token;
        public UserController(Core.Login.Users user,IConfiguration configuration,Token token)
        {
            _users = user;
            _configuration = configuration;
            _token = token;
        }

        [HttpPost]
        [Route("User/SignUP")]
        public IActionResult SingUP([FromBody]User userModel)
        {
            return Ok(_users.SignUP(userModel));
        }


        [HttpPost]
        [Route("User/SignIN")]
        public IActionResult SignIN([FromBody]Model.Login.SingIN signINModel)
        {
            return Ok(_users.SignIN(signINModel));
        }


        [HttpPut]
        [Route("User/AccoutUpdate/{ID}")]
        public IActionResult UpdateAccount([FromBody]User userModel, [FromRoute] int ID)
        {

            return Ok(_users.UpdateAccount(userModel, ID));
        }


        [HttpDelete]
        [Route("User/AccoutDelete/{ID}")]
        public IActionResult DeleteAccount([FromRoute]int ID)
        {

            return Ok(_users.DeleteAccount(ID));
        }
    }
}
