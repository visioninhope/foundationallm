using FoundationaLLM.AgentFactory.Core.Models.Orchestration;
using FoundationaLLM.AgentFactory.Interfaces;
using FoundationaLLM.AgentFactory.Models.ConfigurationOptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace FoundationaLLM.AgentFactory.Services
{
    /// <summary>
    /// The FoundationaLLM Semantic Kernal Service
    /// </summary>
    public class SemanticKernelService : ISemanticKernelService
    {
        readonly SemanticKernelServiceSettings _settings;
        readonly ILogger<SemanticKernelService> _logger;
        private readonly IHttpClientFactoryService _httpClientFactoryService;
        readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// Constructor for the Semantic Kernal Service
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="httpClientFactoryService"></param>
        public SemanticKernelService(
            IOptions<SemanticKernelServiceSettings> options,
            ILogger<SemanticKernelService> logger,
            IHttpClientFactoryService httpClientFactoryService)
        {
            _settings = options.Value;
            _logger = logger;
            _httpClientFactoryService = httpClientFactoryService;
            _jsonSerializerSettings = CommonJsonSerializerSettings.GetJsonSerializerSettings();
        }

        /// <summary>
        /// Checks the Semantic Service returns a call to signal it is initialized and ready for requests.
        /// </summary>
        public bool IsInitialized => GetServiceStatus();

        /// <summary>
        /// Gets a completion from the Semantic Kernel service.
        /// </summary>
        /// <param name="request">Request object populated from the hub APIs including agent, prompt, data source, and model information.</param>
        /// <returns>Returns a completion response from the orchestration engine.</returns>
        public async Task<LLMOrchestrationCompletionResponse> GetCompletion(LLMOrchestrationCompletionRequest request)
        {
            var agentName = string.Empty;
            var promptTemplate = string.Empty;

            switch (request)
            {
                case KnowledgeManagementCompletionRequest kmcr:
                    agentName = kmcr.Agent.Name;
                    break;
                case LegacyOrchestrationCompletionRequest lcr:
                    agentName = lcr.Agent?.Name;
                    promptTemplate = lcr.Agent?.PromptPrefix;
                    break;
                default:
                    throw new Exception($"LLM orchestration completion request of type {request.GetType()} is not supported.");
            }

            var client = _httpClientFactoryService.CreateClient(Common.Constants.HttpClients.SemanticKernelAPI);

            var body = JsonConvert.SerializeObject(request, _jsonSerializerSettings);
            var responseMessage = await client.PostAsync("orchestration/completion",
                new StringContent(
                    body,
                    Encoding.UTF8, "application/json"));
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                var completionResponse = JsonConvert.DeserializeObject<LLMOrchestrationCompletionResponse>(responseContent);

                return new LLMOrchestrationCompletionResponse
                {
                    Completion = completionResponse!.Completion,
                    UserPrompt = completionResponse.UserPrompt,
                    FullPrompt = completionResponse.FullPrompt,
                    PromptTemplate = promptTemplate,
                    AgentName = agentName,
                    PromptTokens = completionResponse.PromptTokens,
                    CompletionTokens = completionResponse.CompletionTokens
                };
            }

            _logger.LogWarning($"The LangChain orchestration service returned status code {responseMessage.StatusCode}: {responseContent}");

            return new LLMOrchestrationCompletionResponse
            {
                Completion = "A problem on my side prevented me from responding.",
                UserPrompt = request.UserPrompt,
                PromptTemplate = promptTemplate,
                AgentName = agentName,
                PromptTokens = 0,
                CompletionTokens = 0
            };
        }

        /// <summary>
        /// Gets the target Semantic Kernel API status.
        /// </summary>
        /// <returns></returns>
        private bool GetServiceStatus()
        {
            var client = _httpClientFactoryService.CreateClient(Common.Constants.HttpClients.SemanticKernelAPI);
            var responseMessage = client.Send(
                new HttpRequestMessage(HttpMethod.Get, "status"));

            return responseMessage.Content.ToString() == "ready";
        }
    }
}
