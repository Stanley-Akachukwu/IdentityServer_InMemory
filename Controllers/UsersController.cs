using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("GetStrings")]
        public IActionResult GetStrings()
        {
            List<string> result = new List<string> { "1","2","3"};
            return Ok(result);
        }
    }
}
