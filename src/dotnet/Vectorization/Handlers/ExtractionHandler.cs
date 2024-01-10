using FoundationaLLM.Common.Constants;
using FoundationaLLM.Vectorization.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Handlers
{
    /// <summary>
    /// Handles the extraction stage of the vectorization pipeline.
    /// </summary>
    /// <param name="parameters">The dictionary of named parameters used to configure the handler.</param>
    /// <param name="stepsConfiguration">The app configuration section containing the configuration for vectorization pipeline steps.</param>
    /// <param name="logger">The logger used for logging.</param>
    public class ExtractionHandler(
        Dictionary<string, string> parameters,
        IConfigurationSection? stepsConfiguration,
        ILogger<ExtractionHandler> logger) : VectorizationStepHandlerBase(VectorizationSteps.Extract, parameters, stepsConfiguration, logger)
    {
        /// <inheritdoc/>
        protected override async Task ProcessRequest(
            VectorizationRequest request,
            VectorizationState state,
            IConfigurationSection? configuration,
            CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}
