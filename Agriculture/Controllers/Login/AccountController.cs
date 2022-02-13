using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Controllers.Login
{
    [Route("Login")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [Route("Account/signUP")]
        [Route("Accont/signIN")]
        [Route("Account/update/{ID}")]
        [Route("Account/delete/{ID}")]

    }
}
