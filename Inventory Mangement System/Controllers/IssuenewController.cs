using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
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
    public class IssuenewController : ControllerBase
    {
        private readonly IIssuenewRepository issuenewRepository;

        public IssuenewController(IIssuenewRepository issuenewRepository)
        {
            this.issuenewRepository = issuenewRepository;
        }

        [HttpPost("addissue")]
        public async Task<IActionResult> Add(IssueModel issueModel) 
        {
            var result = issuenewRepository.Add(issueModel);
            return Ok(result.Result);
        
        }
        [HttpGet("getissuebyId/{issueID}")]
        public async Task<IActionResult> ViewById(int issueID)
        {
            var result = issuenewRepository.ViewById(issueID);
            return Ok(result.Result);

        }
        [HttpGet("getissue")]
        public async Task<IActionResult> View()
        {
            var result = issuenewRepository.View();
            return Ok(result.Result);

        }
        [HttpPut("updateissue/{issueID}")]
        public async Task<IActionResult> Update(IssueModel issueModel,int issueID)
        {
            var result = issuenewRepository.Update(issueModel, issueID);
            return Ok(result.Result);

        }

    }
}
