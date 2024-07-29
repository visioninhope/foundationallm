using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.TableRowOperationResultWithKey;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Common.Services.API
{
    /// <summary>
    /// Contains methods for interacting with the downstream API.
    /// </summary>
    /// <param name="downstreamHttpClientName">The name of the downstream HTTP client.</param>
    /// <param name="callContext">Stores context information extracted from the current HTTP request. This information
    /// is primarily used to inject HTTP headers into downstream HTTP calls.</param>
    /// <param name="httpClientFactoryService">The HTTP client factory service.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public class DownstreamAPIService(
        string downstreamHttpClientName,
        ICallContext callContext,
        IHttpClientFactoryService httpClientFactoryService,
        ILogger<DownstreamAPIService> logger) : IDownstreamAPIService
    {
        private readonly string _downstreamHttpClientName = downstreamHttpClientName;
        private readonly ICallContext _callContext = callContext;
        private readonly IHttpClientFactoryService _httpClientFactoryService = httpClientFactoryService;
        private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();
        private readonly ILogger<DownstreamAPIService> _logger = logger;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string APIName => _downstreamHttpClientName;

        /// <inheritdoc/>
        public async Task<CompletionResponse> GetCompletion(string instanceId, CompletionRequest completionRequest)
        {
            var fallback = new CompletionResponse
            {
                OperationId = completionRequest.OperationId,
                Completion = "A problem on my side prevented me from responding.",
                UserPrompt = completionRequest.UserPrompt ?? string.Empty,
                PromptTokens = 0,
                CompletionTokens = 0,
                UserPromptEmbedding = [ 0f ]
            };

            var client = await _httpClientFactoryService.CreateClient(_downstreamHttpClientName, _callContext.CurrentUserIdentity);

            _logger.LogInformation(
                "Created Http client {ClientName} with timeout {Timeout} seconds.",
                _downstreamHttpClientName,
                (int)client.Timeout.TotalSeconds);

            var serializedRequest = JsonSerializer.Serialize(completionRequest, _jsonSerializerOptions);
            var responseMessage = await client.PostAsync($"instances/{instanceId}/completions",
                new StringContent(
                    serializedRequest,
                        Encoding.UTF8, "application/json"));

            _logger.LogInformation(
                "Http client {ClientName} returned a response with status code {HttpStatusCode}.",
                _downstreamHttpClientName,
                responseMessage.StatusCode);

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var completionResponse = JsonSerializer.Deserialize<CompletionResponse>(responseContent);

                return completionResponse ?? fallback;
            }

            return fallback;
        }

        /// <inheritdoc/>
        public async Task<LongRunningOperation> StartCompletionOperation(string instanceId, CompletionRequest completionRequest)
        {
            var operationId = completionRequest.OperationId;

            var client = await _httpClientFactoryService.CreateClient(_downstreamHttpClientName, _callContext.CurrentUserIdentity);

            _logger.LogInformation(
                "Created Http client {ClientName} with timeout {Timeout} seconds.",
                _downstreamHttpClientName,
                (int)client.Timeout.TotalSeconds);
           
            var serializedRequest = JsonSerializer.Serialize(completionRequest, _jsonSerializerOptions);
            var responseMessage = await client.PostAsync($"instances/{instanceId}/async-completions",
                new StringContent(
                    serializedRequest,
                        Encoding.UTF8, "application/json"));

            _logger.LogInformation(
                "Http client {ClientName} returned a response with status code {HttpStatusCode}.",
                _downstreamHttpClientName,
                responseMessage.StatusCode);

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var completionResponse = JsonSerializer.Deserialize<LongRunningOperation>(responseContent);

                if (completionResponse is not null)
                {
                    return completionResponse;
                }                
            }
            _logger.LogError(
                "Failed to start the completion operation. {ClientName} service returned an invalid response for OperationID {OperationId}.",
                _downstreamHttpClientName,
                operationId);

            return new LongRunningOperation() { Status = OperationStatus.Failed, StatusMessage="Failed to create the long running operation." };            
        }

        /// <inheritdoc/>
        public async Task<LongRunningOperation> GetCompletionOperationStatus(string instanceId, string operationId)
        {
            var client = await _httpClientFactoryService.CreateClient(_downstreamHttpClientName, _callContext.CurrentUserIdentity);

            _logger.LogInformation(
                "Created Http client {ClientName} with timeout {Timeout} seconds.",
                _downstreamHttpClientName,
                (int)client.Timeout.TotalSeconds);


            var responseMessage = await client.GetAsync($"instances/{instanceId}/async-completions/{operationId}/status");

            _logger.LogInformation(
                "Http client {ClientName} returned a response with status code {HttpStatusCode}.",
                _downstreamHttpClientName,
                responseMessage.StatusCode);

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var completionResponse = JsonSerializer.Deserialize<LongRunningOperation>(responseContent);

                if (completionResponse is not null)
                {
                    return completionResponse;
                }
            }
            _logger.LogError(
                "Failed to retrieve the operation state for OperationID {OperationId}. {ClientName} service returned an invalid response.",
                operationId,
                _downstreamHttpClientName);
            return new LongRunningOperation() { Status = OperationStatus.Failed, StatusMessage = "Failed to retrieve the status of the long running operation." };
        }

        /// <inheritdoc/>
        public async Task<CompletionResponse> GetCompletionOperationResult(string instanceId, string operationId)
        {
            var fallback = new CompletionResponse
            {
                OperationId = operationId,
                Completion = "A problem on my side prevented me from responding.",
                UserPrompt = string.Empty,
                PromptTokens = 0,
                CompletionTokens = 0,
                UserPromptEmbedding = [0f]
            };

            var client = await _httpClientFactoryService.CreateClient(_downstreamHttpClientName, _callContext.CurrentUserIdentity);

            _logger.LogInformation(
                "Created Http client {ClientName} with timeout {Timeout} seconds.",
                _downstreamHttpClientName,
                (int)client.Timeout.TotalSeconds);

           
            var responseMessage = await client.GetAsync($"instances/{instanceId}/async-completions/{operationId}/result");

            _logger.LogInformation(
                "Http client {ClientName} returned a response with status code {HttpStatusCode}.",
                _downstreamHttpClientName,
                responseMessage.StatusCode);

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var completionResponse = JsonSerializer.Deserialize<CompletionResponse>(responseContent);

                return completionResponse ?? fallback;
            }

            return fallback;
        }
    }
}
