using KarKhanaBook.Core.Taka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Controllers.Taka
{
    [Route("Taka")]
    [ApiController]
    public class TakaIssueController : ControllerBase
    {
        [HttpPost]
        [Route("TakaIssue/add")]
        public IActionResult Add(Model.Taka.TakaIssue value) 
        {
            return Ok(new TakaIssues().Add(value));
        }
        [HttpGet]
        [Route("TakaIssue/view")]
        public IActionResult view()
        {
            return Ok(new TakaIssues().View());
        }
        [HttpGet]
        [Route("TakaIssue/one/{ID}")]
        public IActionResult One(int ID)
        {
            return Ok(new TakaIssues().ViewByID(ID));
        }
        [HttpPut]
        [Route("TakaIssue/update/{ID}")]
        public IActionResult Update(Model.Taka.TakaIssue value,int ID)
        {
            return Ok(new TakaIssues().Update(value,ID));
        }
      /*  [HttpDelete]
        [Route("TakaIssue/delete/{ID}")]
        public IActionResult Delete(int ID) 
        {
            return Ok(new TakaIssues().De);
        }*/
    }
}
