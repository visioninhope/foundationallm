using FoundationaLLM.Agent.Models.Metadata;
using FoundationaLLM.AgentFactory.Core.Models.Orchestration;
using FoundationaLLM.AgentFactory.Interfaces;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.AgentFactory.Core.Orchestration
{
    /// <summary>
    /// Knowledge Management orchestration.
    /// </summary>
    public class KnowledgeManagementOrchestration : OrchestrationBase
    {
        private KnowledgeManagementCompletionRequest _completionRequestTemplate = null!;
        private readonly ICacheService _cacheService;
        private readonly ICallContext _callContext;
        private readonly ILogger<LegacyOrchestration> _logger;
        private readonly KnowledgeManagementAgent _agent;

        /// <summary>
        /// Constructor for default agent.
        /// </summary>
        /// <param name="agent">The <see cref="KnowledgeManagementAgent"/> agent.</param>
        /// <param name="cacheService">The <see cref="ICacheService"/> used to cache agent-related artifacts.</param>
        /// <param name="callContext">The call context of the request being handled.</param>
        /// <param name="orchestrationService"></param>
        /// <param name="promptHubService"></param>
        /// <param name="dataSourceHubService"></param>
        /// <param name="logger">The logger used for logging.</param>
        public KnowledgeManagementOrchestration(
            KnowledgeManagementAgent agent,
            ICacheService cacheService,
            ICallContext callContext,
            ILLMOrchestrationService orchestrationService,
            IPromptHubAPIService promptHubService,
            IDataSourceHubAPIService dataSourceHubService,
            ILogger<LegacyOrchestration> logger)
            : base(null, orchestrationService, promptHubService, dataSourceHubService)
        {
            _agent = agent;
            _cacheService = cacheService;
            _callContext = callContext;
            _logger = logger;
        }

        /// <inheritdoc/>
        public override Task Configure(string userPrompt, string sessionId) => base.Configure(userPrompt, sessionId);

        /// <inheritdoc/>
        public override async Task<CompletionResponse> GetCompletion(CompletionRequest completionRequest)
        {
            var result = await _orchestrationService.GetCompletion(
                new KnowledgeManagementCompletionRequest
                {
                    UserPrompt = completionRequest.UserPrompt!,
                    Agent = _agent
                });

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
