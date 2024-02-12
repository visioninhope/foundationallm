using Asp.Versioning;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.SemanticKernel.Core.Interfaces;
using FoundationaLLM.SemanticKernel.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace FoundationaLLM.SemanticKernel.API.Controllers
{
    /// <summary>
    /// Wrapper for the Semantic Kernel service.
    /// </summary>
    [ApiVersion(1.0)]
    [ApiController]
    [APIKeyAuthentication]
    [Route("[controller]")]
    public class OrchestrationController : ControllerBase
    {
        private readonly IKnowledgeManagementAgentPlugin _knowledgeManagementAgentPlugin;
        private readonly ILegacyAgentPlugin _legacyAgentPlugin;

        /// <summary>
        /// Constructor for the Semantic Kernel API orchestration controller.
        /// </summary>
        /// <param name="knowledgeManagementAgentPlugin"></param>
        /// <param name="legacyAgentPlugin"></param>
        public OrchestrationController(
            IKnowledgeManagementAgentPlugin knowledgeManagementAgentPlugin,
            ILegacyAgentPlugin legacyAgentPlugin)
        {
            _knowledgeManagementAgentPlugin = knowledgeManagementAgentPlugin;
            _legacyAgentPlugin = legacyAgentPlugin;
        }

        /// <summary>
        /// Gets a completion from the Semantic Kernel service.
        /// </summary>
        /// <param name="request">The completion request containing the user prompt and message history.</param>
        /// <returns>The completion response.</returns>
        [HttpPost("completion")]
        public async Task<LLMCompletionResponse> GetCompletion([FromBody] dynamic request)
        {
            var expandoObject = JsonConvert.DeserializeObject<ExpandoObject>(request.ToString(), new ExpandoObjectConverter());

            var agentType = string.Empty;
            try
            {
                agentType = expandoObject.agent.type;
            }
            catch { }

            if (agentType == "knowledge-management")
            {
                var completionRequest = JsonConvert.DeserializeObject<KnowledgeManagementCompletionRequest>(request.ToString()) as KnowledgeManagementCompletionRequest;

                return await _knowledgeManagementAgentPlugin.GetCompletion(completionRequest!);
            }
            else
            {
                var completionRequest = JsonConvert.DeserializeObject<LegacyOrchestrationCompletionRequest?>(request.ToString()) as LegacyOrchestrationCompletionRequest;

                return await _legacyAgentPlugin.GetCompletion(completionRequest!);
            }
        }
    }
}
