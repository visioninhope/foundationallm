using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Gatekeeper.Core.Interfaces;

namespace FoundationaLLM.Gatekeeper.Core.Services
{
    /// <summary>
    /// Implements the <see cref="IGatekeeperService"/> interface.
    /// </summary>
    /// <remarks>
    /// Constructor for the Gatekeeper service.
    /// </remarks>
    /// <param name="agentFactoryAPIService">The Agent Factory API client.</param>
    /// <param name="contentSafetyService">The user prompt Content Safety service.</param>
    public class GatekeeperService(
        IAgentFactoryAPIService agentFactoryAPIService,
        IContentSafetyService contentSafetyService) : IGatekeeperService
    {
        private readonly IAgentFactoryAPIService _agentFactoryAPIService = agentFactoryAPIService;
        private readonly IContentSafetyService _contentSafetyService = contentSafetyService;

        /// <summary>
        /// Gets a completion from the Gatekeeper service.
        /// </summary>
        /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
        /// <returns>The completion response.</returns>
        public async Task<CompletionResponse> GetCompletion(CompletionRequest completionRequest)
        {
            //TODO: Call RefinementService to refine userPrompt
            //await _refinementService.RefineUserPrompt(completionRequest.Prompt);

            var result = await _contentSafetyService.AnalyzeText(completionRequest.UserPrompt ?? string.Empty);
            
            if (result.Safe)
                return await _agentFactoryAPIService.GetCompletion(completionRequest);

            return new CompletionResponse() { Completion = result.Reason };
        }

        /// <summary>
        /// Gets a summary from the Gatekeeper service.
        /// </summary>
        /// <param name="summaryRequest">The summarize request containing the user prompt.</param>
        /// <returns>The summary response.</returns>
        public async Task<SummaryResponse> GetSummary(SummaryRequest summaryRequest)
        {
            //TODO: Call RefinementService to refine userPrompt
            //await _refinementService.RefineUserPrompt(summaryRequest.Prompt);

            var result = await _contentSafetyService.AnalyzeText(summaryRequest.UserPrompt ?? string.Empty);

            if (result.Safe)
                return await _agentFactoryAPIService.GetSummary(summaryRequest);

            return new SummaryResponse() { Summary = result.Reason };
        }
    }
}
