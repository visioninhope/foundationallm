using FoundationaLLM.AzureOpenAI.ResourceProviders;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM
{
    /// <summary>
    /// Provides extension methods used to configure dependency injection.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Register the handler as a hosted service, passing the step name to the handler ctor
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <remarks>
        /// Requires an <see cref="IGatewayServiceClient"/> service to be also registered with the dependency injection container.
        /// </remarks>
        public static void AddAzureOpenAIResourceProvider(this IHostApplicationBuilder builder)
        {
            builder.AddAzureOpenAIResourceProviderStorage();

            builder.Services.AddSingleton<IResourceProviderService, AzureOpenAIResourceProviderService>(sp =>
                new AzureOpenAIResourceProviderService(
                    sp.GetRequiredService<IOptions<InstanceSettings>>(),
                    sp.GetRequiredService<IAuthorizationService>(),
                    sp.GetRequiredService<IEnumerable<IStorageService>>()
                        .Single(s => s.InstanceName == DependencyInjectionKeys.FoundationaLLM_ResourceProviders_AzureOpenAI),
                    sp.GetRequiredService<IEventService>(),
                    sp.GetRequiredService<IResourceValidatorFactory>(),
                    sp,
                    sp.GetRequiredService<ILogger<AzureOpenAIResourceProviderService>>()));

            builder.Services.ActivateSingleton<IResourceProviderService>();
        }
    }
}
