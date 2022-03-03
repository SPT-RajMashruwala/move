using KarKhanaBook.Core.Challan;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace KarKhanaBook.Controllers.Challan
{
    [Route("challan")]
    [ApiController]
    public class ChallanPaymentController : ControllerBase
    {
        string query;
        private readonly IConfiguration _configuration;

        public ChallanPaymentController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("ChallanPayment/Add")]
        public IActionResult Add([FromBody] Model.Challan.ChallanPayment value)
        {
            return Ok(new ChallanPayments().Add(value));

        }

        [HttpPost]
        [Route("ChallanPayment/Search")]
        public IActionResult Search([FromBody] Model.Search.search value)
        {
          
            if (value.field.ToLower() == "all")
            {
                this.query = $@"
                             DECLARE @name AS VARCHAR(100)
                             SET @name= '{value.keyword}'
                             select * from dbo.PaymentSlips
                             where dbo.PaymentSlips.PaymentSlipIndex LIKE '%'+@name+'%'
                             OR dbo.PaymentSlips.ChallanSlipSerialNumber LIKE '%'+@name+'%'
                             OR dbo.PaymentSlips.BillSerialNumber LIKE '%'+@name+'%'
                             OR dbo.PaymentSlips.Payment LIKE '%'+@name+'%'
                             OR dbo.PaymentSlips.Remark LIKE '%'+@name+'%'
                             OR dbo.PaymentSlips.TotalWeight LIKE '%'+@name+'%'
                          ";
            }
            else if (value.field.ToLower() == "paymentslipindex")
            {
                this.query = $@"
                             DECLARE @name AS VARCHAR(100)
                             SET @name= '{value.keyword}'
                             select * from dbo.PaymentSlips
                             where dbo.PaymentSlips.PaymentSlipIndex LIKE '%'+@name+'%'
                            
                          ";
            }
            else if (value.field.ToLower() == "challanslipserialnumber")
            {
                this.query = $@"
                             DECLARE @name AS VARCHAR(100)
                             SET @name= '{value.keyword}'
                             select * from dbo.PaymentSlips
                             where  dbo.PaymentSlips.ChallanSlipSerialNumber LIKE '%'+@name+'%'
                             
                          ";
            }
            else if (value.field.ToLower() == "billserialnumber")
            {
                this.query = $@"
                             DECLARE @name AS VARCHAR(100)
                             SET @name= '{value.keyword}'
                             select * from dbo.PaymentSlips
                             where dbo.PaymentSlips.BillSerialNumber LIKE '%'+@name+'%'
                   
                          ";
            }
            else if (value.field.ToLower() == "payment")
            {
                this.query = $@"
                             DECLARE @name AS VARCHAR(100)
                             SET @name= '{value.keyword}'
                             select * from dbo.PaymentSlips
                             where dbo.PaymentSlips.Payment LIKE '%'+@name+'%'
                            
                          ";
            }
            else if (value.field.ToLower() == "remark")
            {
                this.query = $@"
                             DECLARE @name AS VARCHAR(100)
                             SET @name= '{value.keyword}'
                             select * from dbo.PaymentSlips
                             where dbo.PaymentSlips.Remark LIKE '%'+@name+'%'
                             
                          ";
            }
            else if (value.field.ToLower() == "totalweight")
            {
                this.query = $@"
                             DECLARE @name AS VARCHAR(100)
                             SET @name= '{value.keyword}'
                             select * from dbo.PaymentSlips
                             where dbo.PaymentSlips.TotalWeight LIKE '%'+@name+'%'
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
            return Ok(new ChallanPayments().Search(table));

        }


        [HttpGet]
        [Route("ChallanPayment/View")]
        public IActionResult View()
        {
            return Ok(new ChallanPayments().View());

        }


        [HttpGet]
        [Route("ChallanPayment/ViewById/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new ChallanPayments().ViewByID(ID));

        }

        [HttpPut]
        [Route("ChallanPayment/Update/{ID}")]
        public IActionResult ViewByID([FromBody] Model.Challan.ChallanPayment value, [FromRoute] int ID)
        {
            return Ok(new ChallanPayments().Update(value, ID));

        }

        [HttpDelete]
        [Route("ChallanPayment/Delete/{ID}")]
        public IActionResult Delete([FromRoute] int ID)
        {
            return Ok(new ChallanPayments().Delete(ID));

        }
    }
}
