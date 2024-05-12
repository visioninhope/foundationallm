using FoundationaLLM.Common.Models.Azure;
using FoundationaLLM.Common.Models.Gateway;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Vectorization;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Common.Models.Gateway
{
    /// <summary>
    /// Provides context associated with an embedding model deployment.
    /// </summary>
    /// <param name="deployment">The <see cref="AzureOpenAIAccountDeployment"/> object with the details of the model deployment.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to create loggers for logging.</param>
    public class CompletionModelDeploymentContextBase(
        AzureOpenAIAccountDeployment deployment,
        ILoggerFactory loggerFactory) : ModelDeploymentContext
    {
        protected readonly AzureOpenAIAccountDeployment _deployment = deployment;
        protected readonly ILoggerFactory _loggerFactory = loggerFactory;
        protected readonly ILogger<CompletionModelDeploymentContextBase> _logger = loggerFactory.CreateLogger<CompletionModelDeploymentContextBase>();
        protected List<GatewayCompletionRequest> _inputRequests = [];

        public bool HasInput =>
            _inputRequests.Count > 0;

        public bool TryAddCompletion(GatewayCompletionRequest request)
        {
            UpdateRateWindows();

            if (_tokenRateWindowTokenCount + request.TokensCount > _deployment.TokenRateLimit
                || _currentRequestTokenCount + request.TokensCount > OPENAI_MAX_INPUT_SIZE_TOKENS)
                // Adding a new text chunk would either push us over to the token rate limit or exceed the maximum input size, so we need to refuse.
                return false;

            if (_requestRateWindowRequestCount == _deployment.RequestRateLimit)
                // We have already reached the allowed number of requests, so we need to refuse.
                return false;

            _inputRequests.Add(request);
            _tokenRateWindowTokenCount += request.TokensCount;
            _currentRequestTokenCount += request.TokensCount;

            return true;
        }

        override public async Task<CompletionResult> GetCompletion() => throw new NotImplementedException();
    }
}
