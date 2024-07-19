using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Orchestration.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Orchestration.API.Controllers
{
    /// <summary>
    /// CompletionsController class
    /// </summary>
    /// <remarks>
    /// Constructor for the Orchestration orchestration controller
    /// </remarks>
    /// <param name="orchestrationService"></param>
    /// <param name="logger"></param>
    [ApiController]
    [APIKeyAuthentication]
    [Route("instances/{instanceId}")]
    public class CompletionsController(
        IOrchestrationService orchestrationService,
        ILogger<CompletionsController> logger) : ControllerBase
    {
        private readonly IOrchestrationService _orchestrationService = orchestrationService;
        private readonly ILogger<CompletionsController> _logger = logger;

        /// <summary>
        /// Retrieves a completion from an orchestration service
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="completionRequest">The completion request.</param>
        /// <returns>The completion response.</returns>
        [HttpPost("completions")]
        public async Task<CompletionResponse> GetCompletion(string instanceId, [FromBody] CompletionRequest completionRequest) =>
            await _orchestrationService.GetCompletion(instanceId, completionRequest);

        /// <summary>
        /// Begins a completion operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
        /// <returns>Returns an <see cref="OperationState"/> object containing the OperationId and Status.</returns>
        [HttpPost("async-completions")]
        public async Task<ActionResult<OperationState>> StartCompletionOperation(string instanceId, CompletionRequest completionRequest)
        {
            var state = await _orchestrationService.StartCompletionOperation(instanceId, completionRequest);
            return Accepted(state);
        }

        /// <summary>
        /// Gets the status of a completion operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="operationId">The OperationId for which to retrieve the status.</param>
        /// <returns>Returns an <see cref="OperationState"/> object containing the OperationId and Status.</returns>
        [HttpGet("async-completions/{operationId}/status")]
        public async Task<OperationState> GetCompletionOperationStatus(string instanceId, string operationId) =>
            await _orchestrationService.GetCompletionOperationStatus(instanceId, operationId);

        /// <summary>
        /// Gets a completion operation from the Orchestration service.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="operationId">The ID of the operation to retrieve.</param>
        /// <returns>Returns a completion response</returns>
        [HttpGet("async-completions/{operationId}/result")]
        public async Task<CompletionResponse> GetCompletionOperation(string instanceId, string operationId) =>
            await _orchestrationService.GetCompletionOperation(instanceId, operationId);

    }
}
