using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Models.Configuration.CosmosDB;
using FoundationaLLM.Core.Interfaces;
using FoundationaLLM.Core.Services;
using FoundationaLLM.Core.Worker;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

DefaultAuthentication.Initialize(
    builder.Environment.IsProduction(),
    ServiceNames.CoreWorker);

builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(builder.Configuration[EnvironmentVariables.FoundationaLLM_AppConfig_ConnectionString]);

    options.ConfigureKeyVault(options =>
    {
        options.SetCredential(DefaultAuthentication.AzureCredential);
    });
    options.Select(AppConfigurationKeyFilters.FoundationaLLM_CoreWorker);
    options.Select(AppConfigurationKeyFilters.FoundationaLLM_CosmosDB);
});
if (builder.Environment.IsDevelopment())
    builder.Configuration.AddJsonFile("appsettings.development.json", true, true);

builder.Services.AddOptions<CosmosDbSettings>()
    .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_CosmosDB));

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
    return new CosmosClientBuilder(settings.Endpoint, DefaultAuthentication.AzureCredential)
        .WithSerializerOptions(new CosmosSerializationOptions
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        })
        .WithConnectionModeGateway()
        .Build();
});

builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>();
builder.Services.AddSingleton<ICosmosDbChangeFeedService, CosmosDbChangeFeedService>();
builder.Services.AddHostedService<ChangeFeedWorker>();
builder.Services.AddApplicationInsightsTelemetryWorkerService(options =>
{
    options.ConnectionString = builder.Configuration[AppConfigurationKeys.FoundationaLLM_APIEndpoints_CoreWorker_AppInsightsConnectionString];
});

var host = builder.Build();

host.Run();
