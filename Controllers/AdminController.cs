using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("GetAdminUsers")]
        public IActionResult GetAdminUsers()
        {
            var users = TestUsers.Users;
            return Ok(users);
        }
    }
}
