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
    [Route("instances/{instanceId}/[controller]")]
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
        [HttpPost(Name = "GetCompletion")]
        public async Task<CompletionResponse> GetCompletion(string instanceId, [FromBody] CompletionRequest completionRequest) =>
            await _orchestrationService.GetCompletion(instanceId, completionRequest);
    }
}
