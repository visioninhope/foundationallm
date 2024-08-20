using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Gateway.Client;
using FoundationaLLM.Gateway.Interfaces;
using FoundationaLLM.Gateway.Models.Configuration;
using FoundationaLLM.Gateway.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FoundationaLLM
{
    /// <summary>
    /// General purpose dependency injection extensions.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Adds the core Gateway service the the dependency injection container.
        /// </summary>
        /// <param name="builder">The host application builder.</param>
        public static void AddGatewayCore(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<GatewayCoreSettings>()
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIEndpoints_GatewayAPI_Configuration));

            builder.Services.AddSingleton<IGatewayCore, GatewayCore>();
            builder.Services.AddHostedService<GatewayWorker>();
        }

        /// <summary>
        /// Adds the core Gateway service the the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> dependency injection container service collection.</param>
        /// <param name="configuration">The <see cref="IConfigurationManager"/> configuration manager.</param>
        public static void AddGatewayCore(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddOptions<GatewayCoreSettings>()
                .Bind(configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIEndpoints_GatewayAPI_Configuration));

            services.AddSingleton<IGatewayCore, GatewayCore>();
            services.AddHostedService<GatewayWorker>();
        }

        /// <summary>
        /// Adds the Gateway service client to the dependency injection container.
        /// </summary>
        /// <param name="builder">The host application builder.</param>
        public static void AddGatewayServiceClient(this IHostApplicationBuilder builder) =>
            builder.Services.AddSingleton<IGatewayServiceClient, GatewayServiceClient>();

        /// <summary>
        /// Adds the Gateway service to the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection of the DI container.</param>
        public static void AddGatewayServiceClient(this IServiceCollection services) =>
            services.AddSingleton<IGatewayServiceClient, GatewayCoreServiceClient>();
    }
}
