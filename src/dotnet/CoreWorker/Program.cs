using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Core.Interfaces;
using FoundationaLLM.Core.Models.Configuration;
using FoundationaLLM.Core.Services;
using FoundationaLLM.Core.Worker;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(builder.Configuration[AppConfigurationKeys.FoundationaLLM_AppConfig_ConnectionString]);

    options.ConfigureKeyVault(options =>
    {
        options.SetCredential(new DefaultAzureCredential());
    });
    options.Select(AppConfigurationKeyFilters.FoundationaLLM_CoreWorker);
    options.Select(AppConfigurationKeyFilters.FoundationaLLM_CosmosDB);
});
if (builder.Environment.IsDevelopment())
    builder.Configuration.AddJsonFile("appsettings.development.json", true, true);

builder.Services.AddOptions<CosmosDbSettings>()
    .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_CosmosDB));
builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>();
builder.Services.AddSingleton<ICosmosDbChangeFeedService, CosmosDbChangeFeedService>();
builder.Services.AddHostedService<ChangeFeedWorker>();

// Add the OpenTelemetry telemetry service and send telemetry data to Azure Monitor.
builder.Services.AddOpenTelemetry().UseAzureMonitor(options =>
{
    options.ConnectionString = builder.Configuration[AppConfigurationKeys.FoundationaLLM_CoreWorker_AppInsightsConnectionString];
});

// Create a dictionary of resource attributes.
var resourceAttributes = new Dictionary<string, object> {
    { "service.name", "CoreWorker" },
    { "service.namespace", "FoundationaLLM" },
    { "service.instance.id", Guid.NewGuid().ToString() }
};

// Configure the OpenTelemetry tracer provider to add the resource attributes to all traces.
builder.Services.ConfigureOpenTelemetryTracerProvider((sp, builder) =>
    builder.ConfigureResource(resourceBuilder =>
        resourceBuilder.AddAttributes(resourceAttributes)));

var host = builder.Build();

host.Run();
