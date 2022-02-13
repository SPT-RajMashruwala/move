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
    public class ProductionController : ControllerBase
    {
        [Route("Production/add")]
        [Route("Production/view")]
        [Route("Production/viewById/{ID}")]
        [Route("Production/update/{ID}")]
        [Route("Production/delete/{ID}")]
    }
}
