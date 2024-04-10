using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Models.ResourceProvider;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.DataSource.Models;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Resources;
using FoundationaLLM.Vectorization.Services.DataSources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Services.Pipelines
{
    /// <summary>
    /// Executes active vectorization data pipelines.
    /// </summary>
    /// <param name="configuration">The global configuration provider.</param>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> providing dependency injection services..</param>
    /// <param name="resourceProviderServices">The list of resurce providers registered with the main dependency injection container.</param>
    /// <param name="loggerFactory">Factory responsible for creating loggers.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public class PipelineExecutionService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        IEnumerable<IResourceProviderService> resourceProviderServices,
        ILoggerFactory loggerFactory,
        ILogger<PipelineExecutionService> logger) : IPipelineExecutionService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<PipelineExecutionService> _logger = logger;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly Dictionary<string, IResourceProviderService> _resourceProviderServices =
            resourceProviderServices.ToDictionary<IResourceProviderService, string>(
                rps => rps.Name);

        /// <inheritdoc/>
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!_resourceProviderServices.TryGetValue(ResourceProviderNames.FoundationaLLM_Vectorization, out var vectorizationResourceProvider))
            {
                _logger.LogError($"Could not retrieve the {ResourceProviderNames.FoundationaLLM_Vectorization} resource provider.");
                return;
            }

            if (!_resourceProviderServices.TryGetValue(ResourceProviderNames.FoundationaLLM_DataSource, out var dataSourceResourceProvider))
            {
                _logger.LogError($"Could not retrieve the {ResourceProviderNames.FoundationaLLM_DataSource} resource provider.");
                return;
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var pipelines = await vectorizationResourceProvider.HandleGetAsync(
                        $"/{VectorizationResourceTypeNames.VectorizationPipelines}", new UnifiedUserIdentity
                        {
                            Name = "VectorizationAPI",
                            UserId = "VectorizationAPI",
                            Username = "VectorizationAPI"
                        });
                    var activePipelines = (pipelines as List<VectorizationPipeline>)!.Where(p => p.Active).ToList();

                    foreach (var activePipeline in activePipelines)
                    {
                        var dataSource = await GetResource<DataSourceBase>(
                            activePipeline.DataSourceObjectId,
                            DataSourceResourceTypeNames.DataSources,
                            dataSourceResourceProvider);
                        var textPartitioningProfile = await GetResource<VectorizationProfileBase>(
                            activePipeline.TextPartitioningProfileObjectId,
                            VectorizationResourceTypeNames.TextPartitioningProfiles,
                            vectorizationResourceProvider);
                        var textEmbeddingProfile = await GetResource<VectorizationProfileBase>(
                            activePipeline.TextEmbeddingProfileObjectId,
                            VectorizationResourceTypeNames.TextEmbeddingProfiles,
                            vectorizationResourceProvider);
                        var indexingProfile = await GetResource<VectorizationProfileBase>(
                            activePipeline.IndexingProfileObjectId,
                            VectorizationResourceTypeNames.IndexingProfiles,
                            vectorizationResourceProvider);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            if(dataSource is null)
                            {                                
                                continue;
                            }
                            switch(dataSource.Type)
                            {
                                case DataSourceTypes.AzureDataLake:
                                    var vectorizationService = scope.ServiceProvider.GetRequiredService<IVectorizationService>();

                                    // resolve configuration references
                                    var blobStorageServiceSettings = new BlobStorageServiceSettings { AuthenticationType = BlobStorageAuthenticationTypes.Unknown };
                                    _configuration.Bind(
                                        $"{AppConfigurationKeySections.FoundationaLLM_Vectorization_ContentSources}:{dataSource.Name}",
                                        blobStorageServiceSettings);
                                    
                                    AzureDataLakeDataSourceService svc = new AzureDataLakeDataSourceService(
                                                                       (AzureDataLakeDataSource)dataSource,
                                                                       blobStorageServiceSettings,                                                                       
                                                                       _loggerFactory);
                                                                        
                                    var files = await svc.GetFilesListAsync();
                                    var firstMultipartToken = $"{blobStorageServiceSettings.AccountName}.dfs.core.windows.net";
                                    if (blobStorageServiceSettings.AccountName!.Equals("onelake"))
                                    {
                                        firstMultipartToken = $"{blobStorageServiceSettings.AccountName}.dfs.fabric.microsoft.com";
                                    }
                                    foreach (var file in files)
                                    {
                                        //first token is the container name
                                        var containerName = file.Split("/")[0];
                                        //remove the first token from the path
                                        var path = file.Substring(file.IndexOf('/') + 1);
                                        //path minus the file extension
                                        var canonical = path.Substring(0, path.LastIndexOf('.'));

                                        var vectorizationRequest = new VectorizationRequest()
                                        {
                                            ContentIdentifier = new ContentIdentifier()
                                            {
                                                DataSourceObjectId = dataSource.ObjectId!,
                                                MultipartId = new List<string> { firstMultipartToken, containerName, path },
                                                CanonicalId = canonical
                                            },
                                            ProcessingType = VectorizationProcessingType.Asynchronous,
                                            Steps = new List<VectorizationStep>()
                                            {
                                                new VectorizationStep()
                                                {
                                                    Id = VectorizationSteps.Extract,
                                                    Parameters = new Dictionary<string, string>()
                                                },
                                                new VectorizationStep()
                                                {
                                                    Id = VectorizationSteps.Partition,
                                                    Parameters = new Dictionary<string, string>()
                                                    {
                                                        {"text_partitioning_profile_name", textPartitioningProfile.Name }
                                                    }
                                                },
                                                new VectorizationStep()
                                                {
                                                    Id = VectorizationSteps.Embed,
                                                    Parameters = new Dictionary<string, string>()
                                                    {
                                                        {"text_embedding_profile_name", textEmbeddingProfile.Name }
                                                    }
                                                },
                                                new VectorizationStep()
                                                {
                                                    Id = VectorizationSteps.Index,
                                                    Parameters = new Dictionary<string, string>()
                                                    {
                                                        {"indexing_profile_name", indexingProfile.Name }
                                                    }
                                                }
                                            }
                                        };

                                        await vectorizationService.ProcessRequest(vectorizationRequest);
                                    }
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error was encountered while running the pipeline execution cycle.");
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        private async Task<T> GetResource<T>(string objectId, string resourceTypeName, IResourceProviderService resourceProviderService)
            where T : ResourceBase
        {
            var result = await resourceProviderService.HandleGetAsync(
                $"/{resourceTypeName}/{objectId.Split("/").Last()}", new UnifiedUserIdentity
                {
                    Name = "VectorizationAPI",
                    UserId = "VectorizationAPI",
                    Username = "VectorizationAPI"
                });
            return (result as List<T>)!.First();
        }

        /// <inheritdoc/>
        public async Task StopAsync(CancellationToken cancellationToken) =>
            await Task.CompletedTask;
    }
}
