using FoundationaLLM.Common.Constants.Agents;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Agents;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.ResourceProvider;
using FoundationaLLM.Configuration.Models;
using FoundationaLLM.Prompt.Models.Metadata;
using FoundationaLLM.SemanticKernel.Core.Exceptions;
using FoundationaLLM.Vectorization.Models.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

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
        protected readonly IResourceProviderService _promptResourceProvider =
            resourceProviderServices.Single(rp => rp.Name == ResourceProviderNames.FoundationaLLM_Prompt);
        protected readonly IResourceProviderService _vectorizationResourceProvider =
            resourceProviderServices.Single(rp => rp.Name == ResourceProviderNames.FoundationaLLM_Vectorization);

        protected readonly LLMCompletionRequest _request = request;
        protected readonly ILogger _logger = logger;

        protected string _llmProvider = string.Empty;
        protected string _endpoint = string.Empty;
        protected string _deploymentName = string.Empty;

        protected string _textEmbeddingDeploymentName = string.Empty;
        protected string _textEmbeddingEndpoint = string.Empty;
        protected string _indexerName = string.Empty;
        protected string _indexingEndpoint = string.Empty;
        protected string _indexName = string.Empty;
        protected string _prompt = string.Empty;


        /// <summary>
        /// Gets the completion for the request.
        /// </summary>
        /// <returns>A <see cref="LLMCompletionResponse"/> object containing the completion response.</returns>
        public async Task<LLMCompletionResponse> GetCompletion()
        {
            ValidateRequest();
            await ExpandRequest();

            return _llmProvider switch
            {
                LanguageModelProviders.MICROSOFT => await BuildResponseWithAzureOpenAI(),
                LanguageModelProviders.OPENAI => await BuildResponseWithOpenAI(),
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

        private async Task ExpandRequest()
        {
            var textEmbeddingProfile = await GetResource<VectorizationProfileBase>(
                (_request.Agent as KnowledgeManagementAgent)!.Vectorization.TextEmbeddingProfileObjectId!,
                VectorizationResourceTypeNames.TextEmbeddingProfiles,
                _vectorizationResourceProvider);

            _textEmbeddingDeploymentName = await GetConfigurationValue(
                textEmbeddingProfile.ConfigurationReferences!["DeploymentName"]);

            _textEmbeddingEndpoint = await GetConfigurationValue(
                textEmbeddingProfile.ConfigurationReferences["Endpoint"]);

            var indexingProfile = await GetResource<VectorizationProfileBase>(
                (_request.Agent as KnowledgeManagementAgent)!.Vectorization.IndexingProfileObjectId!,
                VectorizationResourceTypeNames.IndexingProfiles,
                _vectorizationResourceProvider);

            _indexerName = (indexingProfile as IndexingProfile)!.Indexer.ToString();
            _indexName = indexingProfile.Settings!["IndexName"];
            _indexingEndpoint = await GetConfigurationValue(
                indexingProfile.ConfigurationReferences!["Endpoint"]);

            var prompt = await GetResource<PromptBase>(
                _request.Agent.PromptObjectId!,
                PromptResourceTypeNames.Prompts,
                _promptResourceProvider);

            _prompt = (prompt as MultipartPrompt)!.Prefix!;
        }

        private async Task<string> GetConfigurationValue(string configName) =>
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
            return (result as List<T>)!.First();
        }

        private void ValidateRequest()
        {
            if (_request.Agent == null)
                throw new SemanticKernelException("The Agent property of the completion request cannot be null.");

            if (_request.Agent.OrchestrationSettings == null)
                throw new SemanticKernelException("The OrchestrationSettings property of the agent cannot be null.");

            if (_request.Agent.OrchestrationSettings.EndpointConfiguration == null)
                throw new SemanticKernelException("The EndpointConfiguration property of the agent's OrchestrationSettings property cannot be null.");

            if (!_request.Agent.OrchestrationSettings.EndpointConfiguration.TryGetValue(EndpointConfigurationKeys.Provider, out var llmProvider)
                || string.IsNullOrWhiteSpace(llmProvider.ToString()))
                throw new SemanticKernelException("The Provider property of the agent's OrchestrationSettings.EndpointConfiguration property cannot be null.");

            if (!LanguageModelProviders.All.Contains(llmProvider.ToString()))
                throw new SemanticKernelException($"The LLM provider '{llmProvider}' is not supported.");

            if (!_request.Agent.OrchestrationSettings.EndpointConfiguration.TryGetValue(EndpointConfigurationKeys.Endpoint, out var endpoint)
                || string.IsNullOrWhiteSpace(endpoint.ToString()))
                throw new SemanticKernelException("The Endpoint property of the agent's OrchestrationSettings.EndpointConfiguration property cannot be null.");

            if (_request.Agent.OrchestrationSettings.ModelParameters == null)
                throw new SemanticKernelException("The ModelParameters property of the agent's OrchestrationSettings property cannot be null.");

            if (!_request.Agent.OrchestrationSettings.ModelParameters.TryGetValue(ModelParameterKeys.DeploymentName, out var deploymentName)
                || string.IsNullOrWhiteSpace(deploymentName.ToString()))
                throw new SemanticKernelException("The DeploymentName property of the agent's OrchestrationSettings.ModelParameters property cannot be null.");

            _llmProvider = llmProvider.ToString()!;
            _endpoint = endpoint.ToString()!;
            _deploymentName = deploymentName.ToString()!;
        }
    }
}
