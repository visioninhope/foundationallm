using Asp.Versioning;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.SemanticKernel.Core.Interfaces;
using FoundationaLLM.SemanticKernel.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using FoundationaLLM.SemanticKernel.Core.Services;

namespace FoundationaLLM.SemanticKernel.API.Controllers
{
    /// <summary>
    /// Wrapper for the Semantic Kernel service.
    /// </summary>
    [ApiVersion(1.0)]
    [ApiController]
    //[APIKeyAuthentication]
    [Route("[controller]")]
    public class OrchestrationController : ControllerBase
    {
        //private readonly ISemanticKernelService _semanticKernelService;
        private readonly IKnowledgeManagementAgentService _knowledgeManagementAgentService;

        /// <summary>
        /// Constructor for the Semantic Kernel API orchestration controller.
        /// </summary>
        /// <param name="semanticKernelService"></param>
        public OrchestrationController(
            //ISemanticKernelService semanticKernelService) => _semanticKernelService = semanticKernelService;
            IKnowledgeManagementAgentService knowledgeManagementAgentService) => _knowledgeManagementAgentService = knowledgeManagementAgentService;

        /// <summary>
        /// Gets a completion from the Semantic Kernel service.
        /// </summary>
        /// <param name="request">The completion request containing the user prompt and message history.</param>
        /// <returns>The completion response.</returns>
        [HttpPost("completion")]
        public async Task<LLMOrchestrationCompletionResponse> GetCompletion([FromBody] dynamic request)
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

                return await _knowledgeManagementAgentService.GetCompletion(completionRequest!);
            }
            else
            {
                var completionRequest = JsonConvert.DeserializeObject<LLMOrchestrationCompletionRequest?>(request.ToString()) as LLMOrchestrationCompletionRequest;

                return null; //await _semanticKernelService.GetCompletion(completionRequest!);
            }
        }

        /// <summary>
        /// Gets a summary from the Semantic Kernel service.
        /// </summary>
        /// <param name="request">The summarize request containing the user prompt.</param>
        /// <returns>The summary response.</returns>
        [HttpPost("summary")]
        public async Task<SummaryResponse> GetSummary([FromBody] SummaryRequest request)
        {
            await Task.CompletedTask;

            return null;

            //var info = await _semanticKernelService.GetSummary(request.UserPrompt ?? string.Empty);

            //return new SummaryResponse() { Summary = info };
        }

        /// <summary>
        /// Add an object instance and its associated vectorization to the memory store.
        /// </summary>
        /// <returns></returns>
        [HttpPost("memory/add")]
        public Task AddMemory() =>
            //await _semanticKernelService.AddMemory();

            throw new NotImplementedException();

        /// <summary>
        /// Removes an object instance and its associated vectorization from the memory store.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("memory/remove")]
        public Task RemoveMemory() =>
            //await _semanticKernelService.RemoveMemory();

            throw new NotImplementedException();
    }
}
