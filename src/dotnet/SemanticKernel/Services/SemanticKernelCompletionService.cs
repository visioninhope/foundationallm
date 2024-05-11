using Azure.AI.OpenAI;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.SemanticKernel.Core.Models.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Net;

#pragma warning disable SKEXP0001, SKEXP0010

namespace FoundationaLLM.SemanticKernel.Core.Services
{
    /// <summary>
    /// Generates completions using the Semantic Kernel orchestrator.
    /// </summary>
    public class SemanticKernelCompletionService : ICompletionsService
    {
        private readonly SemanticKernelCompletionServiceSettings _settings;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<SemanticKernelCompletionService> _logger;
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _completionService;

        /// <summary>
        /// Creates a new <see cref="SemanticKernelCompletionService"/> instance.
        /// </summary>
        /// <param name="options">The <see cref="IOptions{TOptions}"/> providing configuration settings.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to create loggers for logging.</param>
        public SemanticKernelCompletionService(
            IOptions<SemanticKernelCompletionServiceSettings> options,
            ILoggerFactory loggerFactory)
        {
            _settings = options.Value;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<SemanticKernelCompletionService>();
            _kernel = CreateKernel();
            _completionService = _kernel.GetRequiredService<IChatCompletionService>();
        }

        /// <inheritdoc/>
        public async Task<CompletionResult> GetCompletionAsync(List<GatewayCompletionRequest> requests, string modelName = "gpt-4")
        {
            if (requests.Count == 0)
                return null;

            //TODO - process multiple requests at some point?
            GatewayCompletionRequest request = requests[0];

            CompletionResult result = new CompletionResult();
            
            if ( string.IsNullOrEmpty(request.UserPrompt) )
            {
                return new CompletionResult
                {
                    InProgress = false,
                    Failed = true,
                    ErrorMessage = "User prompt is required."
                };
            }

            try
            {
                while (true)
                {
                    //add a request to the counter
                    //_gatewayInstrumentation.EmbeddingsRequests.Add(0.5);
                    //bool allowRequests = _gatewayInstrumentation.EmbeddingModels[modelName].RequestCount.TryConsume(1);

                    //add up all the tokens that were sent
                    //_gatewayInstrumentation.EmbeddingsTokens.Add(tc.TokensCount / 2);
                    //bool allowTokens = _gatewayInstrumentation.EmbeddingModels[modelName].TokenCount.TryConsume(tc.TokensCount);
                    bool allowRequests = true;
                    bool allowTokens = true;

                    if (allowRequests && allowTokens)
                    {
                        //TODO - check for chat history to be present...
                        //_completionService.GetChatMessageContentsAsync()

                        //simple user prompt
                        ChatMessageContent chatMessageContent = await _completionService.GetChatMessageContentAsync(request.UserPrompt);
                        CompletionsUsage usage = (CompletionsUsage)chatMessageContent.Metadata["Usage"];

                        result = new CompletionResult {
                            OperationId = request.OperationId,
                            Result = new CompletionResponse
                            {
                                Completion = chatMessageContent.Content,
                                CompletionTokens = usage.CompletionTokens,
                                PromptTokens = usage.PromptTokens,
                                MetaData = (Dictionary<string, object>)chatMessageContent.Metadata
                            },
                            TokenCount = usage.CompletionTokens + usage.PromptTokens                            
                        };

                        return result;

                    }
                    else
                    {
                        if (!allowRequests)
                            Console.WriteLine("Request limit reached");

                        if (!allowTokens)
                            Console.WriteLine("Token limit reached");

                        await Task.Delay(500);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating completion.");

                result.InProgress = false;
                result.Failed = true;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <inheritdoc/>
        public Task<CompletionResult> GetCompletionAsync(string operationId) => throw new NotImplementedException();

        private Kernel CreateKernel()
        {
            ValidateDeploymentName(_settings.DeploymentName);
            ValidateEndpoint(_settings.Endpoint);

            var builder = Kernel.CreateBuilder();

            if (_settings.AuthenticationType == AzureOpenAIAuthenticationTypes.AzureIdentity)
            {
                builder.AddAzureOpenAIChatCompletion(
                    _settings.DeploymentName,
                    _settings.Endpoint,
                    DefaultAuthentication.AzureCredential);
            }
            else
            {
                ValidateAPIKey(_settings.APIKey);
                builder.AddAzureOpenAIChatCompletion(
                    _settings.DeploymentName,
                    _settings.Endpoint,
                    _settings.APIKey!);
            }

            builder.Services.AddSingleton<ILoggerFactory>(_loggerFactory);
            builder.Services.ConfigureHttpClientDefaults(c =>
            {
                // Use a standard resiliency policy configured to retry on 429 (too many requests).
                c.AddStandardResilienceHandler().Configure(o =>
                {
                    o.Retry.ShouldHandle = args => ValueTask.FromResult(args.Outcome.Result?.StatusCode is HttpStatusCode.TooManyRequests);
                });
            });

            return builder.Build();
        }

        private void ValidateDeploymentName(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _logger.LogCritical("The Azure Open AI deployment name is invalid.");
                throw new ConfigurationValueException("The Azure Open AI deployment name is invalid.");
            }
        }

        private void ValidateEndpoint(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _logger.LogCritical("The Azure Open AI endpoint is invalid.");
                throw new ConfigurationValueException("The Azure Open AI endpoint is invalid.");
            }
        }
        private void ValidateAPIKey(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _logger.LogCritical("The Azure Open AI API key is invalid.");
                throw new ConfigurationValueException("The Azure Open AI API key is invalid.");
            }
        }
    }
}
