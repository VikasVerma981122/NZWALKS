using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWALKS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents() 
        {
            string[] studentname = new string[] { "John","Sara","Mike","Jane","Michel" };
            return Ok(studentname);

        
        }
    }
}
