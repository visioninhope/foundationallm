using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Core.Examples.Models;

namespace FoundationaLLM.Core.Examples.Interfaces;

public interface IVectorizationTestService
{
    public InstanceSettings InstanceSettings { get; set; }

    public Task CreateDataSource(string name, List<AppConfigurationKeyValue> configValues);
    public Task CreateTextPartitioningProfile(string name);
    public Task CreateTextEmbeddingProfile(string name);
    public Task CreateIndexingProfile(string name);
    public Task<string> CreateVectorizationRequest(VectorizationRequest request);
    Task<VectorizationResult> ProcessVectorizationRequest(VectorizationRequest request);
    public Task<VectorizationRequest> CheckVectorizationRequestStatus(VectorizationRequest request);
    public Task<TestSearchResult> QueryIndex(string indexProfileName, string embedProfileName, string query);
    public Task<TestSearchResult> QueryIndex(IndexingProfile indexProfile, TextEmbeddingProfile embedProfile, string query);
    public Task DeleteIndexingProfile(string name, bool deleteIndex);
    public Task DeleteDataSource(string name, List<AppConfigurationKeyValue> configValues);   
    public Task DeleteTextPartitioningProfile(string name);
    public Task DeleteTextEmbeddingProfile(string name);
}