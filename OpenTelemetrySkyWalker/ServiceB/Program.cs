using SkyApm.Utilities.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddSkyApmExtensions();

var app = builder.Build();

app.MapControllers();

app.MapGet("/test-logs", (ILogger<Program> logger) =>
{
    logger.LogInformation("This is a test info log from ServiceA via SkyAPM - {Timestamp}", DateTime.UtcNow);
    logger.LogWarning("This is a test warning log from ServiceA via SkyAPM");
    logger.LogError("This is a test error log from ServiceA via SkyAPM");

    return "Test logs generated via SkyAPM!";
});

app.Run();