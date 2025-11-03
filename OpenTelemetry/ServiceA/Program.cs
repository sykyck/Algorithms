using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.Grafana.Loki.HttpClients;
using ServiceA.Middleware;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Get environment variables
var serviceName = Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "ServiceA";
var lokiUrl = Environment.GetEnvironmentVariable("LOKI_URL") ?? "http://loki:3100";
var tempoUrl = Environment.GetEnvironmentVariable("TEMPO_URL") ?? "http://tempo:4317";

// Create ActivitySource
var activitySource = new ActivitySource(serviceName);

// Enable Serilog internal logging
SelfLog.Enable(msg => Console.WriteLine($"[SERILOG INTERNAL] {msg}"));

Console.WriteLine($"[DEBUG] Starting ServiceA with Loki: {lokiUrl}");

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
     )
    .WriteTo.GrafanaLoki(
        uri: lokiUrl,
        labels: new List<LokiLabel>
        {
            new LokiLabel { Key = "app", Value = serviceName }
        },
        propertiesAsLabels: new List<string> { "app" },
        httpClient: new LokiHttpClient()
     )
    .Enrich.FromLogContext()
    .Enrich.WithProperty("app", serviceName)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();

// Add HttpClient for outgoing calls to ServiceB
builder.Services.AddHttpClient();

builder.Services.AddSingleton(activitySource); // Register ActivitySource

// Add OpenTelemetry

// Configure OpenTelemetry
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(serviceName: serviceName, serviceVersion: "1.0.0")
    .AddTelemetrySdk()
    .AddEnvironmentVariableDetector();

// Configure OpenTelemetry Tracing & Metrics
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "ServiceA"))
    .WithTracing(tracing => tracing
        .SetResourceBuilder(resourceBuilder)
        .AddAspNetCoreInstrumentation(options =>
        {
            options.RecordException = true;
            options.EnrichWithException = (activity, exception) =>
            {
                activity.SetTag("stackTrace", exception.StackTrace);
            };
        })
        .AddHttpClientInstrumentation(options =>
        {
            options.RecordException = true;
        })
        .AddSource(serviceName)
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri(Environment.GetEnvironmentVariable("TEMPO_URL") ?? "http://tempo:4317");
            opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        }))
    .WithMetrics(metrics => metrics
        .SetResourceBuilder(resourceBuilder)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddMeter("Microsoft.AspNetCore.Hosting")
        .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
        .AddMeter(serviceName)
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri(Environment.GetEnvironmentVariable("TEMPO_URL") ?? "http://tempo:4317");
            opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        }));

var app = builder.Build();

// Simple metrics tracking - using manual counters
var requestCount = 0L;
var errorCount = 0L;
var requestDurationSum = 0.0;
var requestDurationCount = 0L;

// Simple metrics endpoint in Prometheus format
app.MapGet("/metrics", () =>
{
    var metrics = new StringBuilder();

    // Request count
    metrics.AppendLine("# HELP app_request_count Total number of requests");
    metrics.AppendLine("# TYPE app_request_count counter");
    metrics.AppendLine($"app_request_count{{service=\"{serviceName}\"}} {requestCount}");

    // Error count
    metrics.AppendLine("# HELP app_error_count Total number of errors");
    metrics.AppendLine("# TYPE app_error_count counter");
    metrics.AppendLine($"app_error_count{{service=\"{serviceName}\"}} {errorCount}");

    // Average request duration
    var avgDuration = requestDurationCount > 0 ? requestDurationSum / requestDurationCount : 0;
    metrics.AppendLine("# HELP app_request_duration_seconds Average request duration in seconds");
    metrics.AppendLine("# TYPE app_request_duration_seconds gauge");
    metrics.AppendLine($"app_request_duration_seconds{{service=\"{serviceName}\"}} {avgDuration:F6}");

    // Request duration count
    metrics.AppendLine("# HELP app_request_duration_count Number of request duration measurements");
    metrics.AppendLine("# TYPE app_request_duration_count counter");
    metrics.AppendLine($"app_request_duration_count{{service=\"{serviceName}\"}} {requestDurationCount}");

    // Service status
    metrics.AppendLine("# HELP app_up Service status");
    metrics.AppendLine("# TYPE app_up gauge");
    metrics.AppendLine($"app_up{{service=\"{serviceName}\"}} 1");

    // Process info
    metrics.AppendLine("# HELP process_start_time_seconds Start time of the process since unix epoch in seconds");
    metrics.AppendLine("# TYPE process_start_time_seconds gauge");
    metrics.AppendLine($"process_start_time_seconds{{service=\"{serviceName}\"}} {DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");

    return metrics.ToString();
});

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

// Test endpoint to generate logs
app.MapGet("/test-logs", () =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("This is a test INFO log from ServiceA");
    logger.LogWarning("This is a test WARNING log from ServiceA");
    logger.LogError("This is a test ERROR log from ServiceA");

    return "Test logs generated!";
});


// Test endpoint that generates traces
app.MapGet("/test-trace", async (ILogger<Program> logger, IHttpClientFactory httpClientFactory, ActivitySource activitySource) =>
{
    using var activity = activitySource.StartActivity("test-trace", ActivityKind.Server);

    activity?.SetTag("test.operation", "trace-generation");
    activity?.SetTag("test.service", serviceName);

    logger.LogInformation("Starting test trace operation");

    // Simulate some work
    await Task.Delay(100);

    // Make an HTTP call to generate span
    var client = httpClientFactory.CreateClient();
    try
    {
        using var httpActivity = activitySource.StartActivity("call-service-b", ActivityKind.Client);
        var response = await client.GetAsync("http://serviceb:5001/test-logs");
        logger.LogInformation("Called ServiceB, response: {StatusCode}", response.StatusCode);

        httpActivity?.SetTag("http.status_code", (int)response.StatusCode);
        httpActivity?.SetTag("http.url", "http://serviceb:5001/test-logs");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to call ServiceB");
        activity?.SetStatus(ActivityStatusCode.Error);
        activity?.SetTag("error.message", ex.Message);
    }

    logger.LogInformation("Completed test trace operation");

    return new
    {
        message = "Test trace completed!",
        traceId = activity?.TraceId.ToString(),
        spanId = activity?.SpanId.ToString(),
        service = serviceName
    };
});

// Another test endpoint with more complex tracing
app.MapGet("/test-complex-trace", async (ILogger<Program> logger, ActivitySource activitySource) =>
{
    using var mainActivity = activitySource.StartActivity("complex-operation", ActivityKind.Server);

    logger.LogInformation("Starting complex trace operation");

    // Simulate multiple operations
    await SimulateDatabaseCall(activitySource, logger);
    await SimulateExternalApiCall(activitySource, logger);
    await SimulateProcessing(activitySource, logger);

    logger.LogInformation("Completed complex trace operation");

    return new
    {
        message = "Complex trace completed!",
        traceId = mainActivity?.TraceId.ToString(),
        service = serviceName
    };
});

// Helper methods for complex tracing
async Task SimulateDatabaseCall(ActivitySource activitySource, ILogger<Program> logger)
{
    using var activity = activitySource.StartActivity("database-query", ActivityKind.Internal);
    activity?.SetTag("db.operation", "SELECT");
    activity?.SetTag("db.table", "users");

    logger.LogInformation("Simulating database query");
    await Task.Delay(50);
    activity?.SetTag("db.rows_returned", 10);
}

async Task SimulateExternalApiCall(ActivitySource activitySource, ILogger<Program> logger)
{
    using var activity = activitySource.StartActivity("external-api-call", ActivityKind.Client);
    activity?.SetTag("http.method", "GET");
    activity?.SetTag("http.url", "https://api.example.com/data");

    logger.LogInformation("Simulating external API call");
    await Task.Delay(75);
    activity?.SetTag("http.status_code", 200);
}

async Task SimulateProcessing(ActivitySource activitySource, ILogger<Program> logger)
{
    using var activity = activitySource.StartActivity("data-processing", ActivityKind.Internal);
    activity?.SetTag("processing.type", "batch");
    activity?.SetTag("processing.items", 100);

    logger.LogInformation("Simulating data processing");
    await Task.Delay(25);
}


app.Run();
