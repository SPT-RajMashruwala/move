using KarkhanaBook.Core.Callan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarkhanaBook.Controllers.Challan
{
    [Route("challan")]
    [ApiController]
    public class ChallanPaymentController : ControllerBase
    {
        [HttpPost]
        [Route("ChallanPayment/Add")]
        public IActionResult Add([FromBody] Models.Challan.ChallanPayment value ) 
        {
            return Ok(new ChallanPayments().Add(value));
        
        }

        [HttpGet]
        [Route("ChallanPayment/View")]
        public IActionResult View()
        {
            return Ok(new ChallanPayments().View().Result);

        }


        [HttpGet]
        [Route("ChallanPayment/ViewById/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new ChallanPayments().ViewByID(ID).Result);

        }

        [HttpPut]
        [Route("ChallanPayment/Update/{ID}")]
        public IActionResult ViewByID([FromBody] Models.Challan.ChallanPayment value,[FromRoute] int ID)
        {
            return Ok(new ChallanPayments().Update(value,ID));

        }

        [HttpDelete]
        [Route("ChallanPayment/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new ChallanPayments().Delete(ID));

        }


    }
}
