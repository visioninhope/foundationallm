using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Runtime;
using FoundationaLLM.Common.Models.Configuration.AzureAI;
using FoundationaLLM.Common.Models.Configuration.CosmosDB;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Core.Interfaces;
using FoundationaLLM.Core.Services;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using FoundationaLLM.Core.Examples.Utils;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Services.Storage;
using Microsoft.Extensions.Options;
using FoundationaLLM.Common.Authentication;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos;

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

			services.Configure<AzureAISettings>(configRoot.GetSection(nameof(AzureAISettings)));

			RegisterHttpClients(services);
			RegisterCosmosDb(services);
			RegisterLogging(services);
			RegisterAzureAIService(services);
		}

		private static void RegisterHttpClients(IServiceCollection services)
		{
			var coreApiUri = TestConfiguration.GetAppConfigValueAsync(AppConfigurationKeys.FoundationaLLM_APIs_CoreAPI_APIUrl).GetAwaiter().GetResult();
			
			services
				.AddHttpClient(
					HttpClients.CoreAPI,
					client => { 
						client.BaseAddress = new Uri(coreApiUri);
						client.Timeout = TimeSpan.FromSeconds(120);
					})
				.AddResilienceHandler(
					"DownstreamPipeline",
					static strategyBuilder =>
					{
						CommonHttpRetryStrategyOptions.GetCommonHttpRetryStrategyOptions();
					});
		}

		private static void RegisterCosmosDb(IServiceCollection services)
		{
			var cosmosDbSettings = TestConfiguration.CosmosDbSettings;
			if (cosmosDbSettings == null)
			{
				throw new InvalidOperationException("CosmosDB settings not found. TestConfiguration must be initialized with a call to Initialize(IConfigurationRoot) before accessing configuration values.");
			}

			services.AddSingleton<CosmosClient>(serviceProvider => new CosmosClientBuilder(cosmosDbSettings.Endpoint, DefaultAuthentication.GetAzureCredential())
				.WithSerializerOptions(new CosmosSerializationOptions
				{
					PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
				})
				.WithConnectionModeGateway()
				.Build());

			services.AddScoped<ICosmosDbService, CosmosDbService>(sp => new CosmosDbService(
				Options.Create<CosmosDbSettings>(cosmosDbSettings),
				sp.GetRequiredService<CosmosClient>(),
				sp.GetRequiredService<ILogger<CosmosDbService>>()));
		}

		private static void RegisterAzureAIService(IServiceCollection services)
		{
			var azureAISettings = TestConfiguration.AzureAISettings;
			if (azureAISettings == null)
			{
				throw new InvalidOperationException("Azure AI settings not found. TestConfiguration must be initialized with a call to Initialize(IConfigurationRoot) before accessing configuration values.");
			}

			services.AddSingleton<IStorageService, BlobStorageService>(sp =>
			{
				var settings = sp.GetRequiredService<IOptionsMonitor<AzureAISettings>>();
				var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

				return new BlobStorageService(
					Options.Create<BlobStorageServiceSettings>(settings.CurrentValue.BlobStorageServiceSettings),
					logger)
				{
					InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Agent
				};
			});

			services.AddScoped<IAzureAIService, AzureAIService>();
		}

		private static void RegisterLogging(IServiceCollection services)
		{
			services.AddLogging(builder =>
			{
				builder.AddConsole();
			});
		}
	}

}
