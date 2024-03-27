using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Vectorization.Constants;
using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models.Configuration;
using FoundationaLLM.Vectorization.Models.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Services.DataSources
{
    /// <summary>
    /// Manages data sources registered for use in the vectorization pipelines.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of the data source manager service.
    /// </remarks>
    /// <param name="dataSourceResourceProviderService">The vectorization resource provider service.</param>
    /// <param name="configuration">The global configuration provider.</param>
    /// <param name="loggerFactory">The logger factory used to create loggers.</param>
    public class DataSourceServiceFactory(
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_DataSource)] IResourceProviderService dataSourceResourceProviderService,
        IConfiguration configuration,
        ILoggerFactory loggerFactory) : IVectorizationServiceFactory<IDataSourceService>
    {
        private readonly IResourceProviderService _dataSourceResourceProviderService = dataSourceResourceProviderService;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;

        /// <inheritdoc/>
        public IDataSourceService GetService(string serviceName)
        {
            var contentSourceProfile = _dataSourceResourceProviderService.GetResource<ContentSourceProfile>(
                $"/{VectorizationResourceTypeNames.ContentSourceProfiles}/{serviceName}");

            return contentSourceProfile.ContentSource switch
            {
                ContentSourceType.AzureDataLake => CreateAzureDataLakeDataSourceService(serviceName),
                ContentSourceType.SharePointOnline => CreateSharePointOnlineDataSourceService(serviceName),
                ContentSourceType.AzureSQLDatabase => CreateAzureSQLDatabaseDataSourceService(serviceName),
                ContentSourceType.Web => CreateWebPageDataSourceService(serviceName),
                _ => throw new VectorizationException($"The content source type {contentSourceProfile.ContentSource} is not supported."),
            };
        }

        /// <inheritdoc/>
        public (IDataSourceService Service, VectorizationProfileBase VectorizationProfile) GetServiceWithProfile(string serviceName)
        {
            var contentSourceProfile = _dataSourceResourceProviderService.GetResource<ContentSourceProfile>(
                $"/{VectorizationResourceTypeNames.ContentSourceProfiles}/{serviceName}");

            return contentSourceProfile.ContentSource switch
            {
                ContentSourceType.AzureDataLake => (CreateAzureDataLakeDataSourceService(serviceName), contentSourceProfile),
                ContentSourceType.SharePointOnline => (CreateSharePointOnlineDataSourceService(serviceName), contentSourceProfile),
                ContentSourceType.AzureSQLDatabase => (CreateAzureSQLDatabaseDataSourceService(serviceName), contentSourceProfile),
                ContentSourceType.Web => (CreateWebPageDataSourceService(serviceName), contentSourceProfile),
                _ => throw new VectorizationException($"The content source type {contentSourceProfile.ContentSource} is not supported."),
            };
        }


        private DataLakeDataSourceService CreateAzureDataLakeDataSourceService(string serviceName)
        {
            var blobStorageServiceSettings = new BlobStorageServiceSettings { AuthenticationType = BlobStorageAuthenticationTypes.Unknown };
            _configuration.Bind(
                $"{AppConfigurationKeySections.FoundationaLLM_Vectorization_ContentSources}:{serviceName}",
                blobStorageServiceSettings);

            return new DataLakeDataSourceService(
                blobStorageServiceSettings,
                _loggerFactory);
        }

        private SharePointOnlineDataSourceService CreateSharePointOnlineDataSourceService(string serviceName)
        {
            var sharePointOnlineContentSourceServiceSettings = new SharePointOnlineContentSourceServiceSettings();
            _configuration.Bind(
                $"{AppConfigurationKeySections.FoundationaLLM_Vectorization_ContentSources}:{serviceName}",
                sharePointOnlineContentSourceServiceSettings);

            return new SharePointOnlineDataSourceService(
                sharePointOnlineContentSourceServiceSettings,
                _loggerFactory);
        }

        private AzureSQLDatabaseDataSourceService CreateAzureSQLDatabaseDataSourceService(string serviceName)
        {
            var azureSQLDatabaseContentSourceServiceSettings = new AzureSQLDatabaseContentSourceServiceSettings();
            _configuration.Bind(
                $"{AppConfigurationKeySections.FoundationaLLM_Vectorization_ContentSources}:{serviceName}",
                azureSQLDatabaseContentSourceServiceSettings);

            return new AzureSQLDatabaseDataSourceService(
                azureSQLDatabaseContentSourceServiceSettings,
                _loggerFactory);
        }

        private WebPageDataSourceService CreateWebPageDataSourceService(string serviceName)
            => new WebPageDataSourceService(_loggerFactory);
    }
}
