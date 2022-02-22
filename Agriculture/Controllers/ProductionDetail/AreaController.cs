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
    public class AreaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AreaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Area/add")]
        public IActionResult Add([FromBody] Models.ProductionDetail.Area value) 
        {
            return Ok(new Areas().Add(value));
        }

        [HttpGet]
        [Route("Area/getMainArea")]
        public IActionResult GetMainArea() 
        {
            return Ok(new Areas().GetMainArea());
        }

        [HttpGet]
        [Route("Area/getSubArea/{ID}")]
        public IActionResult GetSubArea([FromRoute] int ID)
        {
            return Ok(new Areas().GetSubArea(ID));
        }

        [HttpGet]
        [Route("Area/viewSubArea")]
        public IActionResult ViewSubArea() 
        {
            return Ok(new Areas().ViewSubArea());
        }

        [HttpGet]
        [Route("Area/viewSearch/{keyword}")]
        public IActionResult viewSearch(string keyword)
        {/*
            or dbo.SubArea.DateTime Like '%{keyword}%'*/
            string query = @$"
                            
                            select * from dbo.SubArea
                            LEFT JOIN dbo.MainArea
                            on dbo.SubArea.MainAreaID=dbo.MainArea.MainAreaID
                            LEFT JOIN dbo.LoginDetails
                            on dbo.SubArea.LoginID=dbo.LoginDetails.LoginID
                            where dbo.SubArea.SubAreaID Like '%{keyword}%'
                            or dbo.SubArea.SubAreaName Like '%{keyword}%'
                            or dbo.SubArea.Remark Like '%{keyword}%'
                            
                            or dbo.MainArea.MainAreaName Like '%{keyword}%'
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
            
            return Ok(new Areas().ViewSearch(table) );
        }

        [HttpGet]
        [Route("Area/viewSubAreaByID/{ID}")]
        public IActionResult ViewSubAreaByID([FromRoute] int ID)
        {
            return Ok(new Areas().ViewSubAreaByID(ID));
        }



        /*  [Route("Area/view")]
          [Route("Area/viewById/{ID}")]
          [Route("Area/update/{ID}")]
          [Route("Area/delete/{ID}")]*/
    }
}
