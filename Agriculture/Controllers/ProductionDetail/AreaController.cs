using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Controllers.ProductionDetail
{
    [Route("ProductionDetail")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        [Route("Area/add")]
        [Route("Area/view")]
        [Route("Area/viewById/{ID}")]
        [Route("Area/update/{ID}")]
        [Route("Area/delete/{ID}")]
    }
}
