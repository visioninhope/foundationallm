using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Gateway.Interfaces;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Gateway.Client
{
    public class GatewayCoreServiceClient : IGatewayServiceClient
    {
        private readonly IGatewayCore _gatewayCore;
        private readonly ILogger<GatewayCoreServiceClient> _logger;

        /// <summary>
        /// Creates a new instance of the Gateway API service.
        /// </summary>
        /// <param name="gatewayCore">The <see cref="IGatewayCore"/> service.</param>
        /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
        public GatewayCoreServiceClient(
            IGatewayCore gatewayCore,
            ILogger<GatewayCoreServiceClient> logger)
        {
            _gatewayCore = gatewayCore;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, object>> CreateAgentCapability(
            string instanceId, string capabilityCategory, string capabilityName,
            UnifiedUserIdentity userIdentity, Dictionary<string, object>? parameters = null) =>
            await _gatewayCore.CreateAgentCapability(instanceId, capabilityCategory, capabilityName, userIdentity, parameters);

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
