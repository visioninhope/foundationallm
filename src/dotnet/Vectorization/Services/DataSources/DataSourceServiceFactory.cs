using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Models.ResourceProvider;
using FoundationaLLM.DataSource.Constants;
using FoundationaLLM.DataSource.Models;
using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models.Configuration;
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
            var (service, _) = this.GetServiceWithResource(serviceName);
            return service;
        }

        /// <inheritdoc/>
        public (IDataSourceService Service, ResourceBase Resource) GetServiceWithResource(string serviceName)
        {
            // serviceName is the data_source_object_id of the request
            var dataSource = dataSourceResourceProviderService.GetResource<DataSourceBase>(serviceName);
            if (dataSource == null)
                throw new VectorizationException($"The data source {serviceName} was not found.");

            return dataSource.Type switch
            {
                DataSourceTypes.AzureDataLake => (CreateAzureDataLakeDataSourceService(serviceName), dataSource),
                DataSourceTypes.SharePointOnlineSite => (CreateSharePointOnlineDataSourceService(serviceName), dataSource),
                DataSourceTypes.AzureSQLDatabase => (CreateAzureSQLDatabaseDataSourceService(serviceName), dataSource),
                // DataSourceTypes.Web => (CreateWebPageDataSourceService(serviceName), dataSource),
                _ => throw new VectorizationException($"The data source type {dataSource.Type} is not supported."),
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
