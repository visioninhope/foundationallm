using FoundationaLLM.Agent.Models.Metadata;
using FoundationaLLM.AgentFactory.Core.Models.Orchestration;
using FoundationaLLM.AgentFactory.Interfaces;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FoundationaLLM.AgentFactory.Core.Agents
{
    /// <summary>
    /// Internal context agent.
    /// </summary>
    public class ICAgent : AgentBase
    {
        private readonly ICacheService _cacheService;
        private readonly ICallContext _callContext;
        private readonly ILogger<DefaultAgent> _logger;
        private readonly InternalContextAgent _agent;

        /// <summary>
        /// Constructor for default agent.
        /// </summary>
        /// <param name="agent">The <see cref="InternalContextAgent"/> agent.</param>
        /// <param name="cacheService">The <see cref="ICacheService"/> used to cache agent-related artifacts.</param>
        /// <param name="callContext">The call context of the request being handled.</param>
        /// <param name="orchestrationService"></param>
        /// <param name="promptHubService"></param>
        /// <param name="dataSourceHubService"></param>
        /// <param name="logger">The logger used for logging.</param>
        public ICAgent(
            InternalContextAgent agent,
            ICacheService cacheService,
            ICallContext callContext,
            ILLMOrchestrationService orchestrationService,
            IPromptHubAPIService promptHubService,
            IDataSourceHubAPIService dataSourceHubService,
            ILogger<DefaultAgent> logger)
            : base(null, orchestrationService, promptHubService, dataSourceHubService)
        {
            _agent = agent;
            _cacheService = cacheService;
            _callContext = callContext;
            _logger = logger;
        }

        public override Task Configure(string userPrompt, string sessionId) => base.Configure(userPrompt, sessionId);

        public override async Task<CompletionResponse> GetCompletion(CompletionRequest completionRequest)
        {
            
            var result = await _orchestrationService.GetCompletion(
                _agent.Name,                
                JsonConvert.SerializeObject(new
                {
                    user_prompt = completionRequest.UserPrompt,
                    agent = _agent
                }));

            return new CompletionResponse
            {
                Completion = result.Completion!,
                UserPrompt = completionRequest.UserPrompt!,
                FullPrompt = result.FullPrompt,
                PromptTemplate = result.PromptTemplate,
                AgentName = result.AgentName,
                PromptTokens = result.PromptTokens,
                CompletionTokens = result.CompletionTokens,
            };
        }
    }
}
