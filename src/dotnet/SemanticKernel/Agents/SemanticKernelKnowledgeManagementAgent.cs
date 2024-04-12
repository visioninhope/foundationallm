using Azure.AI.OpenAI;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.SemanticKernel.Core.Exceptions;
using FoundationaLLM.SemanticKernel.Core.Filters;
using FoundationaLLM.SemanticKernel.Core.Plugins;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureAISearch;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Memory;

#pragma warning disable SKEXP0001, SKEXP0010, SKEXP0020, SKEXP0050, SKEXP0060

namespace FoundationaLLM.SemanticKernel.Core.Agents
{
    /// <summary>
    /// The Knowledge Management agent.
    /// </summary>
    /// <param name="request">The <see cref="KnowledgeManagementCompletionRequest"/> to be processed by the agent.</param>
    /// <param name="resourceProviderServices">A collection of <see cref="IResourceProviderService"/> resource providers.</param>
    /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to create loggers for logging.</param>
    public class SemanticKernelKnowledgeManagementAgent(
        LLMCompletionRequest request,
        IEnumerable<IResourceProviderService> resourceProviderServices,
        ILoggerFactory loggerFactory) : SemanticKernelAgentBase(request, resourceProviderServices, loggerFactory.CreateLogger<SemanticKernelKnowledgeManagementAgent>())
    {
        private readonly ILoggerFactory _loggerFactory = loggerFactory;

        protected override async Task<LLMCompletionResponse> BuildResponseWithAzureOpenAI()
        {
            try
            {
                var kernel = BuildKernel();

                // Use observability features to capture the fully rendered prompt.
                var promptFilter = new DefaultPromptFilter();
                kernel.PromptFilters.Add(promptFilter);

                var arguments = new KernelArguments()
                {
                    ["userPrompt"] = _request.UserPrompt!,
                    ["messageHistory"] = _request.MessageHistory
                };

                var result = await kernel.InvokePromptAsync(_prompt, arguments);

                var completion = result.GetValue<string>()!;
                var completionUsage = (result.Metadata!["Usage"] as CompletionsUsage)!;

                return new LLMCompletionResponse
                {
                    Completion = completion,
                    UserPrompt = _request.UserPrompt!,
                    FullPrompt = promptFilter.RenderedPrompt,
                    AgentName = _request.Agent.Name,
                    PromptTokens = completionUsage!.PromptTokens,
                    CompletionTokens = completionUsage.CompletionTokens,
                    TotalTokens = completionUsage.TotalTokens
                };
            }
            catch (Exception ex)
            {
                var message = "The response building process encountered an error.";
                _logger.LogError(ex, message);
                throw new SemanticKernelException(message, StatusCodes.Status500InternalServerError);
            }
        }

        private Kernel BuildKernel()
        {
            var credential = DefaultAuthentication.GetAzureCredential();

            var builder = Kernel.CreateBuilder();
            builder.Services.AddSingleton<ILoggerFactory>(_loggerFactory);
            builder.AddAzureOpenAIChatCompletion(
                _deploymentName,
                _endpoint,
                credential);
            var kernel = builder.Build();

            var memory = new MemoryBuilder()
                .WithMemoryStore(new AzureAISearchMemoryStore(_indexingEndpoint, credential))
                .WithAzureOpenAITextEmbeddingGeneration(_textEmbeddingDeploymentName, _textEmbeddingEndpoint, credential)
                .Build();

            kernel.ImportPluginFromObject(new KnowledgeManagementContextPlugin(memory, _indexName));

            return kernel;
        }
    }
}
