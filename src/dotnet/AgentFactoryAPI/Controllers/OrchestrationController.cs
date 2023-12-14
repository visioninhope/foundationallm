using Asp.Versioning;
using FoundationaLLM.AgentFactory.Core.Interfaces;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.AgentFactory.API.Controllers
{
    /// <summary>
    /// OrchestrationController class
    /// </summary>
    [ApiVersion(1.0)]
    [ApiController]
    [APIKeyAuthentication]
    [Route("[controller]")]
    public class OrchestrationController : ControllerBase
    {
        private readonly IAgentFactoryService _agentFactoryService;
        private readonly ILogger<OrchestrationController> _logger;


        /// <summary>
        /// Constructor for the Agent Factory orchestration controller
        /// </summary>
        /// <param name="agentFactoryService"></param>
        /// <param name="logger"></param>
        public OrchestrationController(
            IAgentFactoryService agentFactoryService,
            ILogger<OrchestrationController> logger)
        {
            _agentFactoryService = agentFactoryService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a completion from an orchestration service
        /// </summary>
        /// <param name="completionRequest"></param>
        /// <returns></returns>
        [HttpPost("completion")]
        public async Task<CompletionResponse> GetCompletion([FromBody] CompletionRequest completionRequest)
        {
            using var activity = Common.Logging.ActivitySources.AgentFactoryAPIActivitySource.StartActivity("GetCompletion", System.Diagnostics.ActivityKind.Consumer);

            foreach (var bag in activity?.Parent?.Baggage)
            {
                activity?.AddTag(bag.Key, bag.Value);
            }

            return await _agentFactoryService.GetCompletion(completionRequest);
        }

        /// <summary>
        /// Gets a summary from the Agent Factory
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("summary")]
        public async Task<SummaryResponse> GetSummary([FromBody] SummaryRequest content)
        {
            using var activity = Common.Logging.ActivitySources.AgentFactoryAPIActivitySource.StartActivity("GetSummary", System.Diagnostics.ActivityKind.Consumer);

            foreach (var bag in activity?.Parent?.Baggage)
            {
                activity?.AddTag(bag.Key, bag.Value);
            }

            return await _agentFactoryService.GetSummary(content);
        }
    }
}
