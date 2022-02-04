using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionController : ControllerBase
    {
        private readonly IProductionRepository _productionRepository;

        public ProductionController(IProductionRepository productionRepository)
        {
            _productionRepository = productionRepository;
        }
        [HttpPost("addproduction")]
        public async Task<IActionResult> ProductionDetailAdded([FromBody] ProductionModel productionModel)
        {
            var result = _productionRepository.AddProductionDetails(productionModel);
            return Ok(result);

        }

        [HttpGet("getproduction")]
        public async Task<IActionResult> GetProductionDetail()
        {
            var result = _productionRepository.GetProductionDetails();
            return Ok(result.Result);

        }
        [HttpGet("getproductionbyid/{id}")]
        public async Task<IActionResult> GetProductionDetailByID(int id)
        {
            var result = _productionRepository.GetProductionDetailsById(id);
            return Ok(result.Result);

        }
      /*  [HttpPut("updateProduction/{productionID}")]
        public async Task<IActionResult> ProductionDetailUpdated([FromBody] ProductionModel productionModel,int  productionID)
        {
            var result = _productionRepository.UpdateProductionDetails(productionModel, productionID);
            return Ok(result);

        }*/

    }
}
