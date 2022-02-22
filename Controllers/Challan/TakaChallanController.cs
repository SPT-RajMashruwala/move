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
    public class TakaChallanController : ControllerBase
    {
        [HttpPost]
        [Route("TakaChallan/Add")]
        public IActionResult Add([FromBody] Model.Challan.TakaChallan value)
        {
            return Ok(new TakaChallans().Add(value));
        }


        [HttpGet]
        [Route("TakaChallan/View")]
        public IActionResult View()
        {
            return Ok(new TakaChallans().View().Result);
        }


        [HttpGet]
        [Route("TakaChallan/ViewById/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new TakaChallans().ViewById(ID).Result);
        }


        [HttpPut]
        [Route("TakaChallan/Update/{ID}")]
        public IActionResult Update([FromBody] Model.Challan.TakaChallan value, [FromRoute] int ID)
        {
            return Ok(new TakaChallans().Update(value, ID));
        }


        [HttpDelete]
        [Route("TakaChallan/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new TakaChallans().Delete(ID));
        }
    }
}
