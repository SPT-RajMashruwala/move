using KarkhanaBook.Core.Callan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarkhanaBook.Controllers.Challan
{
    [Route("Challan")]
    [ApiController]
    public class ChalanSlipController : ControllerBase
    {
        [HttpPost]
        [Route("ChallanSlip/Add")]
        public IActionResult Add([FromBody] Models.Challan.ChallanSlip value)
        {
            return Ok(new ChallanSlips().Add(value) );
        }

        [HttpGet]
        [Route("ChallanSlip/View")]
        public IActionResult View()
        {
            return Ok(new ChallanSlips().View().Result);
        }

        [HttpGet]
        [Route("ChallanSlip/ViewByID/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new ChallanSlips().ViewByID(ID).Result);
        }

        [HttpPut]
        [Route("ChallanSlip/Update/{ID}")]
        public IActionResult Update([FromBody] Models.Challan.ChallanSlip value,[FromRoute] int ID)
        {
            return Ok(new ChallanSlips().Update(value,ID));
        }

        [HttpDelete]
        [Route("ChallanSlip/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new ChallanSlips().Delete( ID));
        }
    }
}
