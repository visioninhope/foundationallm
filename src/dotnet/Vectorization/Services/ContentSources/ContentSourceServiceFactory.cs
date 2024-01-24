using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using FoundationaLLM.Vectorization.Models.Resources;
using FoundationaLLM.Vectorization.ResourceProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.Vectorization.Services.ContentSources
{
    /// <summary>
    /// Manages content sources registered for use in the vectorization pipelines.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of the content source manager service.
    /// </remarks>
    /// <param name="vectorizationResourceProviderService">The vectorization resource provider service.</param>
    /// <param name="configuration">The global configuration provider.</param>
    /// <param name="loggerFactory">The logger factory used to create loggers.</param>
    public class ContentSourceServiceFactory(
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_Vectorization_ResourceProviderService)] IResourceProviderService vectorizationResourceProviderService,
        IConfiguration configuration,
        ILoggerFactory loggerFactory) : IServiceFactory<IContentSourceService>
    {
        private readonly IResourceProviderService _vectorizationResourceProviderService = vectorizationResourceProviderService;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;

        /// <inheritdoc/>
        public IContentSourceService CreateService(string serviceName)
        {
            var contentSource = _vectorizationResourceProviderService.GetResource<ContentSource>(
                $"/{VectorizationResourceTypeNames.ContentSources}/{serviceName}");

            return contentSource.Type switch
            {
                ContentSourceType.AzureDataLake => CreateAzureDataLakeContentSourceService(serviceName),
                _ => throw new VectorizationException($"The content source type {contentSource.Type} is not supported."),
            };
        }

        private DataLakeContentSourceService CreateAzureDataLakeContentSourceService(string serviceName)
        {
            var blobStorageServiceSettings = new BlobStorageServiceSettings { AuthenticationType = BlobStorageAuthenticationTypes.Unknown };
            _configuration.Bind(
                $"{AppConfigurationKeySections.FoundationaLLM_Vectorization_ContentSources}:{serviceName}",
                blobStorageServiceSettings);

            return new DataLakeContentSourceService(
                blobStorageServiceSettings,
                _loggerFactory);
        }
    }
}
