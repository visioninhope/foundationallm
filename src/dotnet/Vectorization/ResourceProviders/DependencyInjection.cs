using FluentValidation;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Models.Context;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Vectorization.Client;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using FoundationaLLM.Vectorization.Models.Resources;
using FoundationaLLM.Vectorization.ResourceProviders;
using FoundationaLLM.Vectorization.Validation.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace FoundationaLLM
{
    /// <summary>
    /// Provides extension methods used to configure dependency injection.
    /// </summary>
    public static partial class DependencyInjection
    {
        /// <summary>
        /// Register the handler as a hosted service, passing the step name to the handler ctor.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        public static void AddVectorizationResourceProvider(this IHostApplicationBuilder builder)
        {
            builder.Services.AddOptions<BlobStorageServiceSettings>(
                DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization)
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_Vectorization_ResourceProviderService_Storage));

            builder.Services.AddSingleton<IStorageService, BlobStorageService>(sp =>
            {
                var settings = sp.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization);
                var logger = sp.GetRequiredService<ILogger<BlobStorageService>>();

                return new BlobStorageService(
                    Options.Create<BlobStorageServiceSettings>(settings),
                    logger)
                {
                    InstanceName = DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization
                };
            });

            // Register validators.
            builder.Services.AddSingleton<IValidator<TextPartitioningProfile>, TextPartitioningProfileValidator>();
            builder.Services.AddSingleton<IValidator<TextEmbeddingProfile>, TextEmbeddingProfileValidator>();
            builder.Services.AddSingleton<IValidator<IndexingProfile>, IndexingProfileValidator>();
            builder.Services.AddSingleton<IValidator<VectorizationPipeline>, VectorizationPipelineValidator>();
            builder.Services.AddSingleton<IValidator<VectorizationRequest>, VectorizationRequestValidator>();

            // Register the Vectorization API client.
            RegisterDownstreamServices(builder);
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ICallContext, CallContext>();
            builder.Services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();
            builder.Services.AddOptions<VectorizationServiceSettings>()
                .Bind(builder.Configuration.GetSection(AppConfigurationKeySections.FoundationaLLM_APIs_VectorizationAPI));
            builder.Services.AddScoped<IVectorizationServiceClient, VectorizationServiceClient>();          


            // Register the resource provider services (cannot use Keyed singletons due to the Microsoft Identity package being incompatible):
            builder.Services.AddSingleton<IResourceProviderService, VectorizationResourceProviderService>(sp =>
            {
                using var scope = sp.CreateScope();
                var scopedProvider = scope.ServiceProvider;

                return new VectorizationResourceProviderService(
                    scopedProvider.GetRequiredService<IOptions<InstanceSettings>>(),
                    scopedProvider.GetRequiredService<IAuthorizationService>(),
                    scopedProvider.GetRequiredService<IEnumerable<IStorageService>>()
                        .Single(s => s.InstanceName == DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization),
                    scopedProvider.GetRequiredService<IEventService>(),
                    scopedProvider.GetRequiredService<IResourceValidatorFactory>(),
                    sp,
                    scopedProvider.GetRequiredService<IVectorizationServiceClient>(),
                    scopedProvider.GetRequiredService<ILogger<VectorizationResourceProviderService>>());
            });

            builder.Services.ActivateSingleton<IResourceProviderService>();
        }

        private static void RegisterDownstreamServices(IHostApplicationBuilder builder)
        {
            var downstreamAPISettings = new DownstreamAPISettings
            {
                DownstreamAPIs = []
            };

            var vectorizationAPISettings = new DownstreamAPIKeySettings
            {
                APIUrl = builder.Configuration[AppConfigurationKeys.FoundationaLLM_APIs_VectorizationAPI_APIUrl]!,
                APIKey = builder.Configuration[AppConfigurationKeys.FoundationaLLM_APIs_VectorizationAPI_APIKey]!
            };
            downstreamAPISettings.DownstreamAPIs[HttpClients.VectorizationAPI] = vectorizationAPISettings;
            var retryOptions = CommonHttpRetryStrategyOptions.GetCommonHttpRetryStrategyOptions();
            builder.Services
                .AddHttpClient(HttpClients.VectorizationAPI,
                    client => { client.BaseAddress = new Uri(vectorizationAPISettings.APIUrl); })
                .AddResilienceHandler(
                    "DownstreamPipeline",
                    strategyBuilder =>
                    {
                        strategyBuilder.AddRetry(retryOptions);
                    });
            builder.Services.AddSingleton<IDownstreamAPISettings>(downstreamAPISettings);
        }
    }
}
