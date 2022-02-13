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
        [Route("Category/add")]
        [Route("Category/view")]
        [Route("Category/viewById/{ID}")]
        [Route("Category/update/{ID}")]
        [Route("Category/delete/{ID}")]
    }
}
