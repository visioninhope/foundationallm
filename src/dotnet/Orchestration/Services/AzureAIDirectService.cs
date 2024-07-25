using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Agents;
using FoundationaLLM.Common.Constants.Authentication;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Extensions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Infrastructure;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Orchestration.Direct;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Orchestration.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Orchestration.Core.Services
{
    /// <summary>
    /// The Azure AI direct orchestration service.
    /// </summary>
    /// <param name="callContext">The <see cref="ICallContext"/> providing details about the call context.</param>
    /// <param name="logger">The logger used for logging.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> used to retrieve app settings from configuration.</param>
    /// <param name="httpClientFactoryService">The HTTP client factory service.</param>
    /// <param name="resourceProviderServices">A dictionary of <see cref="IResourceProviderService"/> resource providers hashed by resource provider name.</param>
    public class AzureAIDirectService(
        ICallContext callContext,
        ILogger<AzureAIDirectService> logger,
        IConfiguration configuration,
        IHttpClientFactoryService httpClientFactoryService,
        IEnumerable<IResourceProviderService> resourceProviderServices) : IAzureAIDirectService
    {
        private readonly ICallContext _callContext = callContext;
        private readonly ILogger<AzureAIDirectService> _logger = logger;
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
        public string Name => LLMOrchestrationServiceNames.AzureAIDirect;

        /// <inheritdoc/>
        public async Task<LLMCompletionResponse> GetCompletion(string instanceId, LLMCompletionRequest request)
        {
            request.Validate();

            SystemCompletionMessage? systemPrompt = null;
            CompletionMessage? assistantPrompt = null;

            // We are adding an empty assistant prompt and setting the system prompt to the user role to support
            // some models (like Mistral) that require user/assistant prompts and not system prompts.
            var inputStrings = new List<CompletionMessage>()
            {
                new SystemCompletionMessage
                {
                    Role = InputMessageRoles.User,
                    Content = request.Prompt.Prefix ?? string.Empty
                },
                new CompletionMessage
                {
                    Role = InputMessageRoles.Assistant,
                    Content = string.Empty
                }
            };

            // Add conversation history.
            if (request.Agent.ConversationHistorySettings?.Enabled == true && request.MessageHistory != null)
            {
                // The message history needs to be in a continuous order of user and assistant messages.
                // If the MaxHistory value is odd, add one to the number of messages to take to ensure proper pairing.
                if (request.Agent.ConversationHistorySettings.MaxHistory % 2 != 0)
                    request.Agent.ConversationHistorySettings.MaxHistory++;

                var messageHistoryItems = request.MessageHistory?.TakeLast(request.Agent.ConversationHistorySettings.MaxHistory);
                
                foreach(var item in messageHistoryItems!)
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

            var endpointConfiguration = request.AIModelEndpointConfiguration;
            var apiKey = string.Empty;
            var apiKeyHeaderName = string.Empty;
            var apiKeyPrefix = string.Empty;

            if (endpointConfiguration.AuthenticationType == AuthenticationTypes.APIKey)
            {
                if (!endpointConfiguration.AuthenticationParameters.TryGetValue(AuthenticationParametersKeys.APIKeyConfigurationName, out var apiKeyKeyName))
                    throw new OrchestrationException($"The {AuthenticationParametersKeys.APIKeyConfigurationName} key is missing from the AI model enpoint's authentication parameters dictionary.");

                apiKey = _configuration.GetValue<string>(apiKeyKeyName?.ToString()!)!;
            }

            // The expected value for the header name is "Authorization".
            if (!endpointConfiguration.AuthenticationParameters.TryGetValue(AuthenticationParametersKeys.APIKeyHeaderName, out var apiKeyHeaderNameObject))
                throw new OrchestrationException($"The {AuthenticationParametersKeys.APIKeyHeaderName} key is missing from the AI model enpoint's authentication parameters dictionary.");
            apiKeyHeaderName = apiKeyHeaderNameObject.ToString();

            // The optional expected value for the API key prefix is "Bearer".
            if (endpointConfiguration.AuthenticationParameters.TryGetValue(AuthenticationParametersKeys.APIKeyPrefix, out var  apiKeyPrefixObject))
                apiKeyPrefix = apiKeyPrefixObject.ToString();

            if (!string.IsNullOrWhiteSpace(endpointConfiguration.Url)
                && !string.IsNullOrWhiteSpace(apiKey)
                && !string.IsNullOrWhiteSpace(apiKeyHeaderName))
            {
                var client = _httpClientFactoryService.CreateClient(HttpClients.AzureAIDirect);

                if (apiKeyHeaderName == HeaderNames.Authorization)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        apiKeyPrefix ?? string.Empty, apiKey
                    );
                }
                else
                {
                    client.DefaultRequestHeaders.Add(
                        apiKeyHeaderName,
                        string.IsNullOrWhiteSpace(apiKeyPrefix)
                            ? apiKey
                            : $"{apiKeyPrefix} {apiKey}");
                }
                
                client.BaseAddress = new Uri(endpointConfiguration.Url!);

                var modelParameters = request.AIModel.ModelParameters;

                AzureAICompletionRequest azureAiCompletionRequest = new()
                {
                    InputData = new()
                    {
                        InputString = [.. inputStrings],
                        Parameters = modelParameters.ToObject<AzureAICompletionParameters>()
                    }
                };

                var body = JsonSerializer.Serialize(azureAiCompletionRequest, _jsonSerializerOptions);
                var content = new StringContent(body, Encoding.UTF8, "application/json");

                var responseMessage = await client.PostAsync("", content);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.IsSuccessStatusCode)
                {
                    var completionResponse = JsonSerializer.Deserialize<AzureAICompletionResponse>(responseContent);

                    return new LLMCompletionResponse
                    {
                        OperationId = request.OperationId,
                        Completion = completionResponse!.Output,
                        UserPrompt = request.UserPrompt,
                        FullPrompt = body,
                        PromptTemplate = systemPrompt?.Content,
                        AgentName = request.Agent.Name,
                        PromptTokens = 0,
                        CompletionTokens = 0
                    };
                }

                _logger.LogWarning("The AzureAIDirect orchestration service returned status code {StatusCode}: {ResponseContent}",
                    responseMessage.StatusCode, responseContent);
            }

            return new LLMCompletionResponse
            {
                OperationId = request.OperationId,
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
