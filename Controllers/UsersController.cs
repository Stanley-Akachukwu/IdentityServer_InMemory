using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = TestUsers.Users;
            return Ok(users);
        }
    }
}
