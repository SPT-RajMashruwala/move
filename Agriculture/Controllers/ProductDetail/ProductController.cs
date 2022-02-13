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
        [Route("Product/add")]
        [Route("Product/view")]
        [Route("Product/viewById/{ID}")]
        [Route("Product/update/{ID}")]
        [Route("Product/delete/{ID}")]
    }
}
