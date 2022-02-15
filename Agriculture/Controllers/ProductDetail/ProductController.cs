using Agriculture.Core.ProductDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Controllers.ProductDetail
{
    [Route("ProductDetail")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        [Route("Product/add")]
        public IActionResult Add([FromBody] Models.ProductDetail.Product value) 
        {
            return Ok(new Products().Add(value));
        }

        [HttpGet]
        [Route("Product/getUnit")]
        public IActionResult getUnit()
        {
            return Ok(new Products().GetUnit());
        }

        [HttpGet]
        [Route("Product/getProduct")]
        public IActionResult getProduct()
        {
            return Ok(new Products().GetProduct());
        }

        [HttpGet]
        [Route("Product/view")]
        public IActionResult View() 
        {
            return Ok(new Products().View());
        }

        [HttpGet]
        [Route("Product/viewById/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID) 
        {
            return Ok(new Products().ViewById(ID));
        }

        [HttpPut]
        [Route("Product/update/{ID}")]
        public IActionResult Update([FromBody] Models.ProductDetail.Product value,[FromRoute] int ID)
        {
            return Ok(new Products().Update(value,ID));
        }

       /* [Route("Product/delete/{ID}")]*/
    }
}
