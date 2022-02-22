using KarKhanaBook.Core.Kariger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Controllers.Kariger
{
    [Route("Sheet")]
    [ApiController]
    public class KarigerDailyShhetController : ControllerBase
    {
        [HttpPost]
        [Route("KarigerDailySheet/Add")]
        public IActionResult Add([FromBody] Model.Kariger.KarigerDailySheet value)
        {
            return Ok(new KarigerDailySheets().Add(value));
        }

        [HttpGet]
        [Route("KarigerDailySheet/View")]
        public IActionResult View()
        {
            return Ok(new KarigerDailySheets().View().Result);
        }

        [HttpGet]
        [Route("KarigerDailySheet/ViewByID/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new KarigerDailySheets().ViewById(ID).Result);
        }

        [HttpPut]
        [Route("KarigerDailySheet/Update/{ID}")]
        public IActionResult Update([FromBody] Model.Kariger.KarigerDailySheet value, [FromRoute] int ID)
        {
            return Ok(new KarigerDailySheets().Update(value, ID));
        }

        [HttpDelete]
        [Route("KarigerDailySheet/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new KarigerDailySheets().Delete(ID));
        }
    }
}
