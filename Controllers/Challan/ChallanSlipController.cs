using KarKhanaBook.Core.Challan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Controllers.Challan
{
    [Route("Challan")]
    [ApiController]
    public class ChallanSlipController : ControllerBase
    {

        [HttpPost]
        [Route("ChallanSlip/Add")]
        public IActionResult Add([FromBody] Model.Challan.ChallanSlip value)
        {
            return Ok(new ChallanSlips().Add(value));
        }

        [HttpGet]
        [Route("ChallanSlip/View")]
        public IActionResult View()
        {
            return Ok(new ChallanSlips().View());
        }

        [HttpGet]
        [Route("ChallanSlip/ViewByID/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new ChallanSlips().ViewByID(ID));
        }

        [HttpPut]
        [Route("ChallanSlip/Update/{ID}")]
        public IActionResult Update([FromBody] Model.Challan.ChallanSlip value, [FromRoute] int ID)
        {
            return Ok(new ChallanSlips().Update(value, ID));
        }

        [HttpDelete]
        [Route("ChallanSlip/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new ChallanSlips().Delete(ID));
        }
    }
}
