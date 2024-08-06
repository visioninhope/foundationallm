using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.SemanticKernel.Core.Agents;
using FoundationaLLM.SemanticKernel.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundationaLLM.SemanticKernel.Core.Services
{
    /// <summary>
    /// Processes requests targeting the Semantic Kernel agents.
    /// </summary>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to build loggers for logging.</param>
    /// <param name="callContext">Stores context information extracted from the current HTTP request. This information
    /// is primarily used to inject HTTP headers into downstream HTTP calls.</param>
    /// <param name="resourceProviderServices">A collection of <see cref="IResourceProviderService"/> resource providers.</param>
    /// <param name="httpClientFactoryService">The HTTP client factory service.</param>
    public class SemanticKernelService(
        ILoggerFactory loggerFactory,
        ICallContext callContext,
        IEnumerable<IResourceProviderService> resourceProviderServices,
        IHttpClientFactoryService httpClientFactoryService) : ISemanticKernelService
    {
        private readonly ICallContext _callContext = callContext;
        private readonly IEnumerable<IResourceProviderService> _resourceProviderServices = resourceProviderServices;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly IHttpClientFactoryService _httpClientFactoryService = httpClientFactoryService;

        private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions(
            options =>
            {
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return options;
            });

        /// <inheritdoc/>
        public async Task<LLMCompletionResponse> GetCompletion(LLMCompletionRequest request) => request.Agent switch
        {
            KnowledgeManagementAgent => await (new SemanticKernelKnowledgeManagementAgent(
                request,
                _resourceProviderServices,
                _loggerFactory,
                _httpClientFactoryService)).GetCompletion(),
            _ => throw new Exception($"The agent type {request.Agent.GetType()} is not supported.")
        };

        /// <inheritdoc/>
        public async Task<LongRunningOperation> StartCompletionOperation(string instanceId, LLMCompletionRequest completionRequest)
        {
            var fallback = new LongRunningOperation
            {
                OperationId = completionRequest.OperationId,
                Status = OperationStatus.Failed
            };

            var client = await _httpClientFactoryService.CreateClient(HttpClientNames.StateAPI, _callContext.CurrentUserIdentity);

            var response = await client.PostAsync($"instances/{instanceId}/operations/{completionRequest.OperationId}", null);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var operation = JsonSerializer.Deserialize<LongRunningOperation>(responseContent, _jsonSerializerOptions)!;

                /// TODO perform this tasks in background and return the operation immediately
                await Task.Run(async () =>
                {
                    operation.Status = OperationStatus.InProgress;
                    operation.StatusMessage = "Operation state changed to in progress.";

                    _ = client.PutAsync(
                        $"instances/{instanceId}/operations/{completionRequest.OperationId}",
                        new StringContent(JsonSerializer.Serialize(operation)));

                    try
                    {
                        var completion = await GetCompletion(completionRequest);

                        operation.Status = OperationStatus.Completed;
                        operation.StatusMessage = $"Operation {completionRequest.OperationId} completed successfully.";

                        _ = await client.PutAsync(
                            $"instances/{instanceId}/operations/{completionRequest.OperationId}",
                            new StringContent(JsonSerializer.Serialize(operation)));

                        _ = await client.PutAsync(
                            $"instances/{instanceId}/operations/{completionRequest.OperationId}/result",
                            new StringContent(JsonSerializer.Serialize(completion)));
                    }
                    catch (Exception ex)
                    {
                        operation.Status = OperationStatus.Failed;
                        operation.StatusMessage = $"Operation failed with error: {ex}";

                        _ = await client.PutAsync(
                            $"instances/{instanceId}/operations/{completionRequest.OperationId}",
                            new StringContent(JsonSerializer.Serialize(operation)));

                        _ = await client.PutAsync(
                            $"instances/{instanceId}/operations/{completionRequest.OperationId}/result",
                            new StringContent(JsonSerializer.Serialize(new LLMCompletionResponse()
                            {
                                OperationId = operation.OperationId!,
                                UserPrompt = completionRequest.UserPrompt,
                                Completion = $"Operation failed with error: {ex}"
                            })));
                    }
                });

                return operation;
            }

            return fallback;
        }

        /// <inheritdoc/>
        public async Task<LongRunningOperation> GetCompletionOperationStatus(string instanceId, string operationId)
        {
            var fallback = new LongRunningOperation
            {
                OperationId = operationId,
                Status = OperationStatus.Failed
            };

            var client = await _httpClientFactoryService.CreateClient(HttpClientNames.StateAPI, _callContext.CurrentUserIdentity);

            var response = await client.GetAsync($"instances/{instanceId}/operations/{operationId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var operation = JsonSerializer.Deserialize<LongRunningOperation>(responseContent, _jsonSerializerOptions)!;

                return operation;
            }

            return fallback;
        }

        /// <inheritdoc/>
        public async Task<LLMCompletionResponse> GetCompletionOperationResult(string instanceId, string operationId)
        {
            var fallback = new LLMCompletionResponse
            {
                OperationId = operationId
            };

            var client = await _httpClientFactoryService.CreateClient(HttpClientNames.StateAPI, _callContext.CurrentUserIdentity);

            var response = await client.GetAsync($"instances/{instanceId}/operations/{operationId}/result");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var completionResponse = JsonSerializer.Deserialize<LLMCompletionResponse>(responseContent, _jsonSerializerOptions)!;

                return completionResponse;
            }

            return fallback;
        }
    }
}
