using AngleSharp.Dom.Events;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Agents;
using FoundationaLLM.Common.Constants.Authentication;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Infrastructure;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Orchestration.Direct;
using FoundationaLLM.Common.Models.ResourceProviders.Prompt;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Orchestration.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Orchestration.Core.Services
{
    /// <summary>
    /// The Azure OpenAI direct orchestration service.
    /// </summary>
    /// <param name="callContext">The <see cref="ICallContext"/> providing details about the call context.</param>
    /// <param name="logger">The logger used for logging.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> used to retrieve app settings from configuration.</param>
    /// <param name="httpClientFactoryService">The HTTP client factory service.</param>
    /// <param name="resourceProviderServices">A dictionary of <see cref="IResourceProviderService"/> resource providers hashed by resource provider name.</param>
    public class AzureOpenAIDirectService(
        ICallContext callContext,
        ILogger<AzureOpenAIDirectService> logger,
        IConfiguration configuration,
        IHttpClientFactoryService httpClientFactoryService,
        IEnumerable<IResourceProviderService> resourceProviderServices) : IAzureOpenAIDirectService
    {
        private readonly ICallContext _callContext = callContext;
        private readonly ILogger<AzureOpenAIDirectService> _logger = logger;
        private readonly IConfiguration _configuration = configuration;
        private readonly IHttpClientFactoryService _httpClientFactoryService = httpClientFactoryService;
        private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();
        private readonly Dictionary<string, IResourceProviderService> _resourceProviderServices = resourceProviderServices.ToDictionary(
                rps => rps.Name);

        /// <inheritdoc/>
        public async Task<ServiceStatusInfo> GetStatus(string instanceId) =>
            await Task.FromResult(new ServiceStatusInfo
            {
                InstanceId = instanceId,
                Name = Name,
                Status = "ready",
            });

        /// <inheritdoc/>
        public string Name => LLMOrchestrationServiceNames.AzureOpenAIDirect;

        /// <inheritdoc/>
        public async Task<LLMCompletionResponse> GetCompletion(string instanceId, LLMCompletionRequest request)
        {
            request.Validate();

            var endpointConfiguration = request.AIModelEndpointConfiguration;
            var inputStrings = new List<CompletionMessage>();
            SystemCompletionMessage? systemPrompt = null;

            if (!string.IsNullOrWhiteSpace(endpointConfiguration.OperationType)
                && endpointConfiguration.OperationType == OperationTypes.Chat)
            {
                inputStrings.Add(new SystemCompletionMessage
                {
                    Role = InputMessageRoles.System,
                    Content = request.Prompt.Prefix ?? string.Empty
                });

                // Add conversation history.
                if (request.Agent.ConversationHistorySettings?.Enabled == true && request.MessageHistory != null)
                {
                    // The message history needs to be in a continuous order of user and assistant messages.
                    // If the MaxHistory value is odd, add one to the number of messages to take to ensure proper pairing.
                    if (request.Agent.ConversationHistorySettings.MaxHistory % 2 != 0)
                        request.Agent.ConversationHistorySettings.MaxHistory++;

                    var messageHistoryItems = request.MessageHistory?.TakeLast(request.Agent.ConversationHistorySettings.MaxHistory);
                    foreach (var item in messageHistoryItems!)
                    {
                        inputStrings.Add(new CompletionMessage
                        {
                            Role = item.Sender.ToLower(),
                            Content = item.Text
                        });
                    }
                }
                // Add current user prompt.
                var userPrompt = new UserCompletionMessage { Content = request.UserPrompt };
                inputStrings.Add(userPrompt);
            }

            var apiKey = string.Empty;
            var apiKeyHeaderName = string.Empty;

            if (endpointConfiguration.AuthenticationType == AuthenticationTypes.APIKey)
            {
                if (!endpointConfiguration.AuthenticationParameters.TryGetValue(
                    AuthenticationParametersKeys.APIKeyConfigurationName, out var apiKeyKeyName))
                    throw new OrchestrationException($"The {AuthenticationParametersKeys.APIKeyConfigurationName} key is missing from the AI model enpoint's authentication parameters dictionary.");

                apiKey = _configuration.GetValue<string>(apiKeyKeyName?.ToString()!)!;
            }

            // The expected value for the header name is "api-key".
            if (!endpointConfiguration.AuthenticationParameters.TryGetValue(AuthenticationParametersKeys.APIKeyHeaderName, out var apiKeyHeaderNameObject))
                throw new OrchestrationException($"The {AuthenticationParametersKeys.APIKeyHeaderName} key is missing from the AI model enpoint's authentication parameters dictionary.");
            apiKeyHeaderName = apiKeyHeaderNameObject.ToString();

            if (!string.IsNullOrWhiteSpace(endpointConfiguration.Url)
                && !string.IsNullOrWhiteSpace(apiKey)
                && !string.IsNullOrWhiteSpace(request.AIModel.DeploymentName)
                && !string.IsNullOrWhiteSpace(apiKeyHeaderName))
            {
                var client = _httpClientFactoryService.CreateClient(HttpClients.AzureOpenAIDirect);
                if (endpointConfiguration.AuthenticationType == AuthenticationTypes.APIKey && !string.IsNullOrWhiteSpace(apiKey))
                {
                    client.DefaultRequestHeaders.Add(apiKeyHeaderName, apiKey);
                }

                client.BaseAddress = new Uri(endpointConfiguration.Url);

                var modelParameters = request.AIModel.ModelParameters;

                var azureOpenAIDirectRequest = modelParameters.ToObject<AzureOpenAICompletionRequest>();
                var chatOperation = string.Empty;

                switch (endpointConfiguration.OperationType)
                {
                    case OperationTypes.Completions:
                        azureOpenAIDirectRequest.Prompt = request.UserPrompt;
                        break;
                    case OperationTypes.Chat:
                        chatOperation = "/chat";
                        azureOpenAIDirectRequest.Messages = [.. inputStrings];
                        break;
                }

                var body = JsonSerializer.Serialize(azureOpenAIDirectRequest, _jsonSerializerOptions);
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                var responseMessage = await client.PostAsync($"/openai/deployments/{request.AIModel.DeploymentName}{chatOperation}/completions?api-version={endpointConfiguration.APIVersion}", content);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.IsSuccessStatusCode)
                {
                    var completionResponse = JsonSerializer.Deserialize<AzureOpenAICompletionResponse>(responseContent);

                    return new LLMCompletionResponse
                    {
                        Completion = !string.IsNullOrEmpty(endpointConfiguration.OperationType) && endpointConfiguration.OperationType == OperationTypes.Chat
                            ? completionResponse!.Choices?[0].Message?.Content
                            : completionResponse!.Choices?[0].Text,
                        UserPrompt = request.UserPrompt,
                        FullPrompt = body,
                        PromptTemplate = systemPrompt?.Content,
                        AgentName = request.Agent.Name,
                        PromptTokens = completionResponse!.Usage!.PromptTokens,
                        CompletionTokens = completionResponse!.Usage!.CompletionTokens
                    };
                }

                _logger.LogWarning("The AzureOpenAIDirect orchestration service returned status code {StatusCode}: {ResponseContent}",
                    responseMessage.StatusCode, responseContent);
            }

            return new LLMCompletionResponse
            {
                Completion = "A problem on my side prevented me from responding.",
                UserPrompt = request.UserPrompt,
                PromptTemplate = systemPrompt?.Content,
                AgentName = request.Agent.Name,
                PromptTokens = 0,
                CompletionTokens = 0
            };
        }
    }
}
