using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
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

        public Task CreateDataSource(IStorageService svc, string name)
        {
            return managementAPITestManager.CreateDataSource(svc, name);
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

        public Task CreateVectorizationRequest(VectorizationRequest request)
        {
            return _vectorizationAPITestManager.CreateVectorizationRequest(request);
        }

        public Task CheckVectorizationRequestStatus(VectorizationRequest request)
        {
            return _vectorizationAPITestManager.CheckVectorizationRequest(request);

        }

        public Task QueryIndex(string name, string query)
        {
            //_indexService.QueryEmbeddings();
            return null;
        }

        public Task DeleteDataSource(string name)
        {
            return managementAPITestManager.DeleteDataSource(name);
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
