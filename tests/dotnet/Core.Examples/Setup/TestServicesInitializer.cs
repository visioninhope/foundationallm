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

namespace FoundationaLLM.Core.Examples.Setup
{
	public static class TestServicesInitializer
	{
		/// <summary>
		/// Configure base services and dependencies for the tests.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configRoot"></param>
		public static void InitializeServices(IServiceCollection services, IConfigurationRoot configRoot)
		{
			TestConfiguration.Initialize(configRoot, services);

			RegisterHttpClients(services);
		}

		private static void RegisterHttpClients(IServiceCollection services)
		{
			var coreApiUri = TestConfiguration.GetAppConfigValueAsync(AppConfigurationKeys.FoundationaLLM_APIs_CoreAPI_APIUrl).GetAwaiter().GetResult();
			services
				.AddHttpClient(HttpClients.CoreAPI,
					client => { client.BaseAddress = new Uri(coreApiUri); })
				.AddResilienceHandler(
					"DownstreamPipeline",
					static strategyBuilder =>
					{
						CommonHttpRetryStrategyOptions.GetCommonHttpRetryStrategyOptions();
					});
		}
	}

}
