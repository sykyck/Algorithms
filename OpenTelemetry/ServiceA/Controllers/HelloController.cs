using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public HelloController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetStringAsync($"{Environment.GetEnvironmentVariable("SERVICEB_URL")}/world");
            return Ok($"ServiceA says: {response}");
        }
    }
}
