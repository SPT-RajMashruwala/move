using KarKhanaBook.Core.Challan;
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
    public class ChallanSlipController : ControllerBase
    {

        string query;
        private readonly IConfiguration _configuration;

        public ChallanSlipController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("ChallanSlip/Add")]
        public IActionResult Add([FromBody] Model.Challan.ChallanSlip value)
        {
            return Ok(new ChallanSlips().Add(value));
        }

        [HttpPost]
        [Route("ChallanSlip/Search")]
        public IActionResult Search(Model.Search.search value)
        {

            if (value.field.ToLower() == "all")
            {
                this.query = @$"
                            
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                           	 select * from dbo.ChallanSlip
	                         where dbo.ChallanSlip.ChallanSlipIndex LIKE '%'+@name+'%'
	                         OR dbo.ChallanSlip.ChallanSlipSerialNumber LIKE '%'+@name+'%'
	                         OR dbo.ChallanSlip.SellerName LIKE '%'+@name+'%'
	                         OR dbo.ChallanSlip.RangeCartoonSerialNumber LIKE '%'+@name+'%'
	                         OR dbo.ChallanSlip.TotalCartoons LIKE '%'+@name+'%'
	                         OR dbo.ChallanSlip.RsPerKG LIKE '%'+@name+'%'
	                         OR dbo.ChallanSlip.TotalWeight LIKE '%'+@name+'%'
	                         OR dbo.ChallanSlip.DateOfPurchase LIKE '%'+@name+'%'
	                         OR dbo.ChallanSlip.Remark LIKE '%'+@name+'%'
                           ";
            }
            else if (value.field.ToLower() == "challanslipindex") 
            {
                this.query = @$"
                            
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                           	 select * from dbo.ChallanSlip
	                         where dbo.ChallanSlip.ChallanSlipIndex LIKE '%'+@name+'%'
	                         
                           ";

            }
            else if (value.field.ToLower() == "challanslipserialnumber")
            {
                this.query = @$"
                            
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                           	 select * from dbo.ChallanSlip
	                         where  dbo.ChallanSlip.ChallanSlipSerialNumber LIKE '%'+@name+'%'
	                        
                           ";
            }
            else if (value.field.ToLower() == "sellername")
            {
                this.query = @$"
                            
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                           	 select * from dbo.ChallanSlip
	                         where  dbo.ChallanSlip.SellerName LIKE '%'+@name+'%'
	                       
                           ";
            }
            else if (value.field.ToLower() == "rangecartoonserialnumber")
            {
                this.query = @$"
                            
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                           	 select * from dbo.ChallanSlip
	                         where  dbo.ChallanSlip.RangeCartoonSerialNumber LIKE '%'+@name+'%'
	                       
                           ";
            }
            else if (value.field.ToLower() == "totalcartoons")
            {
                this.query = @$"
                            
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                           	 select * from dbo.ChallanSlip
	                         where dbo.ChallanSlip.TotalCartoons LIKE '%'+@name+'%'
	                      
                           ";
            }
            else if (value.field.ToLower() == "rsperkg")
            {
                this.query = @$"
                            
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                           	 select * from dbo.ChallanSlip
	                         where  dbo.ChallanSlip.RsPerKG LIKE '%'+@name+'%'
	                       
                           ";
            }
            else if (value.field.ToLower() == "totalweight")
            {
                this.query = @$"
                            
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                           	 select * from dbo.ChallanSlip
	                         where dbo.ChallanSlip.TotalWeight LIKE '%'+@name+'%'
	                         
                           ";
            }
            else if (value.field.ToLower() == "dateofpurchase")
            {
                this.query = @$"
                            
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                           	 select * from dbo.ChallanSlip
	                         where dbo.ChallanSlip.DateOfPurchase LIKE '%'+@name+'%'
	                        
                           ";
            }
            else if (value.field.ToLower() == "remark")
            {
                this.query = @$"
                            
                             DECLARE @name AS VARCHAR(100)
                             SET @name = '{value.keyword}'
                           	 select * from dbo.ChallanSlip
	                         where dbo.ChallanSlip.Remark LIKE '%'+@name+'%'
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
            return Ok(new ChallanSlips().Search(table));
        }

        [HttpGet]
        [Route("ChallanSlip/View")]
        public IActionResult View()
        {
            return Ok(new ChallanSlips().View());
        }

        [HttpGet]
        [Route("ChallanSlip/ViewByID/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new ChallanSlips().ViewByID(ID));
        }

        [HttpPut]
        [Route("ChallanSlip/Update/{ID}")]
        public IActionResult Update([FromBody] Model.Challan.ChallanSlip value, [FromRoute] int ID)
        {
            return Ok(new ChallanSlips().Update(value, ID));
        }

        [HttpDelete]
        [Route("ChallanSlip/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new ChallanSlips().Delete(ID));
        }
    }
}
