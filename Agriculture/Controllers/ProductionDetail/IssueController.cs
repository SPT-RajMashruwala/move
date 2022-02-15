using Agriculture.Core.ProductionDetails;
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
        [HttpPost]
        [Route("IssueProducts/add")]
        public IActionResult Add([FromBody] Models.ProductionDetail.Issue value) 
        {
            return Ok(new Issues().Add(value));
        }


        [HttpGet]
        [Route("IssueProducts/view")]
        public IActionResult View()
        {
            return Ok(new Issues().View());
        }

        [HttpGet]
        [Route("IssueProducts/viewById/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new Issues().ViewById(ID));
        }
        /*  [Route("IssueProducts/update/{ID}")]
          [Route("IssueProducts/delete/{ID}")]*/
    }
}
