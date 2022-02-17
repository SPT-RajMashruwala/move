using Agriculture.Core.ProductDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Controllers.ProductDetail
{
    [Route("ProductionDetail")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PurchaseController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Purchase/add")]
        public IActionResult Add([FromBody] Models.ProductDetail.Purchase value)
        {
            return Ok(new Purchases().Add(value));
        }


        [HttpGet]
        [Route("Purchase/view")]
        public IActionResult View()
        {
            return Ok(new Purchases().View());
        }

        [HttpGet]
        [Route("Purchase/viewSearch/{keyword}")]
        public IActionResult ViewSearch(string keyword)
        {
            string query = @$"
                            
                          select * from dbo.PurchaseDetails
                          LEFT JOIN LoginDetails
                          on dbo.PurchaseDetails.LoginID=dbo.LoginDetails.LoginID
                          LEFT JOIN Products
                          on dbo.PurchaseDetails.ProductID=dbo.Products.ProductID
                          LEFT JOIN ProductUnits
                          on dbo.PurchaseDetails.UnitID=dbo.ProductUnits.UnitID
                          where dbo.PurchaseDetails.PurchaseID Like '%{keyword}%'
                          or dbo.PurchaseDetails.TotalQuantity Like '%{keyword}%'
                          or dbo.PurchaseDetails.TotalCost Like '%{keyword}%'
                          or dbo.PurchaseDetails.VendorName Like '%{keyword}%'
                           or dbo.PurchaseDetails.PurchaseDate Like '%{keyword}%'
                           or dbo.PurchaseDetails.Remark Like '%{keyword}%'
                            or dbo.Products.ProductName Like '%{keyword}%'
   	                       or dbo.LoginDetails.UserName Like '%{keyword}%'
   	                        or dbo.ProductUnits.Type Like '%{keyword}%'
   	                         or dbo.PurchaseDetails.DateTime Like '%{keyword}%'
                          
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return Ok(new Purchases().ViewSearch(table));
        }


        [HttpGet]
        [Route("Purchase/viewById/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID) 
        {
            return Ok(new Purchases().ViewById(ID));
        }


        [HttpPut]
        [Route("Purchase/update/{ID}")]
        public IActionResult Update([FromBody] Models.ProductDetail.Purchase value,[FromRoute] int ID) 
        {
            return Ok(new Purchases().Update(value,ID));
        }

      /*  [Route("Purchase/delete/{ID}")]*/


    }
}
