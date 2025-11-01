using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.Grafana.Loki.HttpClients;
using ServiceA.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Get environment variables
var serviceName = Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "ServiceA";
var lokiUrl = Environment.GetEnvironmentVariable("LOKI_URL") ?? "http://loki:3100";
var tempoUrl = Environment.GetEnvironmentVariable("TEMPO_URL") ?? "http://tempo:4317";

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

// Add OpenTelemetry

// Configure OpenTelemetry Tracing & Metrics
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "ServiceA"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri(Environment.GetEnvironmentVariable("TEMPO_URL") ?? "http://tempo:4317");
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddMeter("Microsoft.AspNetCore.Hosting")
        .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri(Environment.GetEnvironmentVariable("TEMPO_URL") ?? "http://tempo:4317");
        }));

var app = builder.Build();

// Example using ILogger in middleware
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    using (logger.BeginScope("{TraceId}", context.TraceIdentifier))
    {
        logger.LogInformation("Starting request: {Method} {Path}",
            context.Request.Method, context.Request.Path);
        await next();
        logger.LogInformation("Completed request: {Method} {Path} - {StatusCode}",
            context.Request.Method, context.Request.Path, context.Response.StatusCode);
    }
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


app.Run();
