using KarKhanaBook.Core.Taka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace KarKhanaBook.Controllers.Taka
{
    [Route("Sheet")]
    [ApiController]
    public class TakaSheetController : ControllerBase
    {
        string query;
        private readonly IConfiguration _configuration;

        public TakaSheetController(IConfiguration configuration )
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("TakaSheet/Add")]
        public IActionResult Add([FromBody] Model.Taka.TakaSheet value)
        {
            return Ok(new TakaSheets().Add(value));
        }


        [HttpPost]
        [Route("TakaSheet/Search")]
        public IActionResult Search([FromBody] Model.Search.search value)
        {

            if (value.field.ToLower() == "all")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaSheet
                            where dbo.TakaSheet.TakaSheetIndex LIKE '%'+@name+'%'
                            OR dbo.TakaSheet.SlotNumber LIKE '%'+@name+'%'
                            OR dbo.TakaSheet.TakaID LIKE '%'+@name+'%'
                            OR dbo.TakaSheet.MachineNumber LIKE '%'+@name+'%'
                            OR dbo.TakaSheet.Meter LIKE '%'+@name+'%'
                            OR dbo.TakaSheet.Weight LIKE'%'+@name+'%'
                            OR dbo.TakaSheet.Date LIKE '%'+@name+'%'
                            
                           ";
            }
            else if (value.field.ToLower() == "takasheetindex")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaSheet
                            where dbo.TakaSheet.TakaSheetIndex LIKE '%'+@name+'%'
                            
                            
                           ";
            }
            else if (value.field.ToLower() == "slotnumber")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaSheet
                            where  dbo.TakaSheet.SlotNumber LIKE '%'+@name+'%'
                        
                            
                           ";
            }
            else if (value.field.ToLower() == "takaid")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaSheet
                            where  dbo.TakaSheet.TakaID LIKE '%'+@name+'%'
                           
                            
                           ";
            }
            else if (value.field.ToLower() == "machinenumber")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaSheet
                            where  dbo.TakaSheet.MachineNumber LIKE '%'+@name+'%'
                           
                            
                           ";
            }
            else if (value.field.ToLower() == "meter")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaSheet
                            where  dbo.TakaSheet.Meter LIKE '%'+@name+'%'
                         
                            
                           ";
            }
            else if (value.field.ToLower() == "weight")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaSheet
                            where  dbo.TakaSheet.Weight LIKE'%'+@name+'%'
                         
                            
                           ";
            }
            else if (value.field.ToLower() == "date")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.TakaSheet
                            where dbo.TakaSheet.Date LIKE '%'+@name+'%'
                            
                           ";
            }
            else 
            {
                throw new System.ArgumentException("please enter valid field!!");
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
            return Ok(new TakaSheets().Search(table));
        }

        [HttpGet]
        [Route("TakaSheet/View")]
        public IActionResult View()
        {
            return Ok(new TakaSheets().View());
        }

        [HttpGet]
        [Route("TakaSheet/Filter")]
        public IActionResult Filter()
        {
            return Ok(new TakaSheets().Filter());
        }

        [HttpGet]
        [Route("TakaSheet/ViewById/{ID}")]
        public IActionResult ViewById([FromRoute] int ID)
        {
            return Ok(new TakaSheets().ViewByID(ID));
        }

        [HttpPut]
        [Route("TakaSheet/Update/{ID}")]
        public IActionResult Update([FromBody] Model.Taka.TakaSheet value, [FromRoute] int ID)
        {
            return Ok(new TakaSheets().Update(value, ID));
        }

        [HttpDelete]
        [Route("TakaSheet/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new TakaSheets().Delete(ID));
        }
    }
}
