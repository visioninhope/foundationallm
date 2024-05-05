using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;

namespace FoundationaLLM.Core.Examples.Interfaces;

/// <summary>
/// Provides methods to interact with the FoundationaLLM Vectorization API.
/// </summary>
/// <param name="httpClientManager"></param>
public interface IVectorizationAPITestManager
{
    Task<string> CreateVectorizationRequest(VectorizationRequest request);
    Task<string> CheckVectorizationRequest(VectorizationRequest request);

    Task DeleteVectorizationRequest(VectorizationRequest request);
}