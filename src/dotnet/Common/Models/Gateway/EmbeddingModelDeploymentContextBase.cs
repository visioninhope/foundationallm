using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Azure;
using FoundationaLLM.Common.Models.Gateway;
using FoundationaLLM.Common.Models.Vectorization;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Common.Models.Gateway
{
    /// <summary>
    /// Provides context associated with an embedding model deployment.
    /// </summary>
    /// <param name="deployment">The <see cref="AzureOpenAIAccountDeployment"/> object with the details of the model deployment.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to create loggers for logging.</param>
    public class EmbeddingModelDeploymentContextBase(
        AzureOpenAIAccountDeployment deployment,
        ILoggerFactory loggerFactory) : ModelDeploymentContext
    {
        protected readonly AzureOpenAIAccountDeployment _deployment = deployment;
        protected readonly ILoggerFactory _loggerFactory = loggerFactory;
        protected readonly ILogger<EmbeddingModelDeploymentContextBase> _logger = loggerFactory.CreateLogger<EmbeddingModelDeploymentContextBase>();
        protected List<TextChunk> _inputTextChunks = [];
        
        public bool HasInput =>
            _inputTextChunks.Count > 0;

        public bool TryAddInputTextChunk(TextChunk textChunk)
        {
            UpdateRateWindows();

            if (_tokenRateWindowTokenCount + textChunk.TokensCount > _deployment.TokenRateLimit
                || _currentRequestTokenCount + textChunk.TokensCount > OPENAI_MAX_INPUT_SIZE_TOKENS)
                // Adding a new text chunk would either push us over to the token rate limit or exceed the maximum input size, so we need to refuse.
                return false;

            if (_requestRateWindowRequestCount == _deployment.RequestRateLimit)
                // We have already reached the allowed number of requests, so we need to refuse.
                return false;

            _inputTextChunks.Add(textChunk);
            _tokenRateWindowTokenCount += textChunk.TokensCount;
            _currentRequestTokenCount += textChunk.TokensCount;

            return true;
        }

        public virtual async Task<TextEmbeddingResult> GetEmbeddingsForInputTextChunks() => throw new NotImplementedException();


        public override Task<CompletionResult> GetCompletion() => throw new NotImplementedException();
    }
}
