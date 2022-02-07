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
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseController (IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        [HttpPost("purchaseproduct")]
        public async Task<IActionResult> PurchaseDetailsAdded(PurchaseModel purchaseModel)
        {
            var result = _purchaseRepository.AddPurchaseDetails(purchaseModel);

            return Ok(result);
        }
        [HttpGet("getpurchaseproduct")]
        public async Task<IActionResult> PurchaseDetailsGeted()
        {
            var result = _purchaseRepository.GetPurchaseDetails();

            return Ok(result.Result);
        }
        [HttpGet("getpurchaseproductId/{Id}")]
        public async Task<IActionResult> PurchaseDetailsGetedById(int Id)
        {
            var result = _purchaseRepository.GetPurchaseDetailsById(Id);

            return Ok(result.Result);
        }
        [HttpPut("updatepurchaseproduct/{purchaseID}")]
        public async Task<IActionResult> PurchaseDetailsUpdated(PurchaseModel purchaseModel, int purchaseID)
        {
            var result = _purchaseRepository.UpdatePurchaseDetails(purchaseModel, purchaseID);

            return Ok(result);
        }

    }
}
