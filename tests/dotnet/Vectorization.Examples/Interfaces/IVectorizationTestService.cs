using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Core.Examples.Services;

namespace FoundationaLLM.Vectorization.Examples.Interfaces;

public interface IVectorizationTestService
{
    public InstanceSettings InstanceSettings { get; set; }

    public Task CreateDataSource(string name, List<AppConfigurationKeyValue> configValues);

    public Task CreateContentSourceProfile(string name);
    public Task CreateTextPartitioningProfile(string name);

    public Task CreateTextEmbeddingProfile(string name);

    public Task CreateIndexingProfile(string name);

    public Task<VectorizationResult> CreateVectorizationRequest(VectorizationRequest request);

    public Task<object> CheckVectorizationRequestStatus(VectorizationRequest request);

    public Task<string> QueryIndex(string name, string query);
    public Task DeleteIndexingProfile(string name, bool deleteIndex);
    public Task DeleteDataSource(string name, List<AppConfigurationKeyValue> configValues);
    public Task DeleteContentSourceProfile(string name);
    public Task DeleteTextPartitioningProfile(string name);

    public Task DeleteTextEmbeddingProfile(string name);
    public Task DeleteVectorizationRequest(VectorizationRequest name);
}