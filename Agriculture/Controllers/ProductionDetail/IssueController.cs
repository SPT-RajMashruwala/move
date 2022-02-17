using Agriculture.Core.ProductionDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Agriculture.Controllers.ProductionDetail
{
    [Route("ProductionDetail")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public IssueController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("IssueProducts/add")]
        public IActionResult Add([FromBody] Models.ProductionDetail.Issue value) 
        {
            return Ok(new Issues().Add(value));
        }


        [HttpGet]
        [Route("IssueProducts/view")]
        public IActionResult View()
        {
          
            return Ok(new Issues().View());
        }

        [HttpGet]
        [Route("IssueProducts/viewSearch/{keyword}")]
        public IActionResult ViewSearch(string keyword)
        {
            string query = @$"
                            
                            select * from dbo.Issues
                            LEFT JOIN dbo.MainArea
                            on dbo.Issues.MainAreaID=dbo.MainArea.MainAreaID
                            LEFT JOIN dbo.SubArea
                            on dbo.Issues.SubAreaID=dbo.SubArea.SubAreaID
                            LEFT JOIN dbo.Products
                            on dbo.Issues.ProductID=dbo.Products.ProductID
                            LEFT JOIN LoginDetails
                            on dbo.Issues.LoginID=dbo.LoginDetails.LoginID
                            where dbo.Issues.IssueID Like '%{keyword}%'
                            or dbo.Issues.IssueDate Like '%{keyword}%'
                            or dbo.Issues.PurchaseQuantity Like '%{keyword}%'
                            or dbo.Issues.Remark Like '%{keyword}%'
                            or dbo.Issues.DateTime Like '%{keyword}%'
                            or dbo.MainArea.MainAreaName Like '%{keyword}%'
                            or dbo.SubArea.SubAreaName Like '%{keyword}%'
                            or dbo.Products.ProductName Like '%{keyword}%'
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
          
            return Ok(new Issues().ViewSearch(table));
        }



        [HttpGet]
        [Route("IssueProducts/viewById/{ID}")]
        public IActionResult ViewByID([FromRoute] int ID)
        {
            return Ok(new Issues().ViewById(ID));
        }

        [HttpPut]
        [Route("IssueProducts/update/{ID}")]
        public IActionResult Update([FromBody] Models.ProductionDetail.Issue value, [FromRoute] int ID) 
        {
            return Ok(new Issues().Update(value,ID));
        }
        /*[Route("IssueProducts/delete/{ID}")]*/
    }
}
