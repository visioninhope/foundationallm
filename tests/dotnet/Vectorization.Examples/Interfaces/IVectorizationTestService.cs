using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Core.Examples.Services;

namespace FoundationaLLM.Vectorization.Examples.Interfaces;

public interface IVectorizationTestService
{
    public InstanceSettings InstanceSettings { get; set; }

    public Task CreateDataSource(IStorageService svc, string name);

    public Task CreateContentSourceProfile(string name);
    public Task CreateTextPartitioningProfile(string name);

    public Task CreateTextEmbeddingProfile(string name);

    public Task CreateIndexingProfile(string name);

    public Task CreateVectorizationRequest(VectorizationRequest request);

    public Task CheckVectorizationRequestStatus(VectorizationRequest request);

    public Task QueryIndex(string name, string query);
    public Task DeleteIndexingProfile(string name, bool deleteIndex);
    public Task DeleteDataSource(string name);
    public Task DeleteContentSourceProfile(string name);
    public Task DeleteTextPartitioningProfile(string name);

    public Task DeleteTextEmbeddingProfile(string name);
    public Task DeleteVectorizationRequest(VectorizationRequest name);
}