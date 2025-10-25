using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorldController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Ok("Hello from ServiceB");
    }
}
