using Asp.Versioning;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.Exporter;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Middleware;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Configuration;
using FoundationaLLM.Common.OpenAPI;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Gatekeeper.Core.Interfaces;
using FoundationaLLM.Gatekeeper.Core.Models.ConfigurationOptions;
using FoundationaLLM.Gatekeeper.Core.Services;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FoundationaLLM.Gatekeeper.API
{
    /// <summary>
    /// Program class for the Gatekeeper API.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point for Gatekeeper API.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddOpenTelemetry(logging =>
                {
                    logging.AddConsoleExporter();
                });
            });

            builder.Configuration.Sources.Clear();
            builder.Configuration.AddJsonFile("appsettings.json", false, true);
            builder.Configuration.AddEnvironmentVariables();
            builder.Configuration.AddAzureAppConfiguration(options =>
            {
                options.Connect(builder.Configuration["FoundationaLLM:AppConfig:ConnectionString"]);
                options.ConfigureKeyVault(options =>
                {
                    options.SetCredential(new DefaultAzureCredential());
                });
            });
            if (builder.Environment.IsDevelopment())
                builder.Configuration.AddJsonFile("appsettings.development.json", true, true);

            // Add services to the container.
            builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions
            {
                ConnectionString = builder.Configuration["FoundationaLLM:APIs:GatekeeperAPI:AppInsightsConnectionString"],
                DeveloperMode = builder.Environment.IsDevelopment()
            });
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = Common.Settings.CommonJsonSerializerSettings.GetJsonSerializerSettings().ContractResolver;
            });

            // Add API Key Authorization
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<APIKeyAuthenticationFilter>();
            builder.Services.AddOptions<APIKeyValidationSettings>()
                .Bind(builder.Configuration.GetSection("FoundationaLLM:APIs:GatekeeperAPI"));
            builder.Services.AddOptions<KeyVaultConfigurationServiceSettings>()
                .Bind(builder.Configuration.GetSection("FoundationaLLM:Configuration"));

            // Register the downstream services and HTTP clients.
            RegisterDownstreamServices(builder);

            builder.Services.AddTransient<IAPIKeyValidationService, APIKeyValidationService>();

            builder.Services.AddOptions<RefinementServiceSettings>()
                .Bind(builder.Configuration.GetSection("FoundationaLLM:Refinement"));
            builder.Services.AddScoped<IRefinementService, RefinementService>();

            builder.Services.AddOptions<AzureContentSafetySettings>()
                .Bind(builder.Configuration.GetSection("FoundationaLLM:AzureContentSafety"));
            builder.Services.AddScoped<IContentSafetyService, AzureContentSafetyService>();

            builder.Services.AddScoped<IAgentFactoryAPIService, AgentFactoryAPIService>();

            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddScoped<IUserIdentityContext, UserIdentityContext>();
            builder.Services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();
            builder.Services.AddScoped<IUserClaimsProviderService, NoOpUserClaimsProviderService>();
            builder.Services.AddScoped<IGatekeeperService, GatekeeperService>();

            builder.Services
                .AddApiVersioning(options =>
                {
                    // Reporting api versions will return the headers
                    // "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                })
                .AddMvc()
                .AddApiExplorer(options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";
                });

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(
                options =>
                {
                    // Add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();

                    var fileName = typeof(Program).Assembly.GetName().Name + ".xml";
                    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

                    // Integrate xml comments
                    options.IncludeXmlComments(filePath);

                    // Adds auth via X-API-KEY header
                    options.AddAPIKeyAuth();
                });

            builder.Services.AddOpenTelemetry().WithTracing(b =>
            {
                b
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter()
                .AddJaegerExporter()
                .AddAzureMonitorTraceExporter(o => o.ConnectionString = builder.Configuration["FoundationaLLM:AppInsights:ConnectionString"])
                .AddSource("FoundationaLLM.GatekeeperAPI")
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("FoundationaLLM.GatekeeperAPI"));
            });

            /*
            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeScopes = true;

                logging
                .AddConsoleExporter()
                //.AddOtlpExporter()
                //.AddAzureMonitorLogExporter(o => o.ConnectionString = "InstrumentationKey=110912dc-f6eb-41c2-bc0b-2420492cc32e;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/")
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("FoundationaLLM.GatekeeperAPI"));
            });
            */

            /*
            // Setup Traces
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddAspNetCoreInstrumentation()
                .AddSource("FoundationaLLM.GatekeeperAPI")
                .AddConsoleExporter()
                //.AddAzureMonitorTraceExporter(o => o.ConnectionString = "InstrumentationKey=110912dc-f6eb-41c2-bc0b-2420492cc32e;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/")
                .Build();
            */

            var app = builder.Build();

            // Register the middleware to set the user identity context.
            app.UseMiddleware<UserIdentityMiddleware>();

            app.UseExceptionHandler(exceptionHandlerApp
                => exceptionHandlerApp.Run(async context
                    => await Results.Problem().ExecuteAsync(context)));

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    var descriptions = app.DescribeApiVersions();

                    // build a swagger endpoint for each discovered API version
                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// Bind the downstream API settings to the configuration and register the HTTP clients.
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterDownstreamServices(WebApplicationBuilder builder)
        {
            var downstreamAPISettings = new DownstreamAPISettings
            {
                DownstreamAPIs = new Dictionary<string, DownstreamAPIKeySettings>()
            };

            var agentFactoryAPISettings = new DownstreamAPIKeySettings
            {
                APIUrl = builder.Configuration[$"FoundationaLLM:APIs:{HttpClients.AgentFactoryAPI}:APIUrl"] ?? "",
                APIKey = builder.Configuration[$"FoundationaLLM:APIs:{HttpClients.AgentFactoryAPI}:APIKey"] ?? ""
            };

            downstreamAPISettings.DownstreamAPIs[HttpClients.AgentFactoryAPI] = agentFactoryAPISettings;

            builder.Services
                    .AddHttpClient(HttpClients.AgentFactoryAPI,
                        client => { client.BaseAddress = new Uri(agentFactoryAPISettings.APIUrl); })
                    .AddTransientHttpErrorPolicy(policyBuilder =>
                        policyBuilder.WaitAndRetryAsync(
                            3, retryNumber => TimeSpan.FromMilliseconds(600)));

            builder.Services.AddSingleton<IDownstreamAPISettings>(downstreamAPISettings);
        }
    }
}