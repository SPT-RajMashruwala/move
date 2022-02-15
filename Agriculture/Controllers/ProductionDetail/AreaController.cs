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
    public class AreaController : ControllerBase
    {
        [HttpPost]
        [Route("Area/add")]
        public IActionResult Add([FromBody] Models.ProductionDetail.Area value) 
        {
            return Ok(new Areas().Add(value));
        }

        [HttpGet]
        [Route("Area/getMainArea")]
        public IActionResult GetMainArea() 
        {
            return Ok(new Areas().GetMainArea());
        }

        [HttpGet]
        [Route("Area/getSubArea/{ID}")]
        public IActionResult GetSubArea([FromRoute] int ID)
        {
            return Ok(new Areas().GetSubArea(ID));
        }

        [HttpGet]
        [Route("Area/viewSubArea")]
        public IActionResult ViewSubArea() 
        {
            return Ok(new Areas().ViewSubArea());
        }

        [HttpGet]
        [Route("Area/viewSubAreaByID/{ID}")]
        public IActionResult ViewSubAreaByID([FromRoute] int ID)
        {
            return Ok(new Areas().ViewSubAreaByID(ID));
        }



        /*  [Route("Area/view")]
          [Route("Area/viewById/{ID}")]
          [Route("Area/update/{ID}")]
          [Route("Area/delete/{ID}")]*/
    }
}
