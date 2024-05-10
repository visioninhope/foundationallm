using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Instrumentation;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Gateway.Instrumentation;
using FoundationaLLM.SemanticKernel.Core.Models.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using System.Net;

#pragma warning disable SKEXP0001, SKEXP0010

namespace FoundationaLLM.SemanticKernel.Core.Services
{
    /// <summary>
    /// Generates text embeddings using the Semantic Kernel orchestrator.
    /// </summary>
    public class SemanticKernelTextEmbeddingService : ITextEmbeddingService
    {
        private readonly SemanticKernelTextEmbeddingServiceSettings _settings;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<SemanticKernelTextEmbeddingService> _logger;
        private readonly Kernel _kernel;
        private readonly ITextEmbeddingGenerationService _textEmbeddingService;
        private GatewayInstrumentation _gatewayInstrumentation;

        /// <summary>
        /// Creates a new <see cref="SemanticKernelTextEmbeddingService"/> instance.
        /// </summary>
        /// <param name="options">The <see cref="IOptions{TOptions}"/> providing configuration settings.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> used to create loggers for logging.</param>
        public SemanticKernelTextEmbeddingService(
            IOptions<SemanticKernelTextEmbeddingServiceSettings> options,
            ILoggerFactory loggerFactory,
            ILogger<SemanticKernelTextEmbeddingService> logger,
            GatewayInstrumentation gatewayInstrumentation)
        {
            _settings = options.Value;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<SemanticKernelTextEmbeddingService>();
            _kernel = CreateKernel();
            _textEmbeddingService = _kernel.GetRequiredService<ITextEmbeddingGenerationService>();
            _gatewayInstrumentation = gatewayInstrumentation;
        }

        /// <inheritdoc/>
        public async Task<TextEmbeddingResult> GetEmbeddingsAsync(IList<TextChunk> textChunks, string modelName = "text-embedding-ada-002")
        {
            try
            List<ReadOnlyMemory<float>> embeddings = new List<ReadOnlyMemory<float>>();

            foreach(TextChunk tc in textChunks)
            {
                if ( string.IsNullOrEmpty(tc.Content) )
                {
                    embeddings.Add(ReadOnlyMemory<float>.Empty);
                    continue;
                }

                try
                {
                    while (true)
                    {
                        //add a request to the counter
                        _gatewayInstrumentation.EmbeddingsRequests.Add(0.5);
                        bool allowRequests = _gatewayInstrumentation.EmbeddingModels[modelName].RequestCount.TryConsume(1);

                        //add up all the tokens that were sent
                        _gatewayInstrumentation.EmbeddingsTokens.Add(tc.TokensCount / 2);
                        bool allowTokens = _gatewayInstrumentation.EmbeddingModels[modelName].TokenCount.TryConsume(tc.TokensCount);

                        if (allowRequests && allowTokens)
                        {
                            ReadOnlyMemory<float> item = await _textEmbeddingService.GenerateEmbeddingAsync(tc.Content);
                            embeddings.Add(item);
                            break;
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
                    tc.Error = ex.Message;
                    embeddings.Add(ReadOnlyMemory<float>.Empty);
                }
            }

            //var embeddings = await _textEmbeddingService.GenerateEmbeddingsAsync(textChunks.Select(tc => tc.Content!).ToList());

            return new TextEmbeddingResult
            {
                var embeddings = await _textEmbeddingService.GenerateEmbeddingsAsync(textChunks.Select(tc => tc.Content!).ToList());
                return new TextEmbeddingResult
                {
                    InProgress = false,
                    TextChunks = Enumerable.Range(0, embeddings.Count).Select(i =>
                    {
                        var textChunk = textChunks[i];
                        textChunk.Embedding = new Embedding(embeddings[i]);
                        return textChunk;
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating embeddings.");
                return new TextEmbeddingResult
                {
                    InProgress = false,
                    Failed = true,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <inheritdoc/>
        public Task<TextEmbeddingResult> GetEmbeddingsAsync(string operationId) => throw new NotImplementedException();

        private Kernel CreateKernel()
        {
            ValidateDeploymentName(_settings.DeploymentName);
            ValidateEndpoint(_settings.Endpoint);

            var builder = Kernel.CreateBuilder();
            if (_settings.AuthenticationType == AzureOpenAIAuthenticationTypes.AzureIdentity)
            {
                builder.AddAzureOpenAITextEmbeddingGeneration(
                    _settings.DeploymentName,
                    _settings.Endpoint,
                    DefaultAuthentication.AzureCredential);
            }
            else
            {
                ValidateAPIKey(_settings.APIKey);
                builder.AddAzureOpenAITextEmbeddingGeneration(
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
