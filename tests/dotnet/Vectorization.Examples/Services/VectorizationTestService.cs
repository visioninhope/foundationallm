using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.Vectorization.Examples.Interfaces;

namespace FoundationaLLM.Vectorization.Examples.Services
{
    /// <summary>
    /// Service for running agent conversations using the Core API.
    /// </summary>
    /// <param name="coreAPITestManager"></param>
    /// <param name="azureAIService"></param>
    public class VectorizationTestService(
        IManagementAPITestManager managementAPITestManager,
        IVectorizationAPITestManager vectorizationAPITestManager) : IVectorizationTestService
    {
        private IManagementAPITestManager _managementAPITestManager = managementAPITestManager;
        private IVectorizationAPITestManager _vectorizationAPITestManager = vectorizationAPITestManager;

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
            return null;
        }

        public Task CheckVectorizationRequestStatus(VectorizationRequest request)
        {
            return null;

        }

        public Task QueryIndex(string name, string query)
        {
            return null;
        }

        public Task DeleteIndexingProfile(string name, bool deleteIndex)
        {
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

        public Task DeleteIndexingProfile(string name)
        {
            return managementAPITestManager.DeleteIndexingProfile(name);
        }

        public Task DeleteVectorizationRequest(string name)
        {
            return null;
        }
    }
}
