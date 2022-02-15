using Agriculture.Core.ProductDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Controllers.ProductDetail
{
    [Route("ProductionDetail")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        [HttpPost]
        [Route("Purchase/add")]
        public IActionResult Add([FromBody] Models.ProductDetail.Purchase value)
        {
            return Ok(new Purchases().Add(value));
        }


        [HttpGet]
        [Route("Purchase/view")]
        public IActionResult View()
        {
            return Ok(new Purchases().View());
        }


        [HttpGet]
        [Route("Purchase/viewById/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID) 
        {
            return Ok(new Purchases().ViewById(ID));
        }


        [HttpPut]
        [Route("Purchase/update/{ID}")]
        public IActionResult Update([FromBody] Models.ProductDetail.Purchase value,[FromRoute] int ID) 
        {
            return Ok(new Purchases().Update(value,ID));
        }

      /*  [Route("Purchase/delete/{ID}")]*/


    }
}
