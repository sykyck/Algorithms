using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ServiceB.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add HttpClient for outgoing calls to ServiceB
builder.Services.AddHttpClient();

// Create a ResourceBuilder
var resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(
        serviceName: Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? "ServiceB",
        serviceVersion: "1.0.0"
    );

// Add OpenTelemetry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
        resource.AddService(
            serviceName: Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? "ServiceB",
            serviceVersion: "1.0.0",
            serviceInstanceId: Environment.MachineName
        )
    )
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource(Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? "ServiceB")                 // <-- Add your custom ActivitySource
            .AddOtlpExporter(o =>
            {
                // Point to your collector (use service name in Docker)
                o.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://otel-collector:4317");
                o.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            });
    });

// Add OpenTelemetry Logging
builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(loggingBuilder =>
{
    loggingBuilder.SetResourceBuilder(resourceBuilder)
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri(Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://otel-collector:4317");
            opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        });
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
