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
    public class CategoryController : ControllerBase
    {
        [HttpPost]
        [Route("Category/add")]
        public IActionResult Add([FromBody] Models.ProductDetail.Category value) 
        {
            return Ok(new Categorys().Add(value));
        }

        [HttpGet]
        [Route("Category/view")]
        public IActionResult View() 
        {
            return Ok(new Categorys().View());
        }

        [HttpGet]
        [Route("Category/viewById/{ID}")]
        public IActionResult View([FromRoute] int ID)
        {
            return Ok(new Categorys().ViewById(ID));
        }

        [HttpPut]
        [Route("Category/update/{ID}")]
        public IActionResult update([FromBody] Models.ProductDetail.Category value,[FromRoute] int ID) 
        {
            return Ok(new Categorys().update(value,ID));
        }



        /*[Route("Category/delete/{ID}")]*/
    }
}
