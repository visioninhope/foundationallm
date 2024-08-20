using FoundationaLLM.Common.Clients;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Infrastructure;
using FoundationaLLM.Common.Models.Orchestration.Request;
using FoundationaLLM.Common.Models.Orchestration.Response;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Orchestration.Core.Interfaces;
using FoundationaLLM.Orchestration.Core.Models.ConfigurationOptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace FoundationaLLM.Orchestration.Core.Services
{
    /// <summary>
    /// The FoundationaLLM Semantic Kernal Service
    /// </summary>
    /// <remarks>
    /// Constructor for the Semantic Kernal Service
    /// </remarks>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="callContext"></param>
    /// <param name="httpClientFactoryService"></param>
    public class SemanticKernelService(
        IOptions<SemanticKernelServiceSettings> options,
        ILogger<SemanticKernelService> logger,
        ICallContext callContext,
        IHttpClientFactoryService httpClientFactoryService) : ISemanticKernelService
    {
        readonly SemanticKernelServiceSettings _settings = options.Value;
        readonly ILogger<SemanticKernelService> _logger = logger;
        private readonly ICallContext _callContext = callContext;
        private readonly IHttpClientFactoryService _httpClientFactoryService = httpClientFactoryService;
        readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

        /// <inheritdoc/>
        public async Task<ServiceStatusInfo> GetStatus(string instanceId)
        {
            var client = await _httpClientFactoryService.CreateClient(HttpClientNames.SemanticKernelAPI, _callContext.CurrentUserIdentity);
            var responseMessage = await client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, "status"));

            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ServiceStatusInfo>(responseContent)!;
        }

        /// <inheritdoc/>
        public string Name => LLMOrchestrationServiceNames.SemanticKernel;

        /// <summary>
        /// Gets a completion from the Semantic Kernel service.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance ID.</param>
        /// <param name="request">Request object populated from the hub APIs including agent, prompt, data source, and model information.</param>
        /// <returns>Returns a completion response from the orchestration engine.</returns>
        public async Task<LLMCompletionResponse> GetCompletion(string instanceId, LLMCompletionRequest request)
        {
            var client = await _httpClientFactoryService.CreateClient(HttpClientNames.SemanticKernelAPI, _callContext.CurrentUserIdentity);

            var pollingClient = new PollingHttpClient<LLMCompletionRequest, LLMCompletionResponse>(
                client,
                request,
                $"instances/{instanceId}/async-completions",
                TimeSpan.FromSeconds(0.5),
                client.Timeout.Subtract(TimeSpan.FromSeconds(1)),
                _logger);

            try
            {
                var completionResponse = await pollingClient.GetResponseAsync();
                if (completionResponse == null)
                    throw new Exception("The Semantic Kernel orchestration service did not return a valid completion response.");
                return new LLMCompletionResponse
                {
                    OperationId = request.OperationId,
                    Completion = completionResponse!.Completion,
                    Citations = completionResponse.Citations,
                    UserPrompt = completionResponse.UserPrompt,
                    FullPrompt = completionResponse.FullPrompt,
                    PromptTemplate = string.Empty,
                    AgentName = request.Agent.Name,
                    PromptTokens = completionResponse.PromptTokens,
                    CompletionTokens = completionResponse.CompletionTokens
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the completion request against the Semantic Kernel orchestration service.");

                return new LLMCompletionResponse
                {
                    OperationId = request.OperationId,
                    Completion = "A problem on my side prevented me from responding.",
                    UserPrompt = request.UserPrompt,
                    PromptTemplate = string.Empty,
                    AgentName = request.Agent.Name,
                    PromptTokens = 0,
                    CompletionTokens = 0
                };
            }
        }
    }
}
