using KarKhanaBook.Core.Kariger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace KarKhanaBook.Controllers.Kariger
{
    [Route("Sheet")]
    [ApiController]
    public class KarigerDailyShhetController : ControllerBase
    {
        string query ;
        private readonly IConfiguration _configuration;

        public KarigerDailyShhetController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("KarigerDailySheet/GetUser")]
        public IActionResult GetUser()
        {
            return Ok(new KarigerDailySheets().GetUser());
        }

        [HttpGet]
        [Route("KarigerDailySheet/GetShift")]
        public IActionResult GetShift()
        {
            return Ok(new KarigerDailySheets().GetShift());
        }


        [HttpPost]
        [Route("KarigerDailySheet/Search")]
        public IActionResult Search([FromBody] Model.Search.search value)
        {
          
            if (value.field.ToLower() == "all")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.KarigerDailySheet
                            LEFT JOIN dbo.Users
                            on dbo.KarigerDailySheet.UserID=dbo.Users.UserID
                            LEFT JOIN dbo.Shift
                            on dbo.KarigerDailySheet.ShiftID=dbo.Shift.ShiftID
                            where dbo.KarigerDailySheet.IndexNumber LIKE '%' +@name+ '%'
                            OR dbo.KarigerDailySheet.Date LIKE '%' +@name+ '%'
                            OR dbo.KarigerDailySheet.MachineNumber LIKE '%' +@name+ '%'
                            OR dbo.KarigerDailySheet.AVGOfMachine LIKE '%' +@name+ '%'
                            OR dbo.KarigerDailySheet.Remark LIKE '%' +@name+ '%'
                            OR dbo.Users.UserName LIKE '%' +@name+ '%'
                            OR dbo.Shift.Shift LIKE '%' +@name+ '%'
                          
                          
                            ";
            }
            else if (value.field.ToLower() == "indexnumber")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.KarigerDailySheet
                            LEFT JOIN dbo.Users
                            on dbo.KarigerDailySheet.UserID=dbo.Users.UserID
                            LEFT JOIN dbo.Shift
                            on dbo.KarigerDailySheet.ShiftID=dbo.Shift.ShiftID
                            where dbo.KarigerDailySheet.IndexNumber LIKE '%' +@name+ '%'
                           ";
            }
            else if (value.field.ToLower() =="username") 
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.KarigerDailySheet
                            LEFT JOIN dbo.Users
                            on dbo.KarigerDailySheet.UserID=dbo.Users.UserID
                            LEFT JOIN dbo.Shift
                            on dbo.KarigerDailySheet.ShiftID=dbo.Shift.ShiftID
                            where dbo.Users.UserName LIKE '%' +@name+ '%'
                           ";
            }
            else if (value.field.ToLower() == "shift") 
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.KarigerDailySheet
                            LEFT JOIN dbo.Users
                            on dbo.KarigerDailySheet.UserID=dbo.Users.UserID
                            LEFT JOIN dbo.Shift
                            on dbo.KarigerDailySheet.ShiftID=dbo.Shift.ShiftID
                            where dbo.Shift.Shift LIKE '%' +@name+ '%'
                           ";
            }
            else if (value.field.ToLower() == "date")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.KarigerDailySheet
                            LEFT JOIN dbo.Users
                            on dbo.KarigerDailySheet.UserID=dbo.Users.UserID
                            LEFT JOIN dbo.Shift
                            on dbo.KarigerDailySheet.ShiftID=dbo.Shift.ShiftID
                            where dbo.KarigerDailySheet.Date LIKE '%' +@name+ '%'
                           ";
            }
            else if (value.field.ToLower() == "machinenumber")
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.KarigerDailySheet
                            LEFT JOIN dbo.Users
                            on dbo.KarigerDailySheet.UserID=dbo.Users.UserID
                            LEFT JOIN dbo.Shift
                            on dbo.KarigerDailySheet.ShiftID=dbo.Shift.ShiftID
                            where dbo.KarigerDailySheet.MachineNumber LIKE '%' +@name+ '%'
                           ";
            }
            else if (value.field.ToLower() == "avgofmachine") 
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.KarigerDailySheet
                            LEFT JOIN dbo.Users
                            on dbo.KarigerDailySheet.UserID=dbo.Users.UserID
                            LEFT JOIN dbo.Shift
                            on dbo.KarigerDailySheet.ShiftID=dbo.Shift.ShiftID
                            where dbo.KarigerDailySheet.AVGOfMachine LIKE '%' +@name+ '%'
                           ";
            }
            else if (value.field.ToLower() == "remark") 
            {
                this.query = @$"
                            
                            DECLARE @name AS VARCHAR(100)
                            SET @name = '{value.keyword}'
                            select * from dbo.PaymentSlips
                            where dbo.PaymentSlips.PaymentSlipIndex LIKE '%'+@name+'%'
                            OR dbo.PaymentSlips.ChallanSlipSerialNumber LIKE '%'+@name+'%'
                            OR dbo.PaymentSlips.BillSerialNumber LIKE '%'+@name+'%'
                            OR dbo.PaymentSlips.Payment LIKE '%'+@name+'%'
                            OR dbo.PaymentSlips.Remark LIKE '%'+@name+'%'
                            OR dbo.PaymentSlips.TotalWeight LIKE '%'+@name+'%'
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
            
            
          
            return Ok(new KarigerDailySheets().Search(table));
        }

        [HttpPost]
        [Route("KarigerDailySheet/Add")]
        public IActionResult Add([FromBody] Model.Kariger.KarigerDailySheet value)
        {
            return Ok(new KarigerDailySheets().Add(value));
        }

        [HttpGet]
        [Route("KarigerDailySheet/View")]
        public IActionResult View()
        {
            return Ok(new KarigerDailySheets().View());
        }

        [HttpGet]
        [Route("KarigerDailySheet/ViewByID/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new KarigerDailySheets().ViewById(ID));
        }

        [HttpPut]
        [Route("KarigerDailySheet/Update/{ID}")]
        public IActionResult Update([FromBody] Model.Kariger.KarigerDailySheet value, [FromRoute] int ID)
        {
            return Ok(new KarigerDailySheets().Update(value, ID));
        }

        [HttpDelete]
        [Route("KarigerDailySheet/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new KarigerDailySheets().Delete(ID));
        }
    }
}
