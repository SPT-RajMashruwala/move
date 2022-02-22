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
    public class ProductionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Production/add")]
        public IActionResult Add([FromBody] Models.ProductionDetail.Production value) 
        {
            return Ok(new Productions().Add(value));
        }


        [HttpGet]
        [Route("Production/view")]
        public IActionResult View() 
        {
            return Ok(new Productions().View());
        }

        [HttpGet]
        [Route("Production/viewSearch/{keyword}")]
        public IActionResult ViewSearch(string keyword) 
        {
          /*  or dbo.ProductionDetails.DateTime Like '%{keyword}%'*/
            string query = @$"
                            
                           select * from dbo.ProductionDetails
                           LEFT JOIN dbo.MainArea
                           on dbo.ProductionDetails.MainAreaID=dbo.MainArea.MainAreaID
                           LEFT JOIN dbo.SubArea
                           on dbo.ProductionDetails.SubAreaID=dbo.SubArea.SubAreaID
                           LEFT JOIN dbo.Vegetables
                           on dbo.ProductionDetails.VegetableID=dbo.Vegetables.VegetableID
                           LEFT JOIN dbo.LoginDetails
                           on dbo.ProductionDetails.LoginID=dbo.LoginDetails.LoginID
                          where dbo.ProductionDetails.ProductionID Like '%{keyword}%'
                          or dbo.ProductionDetails.Quantity Like '%{keyword}%'
                          or dbo.ProductionDetails.Remark Like '%{keyword}%'
                          or dbo.MainArea.MainAreaName Like '%{keyword}%'
                          or dbo.SubArea.SubAreaName Like '%{keyword}%'
                        
                           or dbo.Vegetables.VegetableName Like '%{keyword}%'
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
            
            return Ok(new Productions().ViewSearch(table));
        }


        [HttpGet]
        [Route("Production/viewById/{ID}")]
        public IActionResult ViewByID([FromRoute ] int ID) 
        {
            return Ok(new Productions().ViewById(ID));
        }


        [HttpPut]
        [Route("Production/update/{ID}")]
        public IActionResult Update(Models.ProductionDetail.Production value, int ID)
        {
            return Ok(new Productions().Update(value,ID));
        }
/*        [Route("Production/delete/{ID}")]*/
    }
}
