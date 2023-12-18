using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Gatekeeper.Core.Interfaces;

namespace FoundationaLLM.Gatekeeper.Core.Services
{
    /// <summary>
    /// Implements the <see cref="IGatekeeperService"/> interface.
    /// </summary>
    public class GatekeeperService : IGatekeeperService
    {
        private readonly IGatekeeperIntegrationAPIService _gatekeeperIntegrationAPIService;
        private readonly IAgentFactoryAPIService _agentFactoryAPIService;
        private readonly IRefinementService _refinementService;
        private readonly IContentSafetyService _contentSafetyService;

        /// <summary>
        /// Constructor for the Gatekeeper service.
        /// </summary>
        /// <param name="gatekeeperIntegrationAPIService">The Gatekeeper Integration API client.</param>
        /// <param name="agentFactoryAPIService">The Agent Factory API client.</param>
        /// <param name="refinementService">The user prompt Refinement service.</param>
        /// <param name="contentSafetyService">The user prompt Content Safety service.</param>
        public GatekeeperService(
            IGatekeeperIntegrationAPIService gatekeeperIntegrationAPIService,
            IAgentFactoryAPIService agentFactoryAPIService,
            IRefinementService refinementService,
            IContentSafetyService contentSafetyService)
        {
            _gatekeeperIntegrationAPIService = gatekeeperIntegrationAPIService;
            _agentFactoryAPIService = agentFactoryAPIService;
            _refinementService = refinementService;
            _contentSafetyService = contentSafetyService;
        }

        /// <summary>
        /// Gets a completion from the Gatekeeper service.
        /// </summary>
        /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
        /// <returns>The completion response.</returns>
        public async Task<CompletionResponse> GetCompletion(CompletionRequest completionRequest)
        {
            //TODO: Call the Refinement Service with the userPrompt
            //await _refinementService.RefineUserPrompt(completionRequest.Prompt);

            var contentSafetyResult = await _contentSafetyService.AnalyzeText(completionRequest.UserPrompt ?? string.Empty);
            
            if (!contentSafetyResult.Safe)
                return new CompletionResponse() { Completion = contentSafetyResult.Reason };

            //TODO: Call the Gatekeeper Integration API with the userPrompt
            //var gatekeeperIntegrationResult = await _gatekeeperIntegrationAPIService.AnalyzeText(completionRequest.UserPrompt ?? string.Empty);

            return await _agentFactoryAPIService.GetCompletion(completionRequest);
        }

        /// <summary>
        /// Gets a summary from the Gatekeeper service.
        /// </summary>
        /// <param name="summaryRequest">The summarize request containing the user prompt.</param>
        /// <returns>The summary response.</returns>
        public async Task<SummaryResponse> GetSummary(SummaryRequest summaryRequest)
        {
            //TODO: Call the Refinement Service with the userPrompt
            //await _refinementService.RefineUserPrompt(summaryRequest.Prompt);

            var contentSafetyResult = await _contentSafetyService.AnalyzeText(summaryRequest.UserPrompt ?? string.Empty);

            if (!contentSafetyResult.Safe)
                return new SummaryResponse() { Summary = contentSafetyResult.Reason };

            //TODO: Call the Gatekeeper Integration API with the userPrompt
            //var gatekeeperIntegrationResult = await _gatekeeperIntegrationAPIService.AnalyzeText(summaryRequest.UserPrompt ?? string.Empty);

            return await _agentFactoryAPIService.GetSummary(summaryRequest);
        }
    }
}
