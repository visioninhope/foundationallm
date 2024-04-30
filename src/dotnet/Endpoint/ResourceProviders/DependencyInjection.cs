using FluentValidation;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Models.ResourceProviders.Endpoint;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Endpoint.ResourceProviders;
using FoundationaLLM.Endpoint.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM
{
    public static partial class DependencyInjection
    {
        public static void AddEndpointResourceProvider(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<BlobStorageServiceSettings>(
                DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Endpoint)
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_Endpoint_ResourceProviderService_Storage));

            builder.Services.AddSingleton<IStorageService, BlobStorageService>(sp =>
            {
                var settings = sp.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Endpoint);
                var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

                return new BlobStorageService(
                    Options.Create<BlobStorageServiceSettings>(settings),
                    logger)
                {
                    InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Endpoint
                };
            });

            // Register validators.
            builder.Services.AddSingleton<IValidator<EndpointBase>, EndpointBaseValidator>();
            builder.Services.AddSingleton<IValidator<AzureAIEndpoint>, AzureAIEndpointValidator>();
            builder.Services.AddSingleton<IValidator<AzureOpenAIEndpoint>, AzureOpenAIEndpointValidator>();
            builder.Services.AddSingleton<IValidator<OpenAIEndpoint>, OpenAIEndpointValidator>();

            builder.Services.AddSingleton<IResourceProviderService, EndpointResourceProviderService>(sp =>
                new EndpointResourceProviderService(
                    sp.GetRequiredService<IOptions<InstanceSettings>>(),
                    sp.GetRequiredService<IAuthorizationService>(),
                    sp.GetRequiredService<IEnumerable<IStorageService>>()
                        .Single(s => s.InstanceName == DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Endpoint),
                    sp.GetRequiredService<IEventService>(),
                    sp.GetRequiredService<IResourceValidatorFactory>(),
                    sp,
                    sp.GetRequiredService<ILoggerFactory>()));
            builder.Services.ActivateSingleton<IResourceProviderService>();
        }
    }
}
