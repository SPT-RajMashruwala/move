using KarKhanaBook.Core.Taka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Controllers.Taka
{
    [Route("Taka")]
    [ApiController]
    public class TakaIssueController : ControllerBase
    {
        string query ;
        private readonly IConfiguration _configuration;

        public TakaIssueController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetTakaID")]
        public IActionResult GetTakaID()
        {
            return Ok(new TakaIssues().GetTakaID());
        }

        [HttpPost]
        [Route("TakaIssue/Search")]
        public IActionResult Search([FromBody]Model.Search.search value) 
        {
            

            if (value.field.ToLower() == "all")
            {
                this.query = @$"
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                             select * from dbo.TakaIssues
                             where dbo.TakaIssues.TakaIssueIndex LIKE '%'+@name+'%'
                             OR dbo.TakaIssues.TakaChallanNumber LIKE '%'+@name+'%'
                             OR dbo.TakaIssues.TakaID LIKE '%'+@name+'%'
                             OR dbo.TakaIssues.SlotNumber LIKE '%'+@name+'%'
                           ";
            }
            else if (value.field.ToLower() == "takaissueindex") 
            {
                this.query = @$"
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                             select * from dbo.TakaIssues
                             where dbo.TakaIssues.TakaIssueIndex LIKE '%'+@name+'%'
                             
                           ";
            }
            else if (value.field.ToLower() == "takachallannumber")
            {
                this.query = @$"
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                             select * from dbo.TakaIssues
                             where dbo.TakaIssues.TakaChallanNumber LIKE '%'+@name+'%'
                             
                           ";
            }
            else if (value.field.ToLower() == "takaid")
            {
                this.query = @$"
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                             select * from dbo.TakaIssues
                             where  dbo.TakaIssues.TakaID LIKE '%'+@name+'%'
                           ";
            }
            else if (value.field.ToLower() == "slotnumber")
            {
                this.query = @$"
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                             select * from dbo.TakaIssues
                             where dbo.TakaIssues.SlotNumber LIKE '%'+@name+'%'
                           ";
            }

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(this.query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(new TakaIssues().Search(table));
        }

        [HttpGet]
        [Route("GetTakaID/{ID}")]
        public IActionResult GetSloatNumber([FromRoute ] int ID)
        {
            return Ok(new TakaIssues().GetSlotNumber(ID));
        }



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
