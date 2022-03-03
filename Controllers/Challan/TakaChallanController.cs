using KarKhanaBook.Core.Challan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Controllers.Challan
{
    [Route("Challan")]
    [ApiController]
    public class TakaChallanController : ControllerBase
    {
        string query;
        private readonly IConfiguration _configuration;
        public TakaChallanController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        


        [HttpPost]
        [Route("TakaChallan/Add")]
        public IActionResult Add([FromBody] Model.Challan.TakaChallan value)
        {
            return Ok(new TakaChallans().Add(value));
        }

        [HttpPost]
        [Route("TakaChallan/Search")]
        public IActionResult Search([FromBody] Model.Search.search value)
        {
           
            if (value.field.ToLower() == "all")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaChallan
                            where dbo.TakaChallan.TakaChallanIndex LIKE '%'+@name+'%'
                            OR dbo.TakaChallan.TakaChallanNumber LIKE '%'+@name+'%'
                            OR dbo.TakaChallan.TotalTakaQuantity LIKE '%'+@name+'%'
                            OR dbo.TakaChallan.TotalMeter LIKE '%'+@name+'%'
                            OR dbo.TakaChallan.TotalWeight LIKE '%'+@name+'%'
                            OR dbo.TakaChallan.RsPerMeter LIKE '%'+@name+'%'
                            OR dbo.TakaChallan.TotalBillValue LIKE '%'+@name+'%'
                            OR dbo.TakaChallan.Remark LIKE '%'+@name+'%'
                           ";
            }
            else if (value.field.ToLower() == "takachallanindex") 
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaChallan
                            where dbo.TakaChallan.TakaChallanIndex LIKE '%'+@name+'%'
                            
                           ";
            }
            else if (value.field.ToLower() == "takachallannumber")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaChallan
                            where dbo.TakaChallan.TakaChallanNumber LIKE '%'+@name+'%'
                            
                           ";
            }
            else if (value.field.ToLower() == "totaltakaquantity")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaChallan
                            where dbo.TakaChallan.TotalTakaQuantity LIKE '%'+@name+'%'
                            
                           ";
            }
            else if (value.field.ToLower() == "totalmeter")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaChallan
                            where dbo.TakaChallan.TotalMeter LIKE '%'+@name+'%'
                            
                           ";
            }
            else if (value.field.ToLower() == "totalweight")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaChallan
                            where dbo.TakaChallan.TotalWeight LIKE '%'+@name+'%'
                            
                           ";
            }
            else if (value.field.ToLower() == "rspermeter")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaChallan
                            where dbo.TakaChallan.RsPerMeter LIKE '%'+@name+'%'
                            
                           ";
            }
            else if (value.field.ToLower() == "totalbillvalue")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaChallan
                            where dbo.TakaChallan.TotalBillValue LIKE '%'+@name+'%'
                            
                           ";
            }
            else if (value.field.ToLower() == "remark")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaChallan
                            where dbo.TakaChallan.Remark LIKE '%'+@name+'%'
                            
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
            return Ok(new TakaChallans().Search(table));
        }


        [HttpGet]
        [Route("TakaChallan/View")]
        public IActionResult View()
        {
            return Ok(new TakaChallans().View());
        }


        [HttpGet]
        [Route("TakaChallan/ViewById/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new TakaChallans().ViewById(ID));
        }


        [HttpPut]
        [Route("TakaChallan/Update/{ID}")]
        public IActionResult Update([FromBody] Model.Challan.TakaChallan value, [FromRoute] int ID)
        {
            return Ok(new TakaChallans().Update(value, ID));
        }


        [HttpDelete]
        [Route("TakaChallan/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new TakaChallans().Delete(ID));
        }
    }
}
