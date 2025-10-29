using Microsoft.Extensions.DependencyInjection;
using SkyApm.Diagnostics.HttpClient;
using SkyApm.Diagnostics.MSLogging;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add explicit SkyAPM configuration loading
var skyApmConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "skyapm.json");
Console.WriteLine($"=== SkyAPM Configuration ===");
Console.WriteLine($"Config file: {skyApmConfigPath}");
Console.WriteLine($"Config exists: {File.Exists(skyApmConfigPath)}");

if (File.Exists(skyApmConfigPath))
{
    try
    {
        var configContent = File.ReadAllText(skyApmConfigPath);
        Console.WriteLine($"Config content: {configContent}");

        // Validate JSON
        var config = System.Text.Json.JsonSerializer.Deserialize<dynamic>(configContent);
        Console.WriteLine(" skyapm.json is valid JSON");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ skyapm.json parsing failed: {ex.Message}");
    }
}

Console.WriteLine($"SKYWALKING__ENABLED: {Environment.GetEnvironmentVariable("SKYWALKING__ENABLED")}");

// CORRECTED: Add SkyAPM properly
try
{
    builder.Services.AddSkyAPM(); // This is the key line that was missing!
    Console.WriteLine("✅ SkyAPM services registered successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Failed to register SkyAPM services: {ex.Message}");
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapControllers();

// Simple diagnostics endpoint
app.MapGet("/skyapm-status", (IConfiguration configuration, ILogger<Program> logger) =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
    var skyApmAssemblies = assemblies
        .Where(a => a.FullName?.Contains("SkyAPM", StringComparison.OrdinalIgnoreCase) == true)
        .ToList();

    // Check if SkyAPM configuration is loaded
    var skyWalkingSection = configuration.GetSection("SkyWalking");
    var skyApmConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "skyapm.json");
    var skyApmConfigExists = File.Exists(skyApmConfigPath);

    string configContent = null;
    if (skyApmConfigExists)
    {
        configContent = File.ReadAllText(skyApmConfigPath);
    }

    var results = new List<string>();
    var currentDir = Directory.GetCurrentDirectory();

    results.Add($"Searching in: {currentDir}");

    // Look for all DLL files that might contain AgentConfig
    var dllFiles = assemblies
        .Where(f => f.FullName?.Contains("SkyAPM", StringComparison.OrdinalIgnoreCase) == true ||
                   f.FullName?.Contains("SkyApm", StringComparison.OrdinalIgnoreCase) == true ||
                   f.FullName?.Contains("Agent", StringComparison.OrdinalIgnoreCase) == true)
        .ToList();

    results.Add($"Found {dllFiles.Count} relevant DLL files");

    foreach (var assembly in dllFiles)
    {
        try
        {

            var types = assembly.GetTypes();
            var hasAgentConfig = types.Any(t => t.Name == "AgentConfig");

            if (hasAgentConfig)
            {
                results.Add($"   Full path: {assembly.Location}");
                results.Add($"   Assembly: {assembly.FullName}");
            }
        }
        catch (Exception ex)
        {
            // Skip files that can't be loaded
            continue;
        }
    }

    return new
    {
        SkyAPMAssembliesLoaded = skyApmAssemblies.Count,
        LoadedAssemblies = skyApmAssemblies.Select(a => a.GetName().Name).ToArray(),
        ServiceName = skyWalkingSection["ServiceName"] ?? "NotConfigured",
        OAP_Server = skyWalkingSection["Transport:gRPC:Servers"] ?? "oap:11800",
        SkyAPMConfigExists = skyApmConfigExists,
        SkyAPMConfigPath = skyApmConfigPath,
        ConfigServiceName = System.Text.Json.JsonSerializer.Deserialize<dynamic>(configContent ?? "{}")?
            .GetProperty("SkyWalking")?.GetProperty("ServiceName").GetString() ?? "Not Found",
        Results = results,
        EnvironmentVariables = new
        {
            SkyWalkingEnabled = Environment.GetEnvironmentVariable("SKYWALKING__ENABLED"),
            ServiceName = Environment.GetEnvironmentVariable("SKYWALKING__SERVICENAME")
        },
        Timestamp = DateTime.UtcNow
    };
});

// Detailed SkyAPM configuration diagnostics
app.MapGet("/skyapm-detailed-status", (IConfiguration configuration) =>
{
    var skyWalkingSection = configuration.GetSection("SkyWalking");

    // Check multiple ways the agent might be configured
    var allConfigKeys = skyWalkingSection.GetChildren()
        .Select(c => c.Key)
        .ToList();

    return new
    {
        // Configuration existence
        SkyWalkingSectionExists = skyWalkingSection.Exists(),
        HasEnabledProperty = !string.IsNullOrEmpty(skyWalkingSection["Enabled"]),
        EnabledValue = skyWalkingSection["Enabled"],
        ServiceNameValue = skyWalkingSection["ServiceName"],

        // Transport configuration
        TransportServers = skyWalkingSection["Transport:gRPC:Servers"],
        TransportProtocol = skyWalkingSection["Transport:ProtocolVersion"],

        // All configuration keys
        ConfigKeys = allConfigKeys,

        // File system check
        SkyApmJsonExists = File.Exists("skyapm.json"),
        CurrentDirectory = Directory.GetCurrentDirectory(),
        FilesInCurrentDir = Directory.GetFiles(Directory.GetCurrentDirectory())
            .Where(f => f.EndsWith(".json"))
            .ToList(),
        Enable = skyWalkingSection["Enable"] ?? "Not set (NEW KEY)",

        // Check all configuration values
        AllConfigValues = skyWalkingSection.GetChildren()
            .ToDictionary(x => x.Key, x => x.Value ?? "null"),

        // File check
        ConfigFileExists = File.Exists("skyapm.json"),
        ConfigFileContent = File.Exists("skyapm.json") ?
            File.ReadAllText("skyapm.json") : "File not found",
        // Environment variables that affect SkyAPM
        EnvVars = new
        {
            ASPNETCORE_SKYWALKING_ENABLED = Environment.GetEnvironmentVariable("ASPNETCORE_SKYWALKING_ENABLED"),
            SKYWALKING_ENABLED = Environment.GetEnvironmentVariable("SKYWALKING_ENABLED"),
            SKYAPM_ENABLED = Environment.GetEnvironmentVariable("SKYAPM_ENABLED"),
            SKYAPM_SERVICENAME = Environment.GetEnvironmentVariable("SKYAPM_SERVICENAME")
        },

        Timestamp = DateTime.UtcNow
    };
});

// Agent runtime diagnostics
app.MapGet("/skyapm-runtime-check", () =>
{
    var agentChecks = new List<string>();

    // Check 1: AgentConfig type
    var agentConfigType = Type.GetType("SkyApm.AgentConfig, SkyAPM.Agent.AspNetCore");
    agentChecks.Add($"AgentConfig Type: {(agentConfigType != null ? "Found" : "NOT FOUND")}");

    if (agentConfigType != null)
    {
        // Check 2: Instance property
        var instanceProp = agentConfigType.GetProperty("Instance",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        var instanceValue = instanceProp?.GetValue(null);
        agentChecks.Add($"Agent Instance: {(instanceValue != null ? "Exists" : "NULL")}");

        // Check 3: RuntimeContext
        var runtimeContextProp = agentConfigType.GetProperty("RuntimeContext",
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        var runtimeContextValue = runtimeContextProp?.GetValue(null);
        agentChecks.Add($"Runtime Context: {(runtimeContextValue != null ? "Exists" : "NULL")}");

        // Check 4: Try to get service name from agent
        if (instanceValue != null)
        {
            var serviceNameProp = instanceValue.GetType().GetProperty("ServiceName");
            if (serviceNameProp != null)
            {
                var serviceName = serviceNameProp.GetValue(instanceValue);
                agentChecks.Add($"Agent ServiceName: {serviceName}");
            }
        }
    }

    // Check 5: Look for any SkyAPM services in DI
    var serviceTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t => t.FullName?.Contains("SkyAPM") == true || t.FullName?.Contains("SkyApm") == true)
        .Select(t => t.FullName)
        .ToList();

    return new
    {
        AgentChecks = agentChecks,
        LoadedSkyAPMTypes = serviceTypes,
        TotalSkyAPMAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Count(a => a.FullName?.Contains("SkyAPM") == true || a.FullName?.Contains("SkyApm") == true),
        Timestamp = DateTime.UtcNow
    };
});

app.MapGet("/test-oap-all-endpoints", async () =>
{
    var results = new List<object>();
    var endpoints = new[]
    {
        "http://oap:11800",
        "http://oap:12800",
        "http://localhost:11800",
        "http://localhost:12800"
    };

    using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };

    foreach (var endpoint in endpoints)
    {
        try
        {
            var response = await httpClient.GetAsync(endpoint);
            results.Add(new
            {
                Endpoint = endpoint,
                Status = "Reachable",
                StatusCode = (int)response.StatusCode
            });
        }
        catch (Exception ex)
        {
            results.Add(new
            {
                Endpoint = endpoint,
                Status = "Unreachable",
                Error = ex.GetType().Name,
                Message = ex.Message
            });
        }
        await Task.Delay(500); // Small delay between tests
    }

    return results;
});

app.MapGet("/test-oap-grpc-connectivity", async (HttpContext context) =>
{
    try
    {
        using var tcpClient = new System.Net.Sockets.TcpClient();
        await tcpClient.ConnectAsync("oap", 11800);
        await context.Response.WriteAsJsonAsync(new
        {
            Status = "✅ SUCCESS: OAP gRPC port 11800 is reachable from ServiceA",
            LocalEndpoint = tcpClient.Client.LocalEndPoint?.ToString(),
            RemoteEndpoint = tcpClient.Client.RemoteEndPoint?.ToString()
        });
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsJsonAsync(new
        {
            Status = "❌ FAILED: Cannot connect to OAP gRPC port 11800",
            Error = ex.Message,
            Details = "The gRPC server is running but ServiceA cannot connect"
        });
    }
});

app.MapGet("/skyapm-internal-logs", () =>
{
    var logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
    var logFiles = new List<string>();

    if (Directory.Exists(logDir))
    {
        logFiles.AddRange(Directory.GetFiles(logDir, "skyapm-*.log")
            .OrderByDescending(f => f)
            .Select(f => Path.GetFileName(f)));
    }

    return new
    {
        LogDirectory = logDir,
        LogDirectoryExists = Directory.Exists(logDir),
        SkyAPMLogFiles = logFiles,
        CurrentDirectory = Directory.GetCurrentDirectory(),
        AllFilesInRoot = Directory.GetFiles(Directory.GetCurrentDirectory()).Select(Path.GetFileName)
    };
});

// Test endpoint for logs
app.MapGet("/test-logs", (ILogger<Program> logger) =>
{
    logger.LogInformation("Test info log - {Timestamp}", DateTime.UtcNow);
    logger.LogWarning("Test warning log");
    logger.LogError("Test error log");
    return "Basic logs test completed";
});

app.MapGet("/test-all-log-patterns", (ILogger<Program> logger, ILoggerFactory loggerFactory) =>
{
    var results = new List<string>();

    // Pattern 1: Basic logger
    logger.LogInformation("Pattern 1: Basic information log");
    results.Add("Basic log");

    // Pattern 2: Named logger
    var namedLogger = loggerFactory.CreateLogger("TestCategory");
    namedLogger.LogWarning("Pattern 2: Named logger warning");
    results.Add("Named logger");

    // Pattern 3: Logger with category
    var categoryLogger = loggerFactory.CreateLogger(typeof(Program));
    categoryLogger.LogError("Pattern 3: Category logger error");
    results.Add("Category logger");

    // Pattern 4: Scoped logger with properties
    using (var scope = logger.BeginScope(new Dictionary<string, object>
    {
        ["OperationId"] = Guid.NewGuid(),
        ["Service"] = "ServiceA"
    }))
    {
        logger.LogInformation("Pattern 4: Scoped log with properties");
        results.Add("Scoped with properties");
    }

    // Pattern 5: Simple scope
    using (logger.BeginScope("SimpleScope"))
    {
        logger.LogInformation("Pattern 5: Simple scoped log");
        results.Add("Simple scope");
    }

    return new
    {
        PatternsTested = results,
        Message = "Various logging patterns tested - check which ones work"
    };
});

app.MapGet("/skyapm-real-services", (IServiceProvider serviceProvider) =>
{
    var services = new Dictionary<string, object>();

    // Get the actual service collection from the built provider
    try
    {
        // Use reflection to access the internal service collection
        var field = serviceProvider.GetType().GetField("_callSiteFactory",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (field != null)
        {
            var callSiteFactory = field.GetValue(serviceProvider);
            var descriptorsField = callSiteFactory.GetType().GetField("_descriptors",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (descriptorsField != null)
            {
                var descriptors = descriptorsField.GetValue(callSiteFactory) as Dictionary<Type, object>;
                if (descriptors != null)
                {
                    var skyApmServices = descriptors.Keys
                        .Where(type => type.FullName?.Contains("SkyAPM") == true ||
                                     type.FullName?.Contains("SkyApm") == true)
                        .Select(type => type.FullName)
                        .ToList();

                    services["SkyAPM_Service_Types"] = skyApmServices;
                    services["SkyAPM_Services_Count"] = skyApmServices.Count;
                }
            }
        }
    }
    catch (Exception ex)
    {
        services["Reflection_Error"] = ex.Message;
    }

    // Alternative: Try to resolve key services
    var keyServices = new Dictionary<string, object>();

    var serviceTypes = new[]
    {
        "SkyApm.Tracing.ITracingContext, SkyAPM.Core",
        "SkyApm.Transport.ISegmentReporter, SkyAPM.Core",
        "SkyApm.Transport.ISegmentDispatcher, SkyAPM.Core"
    };

    foreach (var typeName in serviceTypes)
    {
        try
        {
            var serviceType = Type.GetType(typeName);
            if (serviceType != null)
            {
                var service = serviceProvider.GetService(serviceType);
                keyServices[typeName] = service != null ? "✅ Resolved" : "❌ Not found";
            }
            else
            {
                keyServices[typeName] = "❌ Type not loaded";
            }
        }
        catch (Exception ex)
        {
            keyServices[typeName] = $"❌ Error: {ex.Message}";
        }
    }

    services["Key_Services_Resolution"] = keyServices;

    return services;
});

// Root endpoint
app.MapGet("/", () => "ServiceA is running with SkyAPM!");

app.Run();