using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using FoundationaLLM.Vectorization.Models.Resources;
using FoundationaLLM.Vectorization.ResourceProviders;
using Microsoft.Extensions.Configuration;
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
    public class ContentSourceManagerService(
        IResourceProviderService vectorizationResourceProviderService,
        IConfiguration configuration,
        ILoggerFactory loggerFactory) : IContentSourceManagerService
    {
        private readonly IResourceProviderService _vectorizationResourceProviderService = vectorizationResourceProviderService;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;

        /// <inheritdoc/>
        public IContentSourceService GetContentSourceService(string contentSourceName)
        {
            var contentSource = _vectorizationResourceProviderService.GetResource<ContentSource>(
                $"/{VectorizationResourceTypeNames.ContentSources}/{contentSourceName}");

            return contentSource.Type switch
            {
                ContentSourceType.AzureDataLake => CreateAzureDataLakeContentSourceService(contentSourceName),
                _ => throw new VectorizationException($"The content source type {contentSource.Type} is not supported."),
            };
        }

        private DataLakeContentSourceService CreateAzureDataLakeContentSourceService(string contentSourceName)
        {
            var blobStorageServiceSettings = new BlobStorageServiceSettings { AuthenticationType = BlobStorageAuthenticationTypes.Unknown };
            _configuration.Bind(
                $"{AppConfigurationKeySections.FoundationaLLM_Vectorization_ContentSources}:{contentSourceName}",
                blobStorageServiceSettings);

            return new DataLakeContentSourceService(
                blobStorageServiceSettings,
                _loggerFactory);
        }
    }
}
