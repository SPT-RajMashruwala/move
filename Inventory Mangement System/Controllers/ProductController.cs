using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using Inventory_Mangement_System.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController (IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost ("addproduct")]
        public async Task<IActionResult> ProductAdded(ProductModel productModel)
        {
            var result = _productRepository.AddProduct(productModel);
            return Ok(result);
           
        }

        [HttpPatch("UpdateProduct/{productID}")]
        public async Task<IActionResult> Update([FromBody] JsonPatchDocument productModel,int productID)
        {
            var result = _productRepository.UpdateProduct(productModel, productID);
            return Ok(result);
        }

        [HttpGet("getunit")]
        public async Task<IActionResult> ProductGet()
        {
            var result = _productRepository.GetUnit();
            return Ok(result.Result);
        }
    }
}
