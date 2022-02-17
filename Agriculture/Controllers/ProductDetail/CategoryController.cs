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
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Category/add")]
        public IActionResult Add([FromBody] Models.ProductDetail.Category value) 
        {
            return Ok(new Categorys().Add(value));
        }

        [HttpGet]
        [Route("Category/view")]
        public IActionResult View() 
        {
            return Ok(new Categorys().View());
        }

        [HttpGet]
        [Route("Category/viewSerach/{keyword}")]
        public IActionResult ViewSearch(string keyword)
        {

            string query = @$"
                            
                            select * from dbo.Categories
                            LEFT JOIN LoginDetails
                            on dbo.Categories.LoginID=dbo.LoginDetails.LoginID
                            where CategoryID Like '%{keyword}%'
                            or dbo.Categories.CategoryName Like '%{keyword}%'
                            or dbo.Categories.Description Like '%{keyword}%'
                            or dbo.Categories.DateTime Like '%{keyword}%'
                            or dbo.LoginDetails.UserName Like '%{keyword}%'

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
            return Ok(new Categorys().ViewSearch(table));

           
        }

        [HttpGet]
        [Route("Category/viewById/{ID}")]
        public IActionResult View([FromRoute] int ID)
        {
            return Ok(new Categorys().ViewById(ID));
        }

        [HttpPut]
        [Route("Category/update/{ID}")]
        public IActionResult update([FromBody] Models.ProductDetail.Category value,[FromRoute] int ID) 
        {
            return Ok(new Categorys().update(value,ID));
        }



        /*[Route("Category/delete/{ID}")]*/
    }
}
