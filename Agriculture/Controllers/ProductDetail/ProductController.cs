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
    [Route("ProductDetail")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Product/add")]
        public IActionResult Add([FromBody] Models.ProductDetail.Product value)
        {
            return Ok(new Products().Add(value));
        }

        [HttpGet]
        [Route("Product/getUnit")]
        public IActionResult getUnit()
        {
            return Ok(new Products().GetUnit());
        }

        [HttpGet]
        [Route("Product/getProduct")]
        public IActionResult getProduct()
        {
            return Ok(new Products().GetProduct());
        }

        [HttpGet]
        [Route("Product/view")]
        public IActionResult View()
        {
            return Ok(new Products().View());
        }


        [HttpGet]
        [Route("Product/viewSearch/{keyword}")]
        public IActionResult ViewSearch(string keyword)
        {
         
            string query = @$"
                            
                           select * from dbo.Products 
                           LEFT JOIN Categories
                           ON DBO.Products.CategoryID=DBO.Categories.CategoryID
                           LEFT JOIN LoginDetails
                           ON DBO.Products.LoginID=DBO.LoginDetails.LoginID
                           LEFT JOIN ProductUnits
                           ON DBO.Products.UnitID=DBO.ProductUnits.UnitID
                           where ProductName Like '%{keyword}%' 
                           or Variety Like '%{keyword}%' 
                           or Company Like '%{keyword}%' 
                           or DBO.Categories.CategoryName Like '%{keyword}%' 
                           or DBO.ProductUnits.Type Like '%{keyword}%'
                           or DBO.LoginDetails.UserName Like '%{keyword}%'
                           
                           or dBO.Products.Description Like '%{keyword}%'
                           or TotalProductQuantity Like '%{keyword}%' 
                           or dBO.Products.DateTime Like '%{keyword}%' 

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
            return Ok(new Products().ViewSearch(table));

            /*return Ok(new Products().View());*/
        }

        [HttpGet]
        [Route("Product/viewById/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new Products().ViewById(ID));
        }

        [HttpPut]
        [Route("Product/update/{ID}")]
        public IActionResult Update([FromBody] Models.ProductDetail.Product value, [FromRoute] int ID)
        {
            return Ok(new Products().Update(value, ID));
        }

        /* [Route("Product/delete/{ID}")]*/
    }
}
