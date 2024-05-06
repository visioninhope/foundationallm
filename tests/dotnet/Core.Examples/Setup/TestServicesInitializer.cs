using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.AzureAI;
using FoundationaLLM.Common.Models.Configuration.CosmosDB;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.Core.Examples.Models;
using FoundationaLLM.Core.Examples.Services;
using FoundationaLLM.Core.Interfaces;
using FoundationaLLM.Core.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.Core.Examples.Setup
{
    public static class TestServicesInitializer
	{
		/// <summary>
		/// Configure base services and dependencies for the tests.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configRoot"></param>
		public static void InitializeServices(
			IServiceCollection services,
			IConfigurationRoot configRoot)
		{
			TestConfiguration.Initialize(configRoot, services);

			RegisterInstance(services, configRoot);
			RegisterHttpClients(services, configRoot);
			RegisterCosmosDb(services, configRoot);
            RegisterAzureAIService(services, configRoot);
            RegisterLogging(services);
			RegisterServiceManagers(services);
		}

        private static void RegisterInstance(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<InstanceSettings>()
                .Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_Instance));
        }
        
        private static void RegisterHttpClients(IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<HttpClientOptions>(HttpClients.CoreAPI, options =>
            {
                options.BaseUri = configuration[AppConfigurationKeys.FoundationaLLM_APIs_CoreAPI_APIUrl]!;
                options.Scope = configuration[AppConfigurationKeys.FoundationaLLM_Chat_Entra_Scopes]!;
                options.Timeout = TimeSpan.FromSeconds(120);
            });
            services.Configure<HttpClientOptions>(HttpClients.ManagementAPI, options =>
            {
                options.BaseUri = configuration[AppConfigurationKeys.FoundationaLLM_APIs_ManagementAPI_APIUrl]!;
                options.Scope = configuration[AppConfigurationKeys.FoundationaLLM_Management_Entra_Scopes]!;
                options.Timeout = TimeSpan.FromSeconds(120);
            });

            services.AddHttpClient(HttpClients.CoreAPI)
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptionsSnapshot<HttpClientOptions>>().Get(HttpClients.CoreAPI);
                    client.BaseAddress = new Uri(options.BaseUri!);
                    if (options.Timeout != null) client.Timeout = (TimeSpan)options.Timeout;
                })
                .AddResilienceHandler("DownstreamPipeline", static strategyBuilder =>
                {
                    CommonHttpRetryStrategyOptions.GetCommonHttpRetryStrategyOptions();
                });

            services.AddHttpClient(HttpClients.ManagementAPI)
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptionsSnapshot<HttpClientOptions>>().Get(HttpClients.ManagementAPI);
                    client.BaseAddress = new Uri(options.BaseUri!);
                    if (options.Timeout != null) client.Timeout = (TimeSpan)options.Timeout;
                })
                .AddResilienceHandler("DownstreamPipeline", static strategyBuilder =>
                {
                    CommonHttpRetryStrategyOptions.GetCommonHttpRetryStrategyOptions();
                });
        }

		private static void RegisterCosmosDb(IServiceCollection services, IConfiguration configuration)
		{
			services.AddOptions<CosmosDbSettings>()
				.Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_CosmosDB));

			services.AddSingleton<CosmosClient>(serviceProvider =>
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

			services.AddScoped<ICosmosDbService, CosmosDbService>();
		}

		private static void RegisterAzureAIService(IServiceCollection services, IConfiguration configuration)
		{
			services.AddOptions<AzureAISettings>()
				.Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_AzureAIStudio));
			services.AddOptions<BlobStorageServiceSettings>()
				.Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_AzureAIStudio_BlobStorageServiceSettings));

			services.AddScoped<IAzureAIService, AzureAIService>();
			services.AddSingleton<IStorageService, BlobStorageService>();
		}

		private static void RegisterLogging(IServiceCollection services)
		{
			services.AddLogging(builder =>
			{
				builder.AddConsole();
			});
		}

        private static void RegisterServiceManagers(IServiceCollection services)
        {
            services.AddScoped<ICoreAPITestManager, CoreAPITestManager>();
			services.AddScoped<IManagementAPITestManager, ManagementAPITestManager>();
            services.AddScoped<IHttpClientManager, HttpClientManager>();
			services.AddScoped<IAgentConversationTestService, AgentConversationTestService>();
        }
    }
}
