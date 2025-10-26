using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace ServiceA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<HelloController> _logger;   // <-- Add logger
        private static readonly ActivitySource ActivitySource = new ActivitySource(
            Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? "ServiceA"
        );

        public HelloController(IHttpClientFactory clientFactory, ILogger<HelloController> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Calling ServiceB...");  // <-- Log before call
            var client = _clientFactory.CreateClient();
            var response = await client.GetStringAsync($"{Environment.GetEnvironmentVariable("SERVICEB_URL")}/world");
            _logger.LogInformation("Received response from ServiceB: {Response}", response);  // <-- Log response
            return Ok($"ServiceA says: {response}");
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            using var activity = ActivitySource.StartActivity("TestSpan");
            activity?.SetTag("test-attr", "hello");

            _logger.LogInformation("TestSpan created with attribute {Attr}", "hello");  // <-- Log in span

            return Ok("Span created");
        }
    }
}
