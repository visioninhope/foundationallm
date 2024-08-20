using FoundationaLLM.Common.Constants.OpenAI;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Vectorization;
using System.Text.Json;

namespace FoundationaLLM.Gateway.Client
{
    public class NoOpGatewayServiceClient : IGatewayServiceClient
    {
        /// <inheritdoc/>
        public async Task<Dictionary<string, object>> CreateAgentCapability(
            string instanceId, string capabilityCategory, string capabilityName,
            UnifiedUserIdentity userIdentity, Dictionary<string, object>? parameters = null)
        {
            await Task.CompletedTask;

            Dictionary<string, object> result = [];

            result[OpenAIAgentCapabilityParameterNames.AssistantId] = JsonSerializer.SerializeToElement(string.Empty);
            result[OpenAIAgentCapabilityParameterNames.AssistantFileId] = JsonSerializer.SerializeToElement(string.Empty);
            result[OpenAIAgentCapabilityParameterNames.AssistantThreadId] = JsonSerializer.SerializeToElement(string.Empty);
            result[OpenAIAgentCapabilityParameterNames.AttachmentObjectId] = JsonSerializer.SerializeToElement(string.Empty);

            return result;
        }

        /// <inheritdoc/>
        public Task<TextEmbeddingResult> GetEmbeddingOperationResult(
            string instanceId, string operationId, UnifiedUserIdentity userIdentity) =>
            throw new NotImplementedException();

        /// <inheritdoc/>
        public Task<TextEmbeddingResult> StartEmbeddingOperation(
            string instanceId, TextEmbeddingRequest embeddingRequest, UnifiedUserIdentity userIdentity) =>
            throw new NotImplementedException();
    }
}
