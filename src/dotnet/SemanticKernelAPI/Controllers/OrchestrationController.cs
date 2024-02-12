using Asp.Versioning;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.SemanticKernel.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text.Json;

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
        public async Task<LLMCompletionResponse> GetCompletion([FromBody] LLMCompletionRequest request)
        {
            //var expandoObject = JsonSerializer.Deserialize<ExpandoObject>(request.ToString(), new ExpandoObjectConverter());

            //var agentType = string.Empty;
            //try
            //{
            //    agentType = expandoObject.agent.type;
            //}
            //catch { }

            //if (agentType == "knowledge-management")
            //{
            //    var completionRequest = JsonSerializer.Deserialize<KnowledgeManagementCompletionRequest>(request.ToString()) as KnowledgeManagementCompletionRequest;

            //    return await _knowledgeManagementAgentPlugin.GetCompletion(completionRequest!);
            //}
            //else
            //{
            //    var completionRequest = JsonSerializer.Deserialize<LegacyCompletionRequest?>(request.ToString()) as LegacyCompletionRequest;

            //    return await _legacyAgentPlugin.GetCompletion(completionRequest!);
            //}

            return default;
        }
    }
}
