// Program.cs (for .NET 6/7)
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using ServiceB.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.GrafanaLoki(Environment.GetEnvironmentVariable("LOKI_URL") ?? "http://loki:3100")
    .Enrich.FromLogContext()
    .Enrich.WithProperty("service", Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "ServiceB")
    .Enrich.WithProperty("environment", builder.Environment.EnvironmentName)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();

// Add HttpClient for outgoing calls to ServiceB
builder.Services.AddHttpClient();

// Configure OpenTelemetry Logging
builder.Logging.AddOpenTelemetry(options =>
{
    options.SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "ServiceB"));
    options.AddOtlpExporter(opt =>
    {
        opt.Endpoint = new Uri(Environment.GetEnvironmentVariable("TEMPO_URL") ?? "http://tempo:4317");
    });
});

// Add OpenTelemetry

// Configure OpenTelemetry Tracing & Metrics
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "ServiceB"))
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

app.Run();
