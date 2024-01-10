using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Handlers
{
    /// <summary>
    /// Implements basic vectorization step handler functionality.
    /// </summary>
    /// <param name="stepId">The identifier of the vectorization step.</param>
    /// <param name="parameters">The dictionary of named parameters used to configure the handler.</param>
    /// <param name="logger">The logger used for logging.</param>
    public class VectorizationStepHandlerBase(
        string stepId,
        Dictionary<string, string> parameters,
        ILogger logger) : IVectorizationStepHandler
    {
        /// <summary>
        /// The identifier of the vectorization step.
        /// </summary>
        protected readonly string _stepId = stepId;
        /// <summary>
        /// The dictionary of named parameters used to configure the handler.
        /// </summary>
        protected readonly Dictionary<string, string> _parameters = parameters;
        /// <summary>
        /// The logger used for logging.
        /// </summary>
        protected readonly ILogger _logger = logger;

        /// <inheritdoc/>
        public string StepId => _stepId;

        /// <inheritdoc/>
        public async Task Invoke(VectorizationRequest request, VectorizationState state, CancellationToken cancellationToken)
        {
            try
            {
                state.LogHandlerStart(this, request.Id);
                _logger.LogInformation("Starting handler {HandlerId} for request {RequestId}", _stepId, request.Id);

                ValidateRequest(request);
                await ProcessRequest(request, state, cancellationToken);

                state.LogHandlerEnd(this, request.Id);
                _logger.LogInformation("Finished handler {HandlerId} for request {RequestId}", _stepId, request.Id);
            }
            catch (Exception ex)
            {
                state.LogHandlerError(this, request.Id, ex);
            }
        }

        private void ValidateRequest(VectorizationRequest request)
        {
            if (request[_stepId] == null)
                throw new VectorizationException($"The request with id {request.Id} does not contain a step with id {_stepId}.");
        }

        /// <summary>
        /// Processes a vectorization request.
        /// The vectorization state will be updated with the result(s) of the processing.
        /// </summary>
        /// <param name="request">The <see cref="VectorizationRequest"/> to be processed.</param>
        /// <param name="state">The <see cref="VectorizationState"/> associated with the vectorization request.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that signals stopping the processing.</param>
        /// <returns></returns>
        protected virtual async Task ProcessRequest(VectorizationRequest request, VectorizationState state, CancellationToken cancellationToken) =>
            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
    }
}
