using FoundationaLLM.Common.Models.Azure;
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
    public abstract class ModelDeploymentContext()
    {
        protected const int OPENAI_MAX_INPUT_SIZE_TOKENS = 8191;

        protected readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

        /// <summary>
        /// The cummulated number of tokens for the current token rate window.
        /// </summary>
        protected int _tokenRateWindowTokenCount = 0;
        /// <summary>
        /// The cummulated number of requests for the current request rate window.
        /// </summary>
        protected int _requestRateWindowRequestCount = 0;
        /// <summary>
        /// The start timestamp of the current token rate window.
        /// </summary>
        protected DateTime _tokenRateWindowStart = DateTime.MinValue;
        /// <summary>
        /// The start timestamp of the current request rate window.
        /// </summary>
        protected DateTime _requestRateWindowStart = DateTime.MinValue;

        protected int _currentRequestTokenCount = 0;

        protected virtual void UpdateRateWindows()
        {

        }

        public abstract Task<CompletionResult> GetCompletion();
    }
}
