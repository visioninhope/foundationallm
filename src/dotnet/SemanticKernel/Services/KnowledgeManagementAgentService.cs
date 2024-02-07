using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Metadata;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.SemanticKernel.Core.Interfaces;
using FoundationaLLM.SemanticKernel.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace FoundationaLLM.SemanticKernel.Core.Services
{
    public class KnowledgeManagementAgentService : IKnowledgeManagementAgentService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public KnowledgeManagementAgentService(
            ILogger<KnowledgeManagementAgentService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<LLMOrchestrationCompletionResponse> GetCompletion(KnowledgeManagementCompletionRequest request)
        {
            var kernel = CreateKernel(request.Agent.LanguageModel!);

            //var agentPrompt = ResourceProviderService.GetAgentPrompt(request.Agent.Prompt);
            var agentPrompt = "";

            if (request.Agent.IndexingProfile != null && request.Agent.EmbeddingProfile != null)
            {
                // TODO
            }

            if (request.Agent.ConversationHistory != null)
            {
                if (request.Agent.ConversationHistory.Enabled)
                {
                    // TODO
                }
            }

            var modelVersion = _configuration.GetValue<string>(request.Agent.LanguageModel!.Version!);

            //var function = kernel.CreateFunctionFromPrompt(agentPrompt,
            //    new OpenAIPromptExecutionSettings()
            //    {
            //        Temperature = request.Agent.LanguageModel!.Temperature,
            //        ModelId = modelVersion
            //    });
            //var result = await kernel.InvokeAsync(function, new() { ["input"] = request.UserPrompt });

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
            var result = await chatCompletionService.GetChatMessageContentAsync(request.UserPrompt, new PromptExecutionSettings() { ModelId = modelVersion });

            return new LLMOrchestrationCompletionResponse() { Completion = result.Content };
        }

        private Kernel CreateKernel(LanguageModel llm)
        {
            var deploymentName = _configuration.GetValue<string>(llm.Deployment!);
            var endpoint = _configuration.GetValue<string>(llm.ApiEndpoint!);
            var apiKey = _configuration.GetValue<string>(llm.ApiKey!);

            var builder = Kernel.CreateBuilder();

            if (llm.Provider == LanguageModelProviders.MICROSOFT)
            {
                if (llm.UseChat)
                    builder.AddAzureOpenAIChatCompletion(deploymentName!, endpoint!, apiKey!);
                else
                    builder.AddAzureOpenAITextGeneration(deploymentName!, endpoint!, apiKey!);
            }
            else
            {
                if (llm.UseChat)
                    builder.AddOpenAIChatCompletion(deploymentName!, endpoint!, apiKey!);
                else
                    builder.AddOpenAITextGeneration(deploymentName!, endpoint!, apiKey!);
            }

            return builder.Build();
        }
    }
}
