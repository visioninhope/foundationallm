using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.AIModel;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using FoundationaLLM.Common.Models.ResourceProviders.Prompt;
using FoundationaLLM.SemanticKernel.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System.Text.Json;

namespace FoundationaLLM.SemanticKernel.Core.Agents
{
    /// <summary>
    /// Implements the base functionality for a Semantic Kernel agent.
    /// </summary>
    /// <param name="request">The <see cref="LLMCompletionRequest"/> being processed by the agent.</param>
    /// <param name="resourceProviderServices">A collection of <see cref="IResourceProviderService"/> resource providers.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public class SemanticKernelAgentBase(
        LLMCompletionRequest request,
        IEnumerable<IResourceProviderService> resourceProviderServices,
        ILogger logger)
    {
        protected readonly IResourceProviderService _configurationResourceProvider =
            resourceProviderServices.Single(rp => rp.Name == ResourceProviderNames.FoundationaLLM_Configuration);

        protected readonly LLMCompletionRequest _request = request;
        protected readonly ILogger _logger = logger;

        protected string _llmProvider = string.Empty;
        protected string _endpoint = string.Empty;
        protected string _deploymentName = string.Empty;

        /// <summary>
        /// Gets the completion for the request.
        /// </summary>
        /// <returns>A <see cref="LLMCompletionResponse"/> object containing the completion response.</returns>
        public async Task<LLMCompletionResponse> GetCompletion()
        {
            ValidateRequest();
            await ExpandAndValidateAgent();

            return _llmProvider switch
            {
                APIEndpointProviders.MICROSOFT => await BuildResponseWithAzureOpenAI(),
                APIEndpointProviders.OPENAI => await BuildResponseWithOpenAI(),
                _ => throw new SemanticKernelException($"The LLM provider '{_llmProvider}' is not supported.")
            };
        }

        /// <summary>
        /// Builds the completion response using Azure OpenAI.
        /// </summary>
        /// <returns>A <see cref="LLMCompletionResponse"/> object containing the completion response.</returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual async Task<LLMCompletionResponse> BuildResponseWithAzureOpenAI()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Builds the completion response using OpenAI.
        /// </summary>
        /// <returns>A <see cref="LLMCompletionResponse"/> object containing the completion response.</returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual async Task<LLMCompletionResponse> BuildResponseWithOpenAI()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves the agent-specific details from the agent properties payload.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task ExpandAndValidateAgent()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        protected async Task<string> GetConfigurationValue(string configName) =>
            (await GetResource<AppConfigurationKeyBase>(
                configName,
                ConfigurationResourceTypeNames.AppConfigurations,
                _configurationResourceProvider)).Value!;

        private async Task<T> GetResource<T>(string objectId, string resourceTypeName, IResourceProviderService resourceProviderService)
            where T : ResourceBase
        {
            var result = await resourceProviderService.HandleGetAsync(
                $"/{resourceTypeName}/{objectId.Split("/").Last()}", new UnifiedUserIdentity
                {
                    Name = "SemanticKernelAPI",
                    UserId = "SemanticKernelAPI",
                    Username = "SemanticKernelAPI"
                });
            var resource = (result as List<ResourceProviderGetResult<T>>)!.First().Resource;
            return resource;
        }

        private void ValidateRequest()
        {
            if (_request.Agent == null)
                throw new SemanticKernelException("The Agent property of the completion request cannot be null.", StatusCodes.Status400BadRequest);

            if (_request.Agent.OrchestrationSettings == null)
                throw new SemanticKernelException("The OrchestrationSettings property of the agent cannot be null.", StatusCodes.Status400BadRequest);

            if (_request.Objects == null)
                throw new SemanticKernelException("The Objects property of the completion request cannot be null.", StatusCodes.Status400BadRequest);

            if (string.IsNullOrWhiteSpace(_request.Agent.AIModelObjectId))
                throw new SemanticKernelException("Invalid AI model object id.", StatusCodes.Status400BadRequest);

            if (!_request.Objects.TryGetValue(
                    _request.Agent.AIModelObjectId, out var aiModelObject))
                throw new SemanticKernelException("The AI model object is missing from the request's objects.", StatusCodes.Status400BadRequest);

            var aiModel = aiModelObject is JsonElement aiModelJsonElement
                ? aiModelJsonElement.Deserialize<AIModelBase>()
                : aiModelObject as AIModelBase;

            if (aiModel == null
                || string.IsNullOrWhiteSpace(aiModel.EndpointObjectId)
                || string.IsNullOrWhiteSpace(aiModel.DeploymentName)
                || aiModel.ModelParameters == null)
                throw new SemanticKernelException("The AI model object provided in the request's objects is invalid.", StatusCodes.Status400BadRequest);

            if (!_request.Objects.TryGetValue(
                    aiModel.EndpointObjectId, out var endpointObject))
                throw new SemanticKernelException("The API endpoint configuration object is missing from the request's objects.", StatusCodes.Status400BadRequest);

            var endpoint = endpointObject is JsonElement endpointJsonElement
                ? endpointJsonElement.Deserialize<APIEndpointConfiguration>()
                : endpointObject as APIEndpointConfiguration;

            if (endpoint == null
                || string.IsNullOrWhiteSpace(endpoint.Provider)
                || !APIEndpointProviders.All.Contains(endpoint.Provider)
                || string.IsNullOrWhiteSpace(endpoint.Url))
                throw new SemanticKernelException("The API endpoint configuration object provided in the requets's objects is invalid.", StatusCodes.Status400BadRequest);

            _llmProvider = endpoint.Provider!;
            _endpoint = endpoint.Url!;
            _deploymentName = aiModel.DeploymentName!;
        }
    }
}
