using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("openid-configuration")]
        public IActionResult GetOpenidConfiguration()
        {
            return Redirect("https://localhost:5001/.well-known/openid-configuration");
        }
    }
}
