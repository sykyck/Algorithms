// Program.cs (for .NET 6/7)
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add OpenTelemetry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
        resource.AddService("ServiceA") // or ServiceB
    )
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(o =>
            {
                // Point to your collector (use service name in Docker)
                o.Endpoint = new Uri("http://otel-collector:4317");
            });
    });

var app = builder.Build();

app.MapControllers();

app.Run();
