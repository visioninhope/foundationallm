using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Gateway.Exceptions;
using FoundationaLLM.Gateway.Interfaces;
using FoundationaLLM.Gateway.Models.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoundationaLLM.Gateway.Services
{
    /// <summary>
    /// General purpose dependency injection extensions.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Add the core Gateway service the the dependency injection container.
        /// </summary>
        /// <param name="builder">The host application builder.</param>
        public static void AddGatewayService(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<GatewayServiceSettings>()
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_Gateway));

            builder.Services.AddSingleton<IGatewayService, GatewayService>();
            builder.Services.AddHostedService<GatewayWorker>();
        }
    }
}
