
using KarkhanaBook.Core.Taka;
using KarkhanaBookContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarkhanaBook.Controllers.Taka
{
    [Route("Sheet")]
    [ApiController]
    public class TakaSheetController : ControllerBase
    {
        [HttpPost]
        [Route("TakaSheet/Add")]
        public IActionResult Add([FromBody] Models.Taka.TakaSheet value)
        {
            return Ok(new TakaSheets().Add(value));
        }

        [HttpGet]
        [Route("TakaSheet/View")]
        public IActionResult View()
        {
            return Ok(new TakaSheets().View().Result);
        }

        [HttpGet]
        [Route("TakaSheet/ViewById/{ID}")]
        public IActionResult ViewById([FromRoute] int ID)
        {
            return Ok(new TakaSheets().ViewByID(ID).Result);
        }

        [HttpPut]
        [Route("TakaSheet/Update/{ID}")]
        public IActionResult Update([FromBody] Models.Taka.TakaSheet value, [FromRoute] int ID)
        {
            return Ok(new TakaSheets().Update(value, ID));
        }

        [HttpDelete]
        [Route("TakaSheet/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new TakaSheets().Delete(ID));
        }
    }
}
