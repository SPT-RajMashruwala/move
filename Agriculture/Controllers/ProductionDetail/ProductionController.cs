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
    public class ProductionController : ControllerBase
    {
        [HttpPost]
        [Route("Production/add")]
        public IActionResult Add([FromBody] Models.ProductionDetail.Production value) 
        {
            return Ok(new Productions().Add(value));
        }


        [HttpGet]
        [Route("Production/view")]
        public IActionResult View() 
        {
            return Ok(new Productions().View());
        }


        [HttpGet]
        [Route("Production/viewById/{ID}")]
        public IActionResult ViewByID([FromRoute ] int ID) 
        {
            return Ok(new Productions().ViewById(ID));
        }


        [HttpPut]
        [Route("Production/update/{ID}")]
        public IActionResult Update(Models.ProductionDetail.Production value, int ID)
        {
            return Ok(new Productions().Update(value,ID));
        }
/*        [Route("Production/delete/{ID}")]*/
    }
}
