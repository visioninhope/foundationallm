using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.SemanticKernel.Core.Services;
using FoundationaLLM.Vectorization.Examples.Interfaces;
using Microsoft.Extensions.Options;
using NSubstitute.Routing.Handlers;

namespace FoundationaLLM.Vectorization.Examples.Services
{
    /// <summary>
    /// Service for running agent conversations using the Core API.
    /// </summary>
    /// <param name="coreAPITestManager"></param>
    /// <param name="azureAIService"></param>
    public class VectorizationTestService(
        //IIndexingService indexingService,
        IManagementAPITestManager managementAPITestManager,
        IVectorizationAPITestManager vectorizationAPITestManager,
        IOptions<InstanceSettings> instanceSettings) : IVectorizationTestService
    {
        private IManagementAPITestManager _managementAPITestManager = managementAPITestManager;
        private IVectorizationAPITestManager _vectorizationAPITestManager = vectorizationAPITestManager;
        //private IIndexingService _indexService = indexingService;
        private InstanceSettings _instanceSettings = instanceSettings.Value;

        InstanceSettings IVectorizationTestService.InstanceSettings { get { return _instanceSettings; } set { _instanceSettings = value; } }

        public async Task CreateDataSource(string name, List<AppConfigurationKeyValue> configList) 
        {
            foreach(var config in configList)
                await _managementAPITestManager.CreateAppConfiguration(config);

            await managementAPITestManager.CreateDataSource(name);
        }

        public Task CreateContentSourceProfile(string name)
        {
            return managementAPITestManager.CreateContentSourceProfile(name);
        }

        public Task CreateTextPartitioningProfile(string name)
        {
            return managementAPITestManager.CreateTextPartitioningProfile(name);
        }

        public Task CreateTextEmbeddingProfile(string name)
        {
            return managementAPITestManager.CreateTextEmbeddingProfile(name);
        }

        public Task CreateIndexingProfile(string name)
        {
            return managementAPITestManager.CreateIndexingProfile(name);
        }

        public Task<VectorizationResult> CreateVectorizationRequest(VectorizationRequest request)
        {
            return _vectorizationAPITestManager.CreateVectorizationRequest(request);
        }

        public Task<object> CheckVectorizationRequestStatus(VectorizationRequest request)
        {
            return managementAPITestManager.GetVectorizationRequest(request);

        }

        public Task<string> QueryIndex(string name, string query)
        {
            //_indexService.QueryEmbeddings();
            return Task.FromResult("Querying index");
        }

        public async Task DeleteDataSource(string name, List<AppConfigurationKeyValue> configList)
        {
            await managementAPITestManager.DeleteDataSource(name, configList);

            return;
        }

        public Task DeleteContentSourceProfile(string name)
        {
            return managementAPITestManager.DeleteContentSourceProfile(name);
        }

        public Task DeleteTextPartitioningProfile(string name)
        {
            return managementAPITestManager.DeleteTextPartitioningProfile(name);
        }

        public Task DeleteTextEmbeddingProfile(string name)
        {
            return managementAPITestManager.DeleteTextEmbeddingProfile(name);
        }


        public Task DeleteIndexingProfile(string name, bool deleteIndex = true)
        {
            return managementAPITestManager.DeleteIndexingProfile(name);
        }

        public Task DeleteVectorizationRequest(VectorizationRequest request)
        {
            //remove all artifacts related to the request
            return null;
        }
    }
}
