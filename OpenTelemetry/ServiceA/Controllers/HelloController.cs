using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace ServiceA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private static readonly ActivitySource ActivitySource = new ActivitySource(Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? "ServiceA");


        public HelloController(IHttpClientFactory clientFactory, TracerProvider tracerProvider)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetStringAsync($"{Environment.GetEnvironmentVariable("SERVICEB_URL")}/world");
            return Ok($"ServiceA says: {response}");
        }

        [HttpGet("test")]
        public IActionResult Test([FromServices] TracerProvider tracerProvider)
        {
            using var activity = ActivitySource.StartActivity("TestSpan");
            activity?.SetTag("test-attr", "hello");
            return Ok("Span created");
        }
    }
}
