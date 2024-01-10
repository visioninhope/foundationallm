using FoundationaLLM.Common.Constants;
using FoundationaLLM.Vectorization.Models;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Handlers
{
    /// <summary>
    /// Handles the partitioning stage of the vectorization pipeline.
    /// </summary>
    /// <param name="parameters">The dictionary of named parameters used to configure the handler.</param>
    /// <param name="logger">The logger used for logging.</param>
    public class PartitionHandler(
        Dictionary<string, string> parameters,
        ILogger<PartitionHandler> logger) : VectorizationStepHandlerBase(VectorizationSteps.Partition, parameters, logger)
    {
        /// <inheritdoc/>
        protected override async Task ProcessRequest(VectorizationRequest request, VectorizationState state, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}
