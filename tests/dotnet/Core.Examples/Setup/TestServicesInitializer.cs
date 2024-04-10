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
using FoundationaLLM.Common.Models.Configuration.CosmosDB;
using FoundationaLLM.Core.Interfaces;
using FoundationaLLM.Core.Services;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using FoundationaLLM.Core.Examples.Utils;

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

			RegisterHttpClients(services);
			RegisterCosmosDb(services);
			RegisterLogging(services);
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

			services.Configure<CosmosDbSettings>(options =>
			{
				options.Endpoint = cosmosDbSettings.Endpoint;
				options.Database = cosmosDbSettings.Database;
				options.Containers = cosmosDbSettings.Containers;
				options.MonitoredContainers = cosmosDbSettings.MonitoredContainers;
				options.ChangeFeedLeaseContainer = cosmosDbSettings.ChangeFeedLeaseContainer;
				options.EnableTracing = cosmosDbSettings.EnableTracing;
			});

			services.AddScoped<ICosmosDbService, CosmosDbService>();
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
