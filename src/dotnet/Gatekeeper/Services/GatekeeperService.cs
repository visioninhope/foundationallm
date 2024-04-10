using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Agents;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Gatekeeper.Core.Interfaces;
using FoundationaLLM.Gatekeeper.Core.Models.ConfigurationOptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.Gatekeeper.Core.Services
{
    /// <summary>
    /// Implements the <see cref="IGatekeeperService"/> interface.
    /// </summary>
    /// <remarks>
    /// Constructor for the Gatekeeper service.
    /// </remarks>
    /// <param name="orchestrationAPIService">The Orchestration API client.</param>
    /// <param name="contentSafetyService">The user prompt Content Safety service.</param>
    /// <param name="lakeraGuardService">The Lakera Guard service.</param>
    /// <param name="guardrailsService">The Enkrypt Guardrails service.</param>
    /// <param name="gatekeeperIntegrationAPIService">The Gatekeeper Integration API client.</param>
    /// <param name="gatekeeperServiceSettings">The configuration options for the Gatekeeper service.</param>
    /// <param name="resourceProviderServices">The list of resurce providers registered with the main dependency injection container.</param>
    /// <param name="callContext">The call context of the request being handled.</param>
    /// <param name="logger">The logger used for logging.</param>
    public class GatekeeperService(
        IDownstreamAPIService orchestrationAPIService,
        IContentSafetyService contentSafetyService,
        ILakeraGuardService lakeraGuardService,
        IEnkryptGuardrailsService guardrailsService,
        IGatekeeperIntegrationAPIService gatekeeperIntegrationAPIService,
        IOptions<GatekeeperServiceSettings> gatekeeperServiceSettings,
        IEnumerable<IResourceProviderService> resourceProviderServices,
        ICallContext callContext,
        ILogger<GatekeeperService> logger) : IGatekeeperService
    {
        private readonly IDownstreamAPIService _orchestrationAPIService = orchestrationAPIService;
        private readonly IContentSafetyService _contentSafetyService = contentSafetyService;
        private readonly ILakeraGuardService _lakeraGuardService = lakeraGuardService;
        private readonly IEnkryptGuardrailsService _guardrailsService = guardrailsService;
        private readonly IGatekeeperIntegrationAPIService _gatekeeperIntegrationAPIService = gatekeeperIntegrationAPIService;
        private readonly GatekeeperServiceSettings _gatekeeperServiceSettings = gatekeeperServiceSettings.Value;
        private readonly Dictionary<string, IResourceProviderService> _resourceProviderServices =
            resourceProviderServices.ToDictionary<IResourceProviderService, string>(
                rps => rps.Name);
        private readonly ICallContext _callContext = callContext;
        private readonly ILogger<GatekeeperService> _logger = logger;

        /// <summary>
        /// Gets a completion from the Gatekeeper service.
        /// </summary>
        /// <param name="completionRequest">The completion request containing the user prompt and message history.</param>
        /// <returns>The completion response.</returns>
        public async Task<CompletionResponse> GetCompletion(CompletionRequest completionRequest)
        {
            //TODO: Call the Refinement Service with the userPrompt
            //await _refinementService.RefineUserPrompt(completionRequest.Prompt);

            if (_gatekeeperServiceSettings.EnableLakeraGuard)
            {
                var promptInjectionResult = await _lakeraGuardService.DetectPromptInjection(completionRequest.UserPrompt!);

                if (!string.IsNullOrWhiteSpace(promptInjectionResult))
                    return new CompletionResponse() { Completion = promptInjectionResult };
            }

            if (_gatekeeperServiceSettings.EnableEnkryptGuardrails)
            {
                var promptInjectionResult = await _guardrailsService.DetectPromptInjection(completionRequest.UserPrompt!);

                if (!string.IsNullOrWhiteSpace(promptInjectionResult))
                    return new CompletionResponse() { Completion = promptInjectionResult };
            }

            if (_gatekeeperServiceSettings.EnableAzureContentSafety)
            {
                var contentSafetyResult = await _contentSafetyService.AnalyzeText(completionRequest.UserPrompt!);

                if (!contentSafetyResult.Safe)
                    return new CompletionResponse() { Completion = contentSafetyResult.Reason };
            }

            var completionResponse = await _orchestrationAPIService.GetCompletion(completionRequest);

            if (settings.EnableMicrosoftPresidio)
                completionResponse.Completion = await _gatekeeperIntegrationAPIService.AnonymizeText(completionResponse.Completion);

            return completionResponse;
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

            if (_gatekeeperServiceSettings.EnableLakeraGuard)
            {
                var promptinjectionResult = await _lakeraGuardService.DetectPromptInjection(summaryRequest.UserPrompt!);

                if (!string.IsNullOrWhiteSpace(promptinjectionResult))
                    return new SummaryResponse() { Summary = promptinjectionResult };
            }

            if (_gatekeeperServiceSettings.EnableAzureContentSafety)
            {
                var contentSafetyResult = await _contentSafetyService.AnalyzeText(summaryRequest.UserPrompt!);

                if (!contentSafetyResult.Safe)
                    return new SummaryResponse() { Summary = contentSafetyResult.Reason };
            }

            var summaryResponse = await _orchestrationAPIService.GetSummary(summaryRequest);

            if (_gatekeeperServiceSettings.EnableMicrosoftPresidio)
                summaryResponse.Summary = await _gatekeeperIntegrationAPIService.AnonymizeText(summaryResponse.Summary!);

            return summaryResponse;
        }
    }
}
