using FoundationaLLM.AgentFactory.Core.Interfaces;
using FoundationaLLM.AgentFactory.Core.Models.ConfigurationOptions;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Agents;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Prompt.Models.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoundationaLLM.AgentFactory.Core.Services
{
    /// <summary>
    /// The Azure AI direct orchestration service.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="httpClientFactoryService"></param>
    public class AzureAIDirectService(
        IOptions<AzureAIDirectServiceSettings> options,
        ILogger<AzureAIDirectService> logger,
        IHttpClientFactoryService httpClientFactoryService) : IAzureAIDirectService
    {
        readonly AzureAIDirectServiceSettings _settings = options.Value;
        readonly ILogger<AzureAIDirectService> _logger = logger;
        private readonly IHttpClientFactoryService _httpClientFactoryService = httpClientFactoryService;
        readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();
        //readonly Dictionary<string, IResourceProviderService> _resourceProviderServices = new Dictionary<string, IResourceProviderService>();

        /// <inheritdoc/>
        public bool IsInitialized => true;

        /// <inheritdoc/>
        public async Task<LLMCompletionResponse> GetCompletion(LLMCompletionRequest request)
        {
            AgentBase? agent = request switch
            {
                KnowledgeManagementCompletionRequest kmcr => kmcr.Agent,
                InternalContextCompletionRequest icr => icr.Agent,
                _ => null
            };
            if (agent == null) throw new Exception("Agent cannot be null.");

            var endpointConfiguration = agent.OrchestrationSettings?.EndpointConfiguration;
            if (endpointConfiguration == null) throw new Exception("Endpoint Configuration must be provided.");

            endpointConfiguration.TryGetValue(EndpointConfigurationKeys.Endpoint, out var endpoint);
            endpointConfiguration.TryGetValue(EndpointConfigurationKeys.APIKey, out var apiKey);

            //if (!_resourceProviderServices.TryGetValue(ResourceProviderNames.FoundationaLLM_Prompt, out var promptResourceProvider))
            //    throw new ResourceProviderException($"The resource provider {ResourceProviderNames.FoundationaLLM_Prompt} was not loaded.");

            //var systemPrompt = await promptResourceProvider.GetResourceAsync<MultipartPrompt>(agent.PromptObjectId!);

            if (endpoint != null && apiKey != null)
            {
                var client = _httpClientFactoryService.CreateClient(Common.Constants.HttpClients.AzureAIDirect);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", apiKey.ToString()
                );

                var modelParameters = agent.OrchestrationSettings?.ModelParameters;
                AzureAIDirectRequest azureAIDirectRequest;

                if (modelParameters != null)
                {
                    azureAIDirectRequest = new()
                    {
                        InputData = new()
                        {
                            InputString =
                            [
                                new InputString { Role = "user", Content = request.UserPrompt }
                            ],
                            Parameters = new Parameters
                            {
                                Temperature = Convert.ToSingle(modelParameters.GetValueOrDefault(ModelParameterKeys.Temperature, 0.0f).ToString())
                            }
                        }
                    };

                    var body = JsonSerializer.Serialize(azureAIDirectRequest, _jsonSerializerOptions);

                    var responseMessage = await client.PostAsync(
                        endpoint.ToString(),
                        new StringContent(body, Encoding.UTF8, "application/json")
                    );
                    var responseContent = await responseMessage.Content.ReadAsStringAsync();

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var completionResponse = JsonSerializer.Deserialize<AzureAIDirectResponse>(responseContent);

                        return new LLMCompletionResponse
                        {
                            Completion = completionResponse!.Output,
                            UserPrompt = request.UserPrompt,
                            FullPrompt = body,
                            PromptTemplate = null,
                            AgentName = agent.Name,
                            PromptTokens = 0,
                            CompletionTokens = 0
                        };
                    }

                    _logger.LogWarning("The LangChain orchestration service returned status code {StatusCode}: {ResponseContent}",
                        responseMessage.StatusCode, responseContent);
                }
            }

            return new LLMCompletionResponse
            {
                Completion = "A problem on my side prevented me from responding.",
                UserPrompt = request.UserPrompt,
                PromptTemplate = null,
                AgentName = agent.Name,
                PromptTokens = 0,
                CompletionTokens = 0
            };
        }
    }
}
