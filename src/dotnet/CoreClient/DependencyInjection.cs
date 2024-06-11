using FoundationaLLM.Client.Core;
using FoundationaLLM.Client.Core.Interfaces;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Models.Configuration.API;
using FoundationaLLM.Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FoundationaLLM
{
    /// <summary>
    /// Core Client service dependency injection extensions.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Add the Core Client and its related dependencies to the dependency injection container.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void AddCoreClient(this IHostApplicationBuilder builder)
        {
            builder.Services.Configure<APIClientSettings>(HttpClients.CoreAPI, options =>
            {
                options.APIUrl = builder.Configuration[AppConfigurationKeys.FoundationaLLM_APIs_CoreAPI_APIUrl]!;
                options.Timeout = TimeSpan.FromSeconds(900);
            });

            builder.Services.AddHttpClient(HttpClients.CoreAPI)
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptionsSnapshot<APIClientSettings>>().Get(HttpClients.CoreAPI);
                    client.BaseAddress = new Uri(options.APIUrl!);
                    if (options.Timeout != null) client.Timeout = (TimeSpan)options.Timeout;
                })
                .AddResilienceHandler("DownstreamPipeline", static strategyBuilder =>
                {
                    CommonHttpRetryStrategyOptions.GetCommonHttpRetryStrategyOptions();
                });

            builder.Services.AddSingleton<ICoreRESTClient, CoreRESTClient>();
        }
    }
}
