using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Provides completion capabilities.
    /// </summary>
    public interface ICompletionsService
    {
        Task<CompletionResult> GetCompletionAsync(List<GatewayCompletionRequest> request, string modelName = "gpt-4");

        Task<CompletionResult> GetCompletionAsync(string operationId);
    }
}
