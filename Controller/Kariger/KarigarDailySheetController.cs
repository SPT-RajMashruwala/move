
using KarkhanaBook.Core.Kariger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarkhanaBook.Controllers.Kariger
{
    [Route("Sheet")]
    [ApiController]
    public class KarigarDailySheetController : ControllerBase
    {
        [HttpPost]
        [Route("KarigerDailySheet/Add")]
        public IActionResult Add([FromBody] Models.Kariger.KarigarDailySheet value) 
        {
            return Ok(new KarigarDailySheets().Add(value));
        }

        [HttpGet]
        [Route("KarigerDailySheet/View")]
        public IActionResult View()
        {
            return Ok(new KarigarDailySheets().View().Result);
        }

        [HttpGet]
        [Route("KarigerDailySheet/ViewByID/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new KarigarDailySheets().ViewById(ID).Result);
        }

        [HttpPut]
        [Route("KarigerDailySheet/Update/{ID}")]
        public IActionResult Update([FromBody] Models.Kariger.KarigarDailySheet value,[FromRoute] int ID)
        {
            return Ok(new KarigarDailySheets().Update(value,ID));
        }

        [HttpDelete]
        [Route("KarigerDailySheet/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new KarigarDailySheets().Delete(ID));
        }
    }
}
