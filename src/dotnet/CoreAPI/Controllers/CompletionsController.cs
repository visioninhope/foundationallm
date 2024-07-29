using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using FoundationaLLM.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Core.API.Controllers
{
    /// <summary>
    /// Methods for orchestration services exposed by the Gatekeeper API service.
    /// </summary>
    /// <remarks>
    /// Constructor for the Orchestration Controller.
    /// </remarks>
    [Authorize(Policy = "DefaultPolicy")]
    [ApiController]
    [Route("instances/{instanceId}")]
    public class CompletionsController : ControllerBase
    {
        private readonly ICoreService _coreService;
        private readonly IResourceProviderService _agentResourceProvider;
#pragma warning disable IDE0052 // Remove unread private members.
        private readonly ILogger<CompletionsController> _logger;
        ICallContext _callContext;

        /// <summary>
        /// Methods for orchestration services exposed by the Gatekeeper API service.
        /// </summary>
        /// <remarks>
        /// Constructor for the Orchestration Controller.
        /// </remarks>
        /// <param name="coreService">The Core service provides methods for getting
        /// completions from the orchestrator.</param>
        /// <param name="callContext">The call context for the request.</param>
        /// <param name="resourceProviderServices">The list of <see cref="IResourceProviderService"/> resource provider services.</param>
        /// <param name="logger">The logging interface used to log under the
        /// <see cref="CompletionsController"/> type name.</param>
        public CompletionsController(ICoreService coreService,
            ICallContext callContext,
            IEnumerable<IResourceProviderService> resourceProviderServices,
            ILogger<CompletionsController> logger)
        {
            _coreService = coreService;
            var resourceProviderServicesDictionary = resourceProviderServices.ToDictionary<IResourceProviderService, string>(
                rps => rps.Name);
            if (!resourceProviderServicesDictionary.TryGetValue(ResourceProviderNames.FoundationaLLM_Agent, out var agentResourceProvider))
                throw new ResourceProviderException($"The resource provider {ResourceProviderNames.FoundationaLLM_Agent} was not loaded.");
            _agentResourceProvider = agentResourceProvider;
            _logger = logger;
            _callContext = callContext;
        }

        /// <summary>
        /// Requests a completion from the downstream APIs.
        /// </summary>
        /// <param name="instanceId">The instance ID of the current request.</param>
        /// <param name="completionRequest">The user prompt for which to generate a completion.</param>
        [HttpPost("completions", Name = "GetCompletion")]
        public async Task<IActionResult> GetCompletion(string instanceId, [FromBody] CompletionRequest completionRequest)
        => !string.IsNullOrWhiteSpace(completionRequest.SessionId) ? Ok(await _coreService.GetChatCompletionAsync(instanceId, completionRequest)) :
                Ok(await _coreService.GetCompletionAsync(instanceId, completionRequest));
        

            

        /// <summary>
        /// Begins a completion operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
        /// <returns>Returns an <see cref="LongRunningOperation"/> object containing the OperationId and Status.</returns>
        [HttpPost("async-completions")]
        public async Task<ActionResult<LongRunningOperation>> StartCompletionOperation(string instanceId, CompletionRequest completionRequest)
        {
            var state = await _coreService.StartCompletionOperation(instanceId, completionRequest);
            return Accepted(state);
        }

        /// <summary>
        /// Gets the status of a completion operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="operationId">The OperationId for which to retrieve the status.</param>
        /// <returns>Returns an <see cref="LongRunningOperation"/> object containing the OperationId and Status.</returns>
        [HttpGet("async-completions/{operationId}/status")]
        public async Task<LongRunningOperation> GetCompletionOperationStatus(string instanceId, string operationId) =>
            await _coreService.GetCompletionOperationStatus(instanceId, operationId);

        /// <summary>
        /// Gets a completion operation result from the downstream APIs.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="operationId">The ID of the operation to retrieve.</param>
        /// <returns>Returns a completion response</returns>
        [HttpGet("async-completions/{operationId}/result")]
        public async Task<CompletionResponse> GetCompletionOperationResult(string instanceId, string operationId) =>
            await _coreService.GetCompletionOperationResult(instanceId, operationId);

        /// <summary>
        /// Retrieves a list of global and private agents.
        /// </summary>
        /// <param name="instanceId">The instance ID of the current request.</param>
        /// <returns>A list of available agents.</returns>
        [HttpGet("completions/agents", Name = "GetAgents")]
        public async Task<IEnumerable<ResourceProviderGetResult<AgentBase>>> GetAgents(string instanceId) =>
            await _agentResourceProvider.GetResourcesWithRBAC<AgentBase>(instanceId, _callContext.CurrentUserIdentity!);
    }
}
