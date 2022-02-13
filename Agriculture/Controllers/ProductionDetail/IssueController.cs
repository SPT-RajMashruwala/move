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
    public class IssueController : ControllerBase
    {
        [Route("IssueProducts/add")]
        [Route("IssueProducts/view")]
        [Route("IssueProducts/viewById/{ID}")]
        [Route("IssueProducts/update/{ID}")]
        [Route("IssueProducts/delete/{ID}")]
    }
}
