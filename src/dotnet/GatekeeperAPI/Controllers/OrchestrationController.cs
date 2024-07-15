using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Gatekeeper.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Gatekeeper.API.Controllers
{
    /// <summary>
    /// Wrapper for Gatekeeper service.
    /// </summary>
    /// <remarks>
    /// Constructor for the Gatekeeper API orchestration controller.
    /// </remarks>
    /// <param name="gatekeeperService"></param>
    [ApiController]
    [APIKeyAuthentication]
    [Route($"instances/{{instanceId}}")]
    public class OrchestrationController(
        IGatekeeperService gatekeeperService) : ControllerBase
    {
        private readonly IGatekeeperService _gatekeeperService = gatekeeperService;

        /// <summary>
        /// Gets a completion from the Gatekeeper service.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
        /// <returns>The completion response.</returns>
        [HttpPost("completions")]
        public async Task<CompletionResponse> GetCompletion(string instanceId, CompletionRequest completionRequest) =>
            await _gatekeeperService.GetCompletion(instanceId, completionRequest);

        /// <summary>
        /// /// Begins a completion operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
        /// <returns>Returns an <see cref="OperationState"/> object containing the OperationId and Status.</returns>
        [HttpPost("completions/operations")]
        public async Task<OperationState> StartCompletionOperation(string instanceId, CompletionRequest completionRequest) =>
            await _gatekeeperService.StartCompletionOperation(instanceId, completionRequest);

        /// <summary>
        /// Gets a completion operation from the Gatekeeper service.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="operationId">The ID of the operation to retrieve.</param>
        /// <returns>Returns a completion response</returns>
        [HttpGet("completions/operations/{operationId}")]
        public async Task<CompletionResponse> GetCompletionOperation(string instanceId, string operationId) =>
            await _gatekeeperService.GetCompletionOperation(instanceId, operationId);
    }
}
