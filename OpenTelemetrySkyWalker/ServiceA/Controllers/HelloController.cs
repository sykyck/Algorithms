using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System.Diagnostics;
using System.Net.Http;

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

        [HttpGet("testComprehensive")]
        // Test endpoint that forces multiple log types
        public async Task<dynamic> TestComprehensive()
        {
            // Test 1: Basic logs
            _logger.LogInformation("=== STARTING COMPREHENSIVE TEST ===");
            _logger.LogInformation("Information log at {Time}", DateTime.UtcNow);
            _logger.LogWarning("Warning log at {Time}", DateTime.UtcNow);
            _logger.LogError("Error log at {Time}", DateTime.UtcNow);

            // Test 2: Logs with structured data
            _logger.LogInformation("Structured log with data: {@Data}", new { UserId = 123, Action = "test", Timestamp = DateTime.UtcNow });

            // Test 3: HTTP call to generate distributed trace
            try
            {
                var client = _clientFactory.CreateClient();
                var serviceBUrl = Environment.GetEnvironmentVariable("SERVICEB_URL") ?? "http://serviceb:5001";
                _logger.LogInformation("Making HTTP call to ServiceB: {Url}", serviceBUrl);

                var response = await client.GetStringAsync($"{serviceBUrl}/hello");
                _logger.LogInformation("ServiceB responded: {Response}", response);

                // Test 4: Exception log
                try
                {
                    throw new InvalidOperationException("This is a test exception for logging");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "This is a test exception log");
                }

                _logger.LogInformation("=== COMPREHENSIVE TEST COMPLETED ===");

                return new
                {
                    Message = "Comprehensive test completed",
                    ServiceBResponse = response,
                    TestTypes = new[] { "basic_logs", "structured_logs", "http_trace", "exception_log" },
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Comprehensive test failed");
                return new { Error = ex.Message };
            }
        }
    }
}
