﻿using Azure.AI.OpenAI;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Vectorization;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Gateway.Services
{
    public class GatewayTextEmbeddingService : ITextEmbeddingService
    {
        private readonly string _accountEndpoint;
        private readonly AzureOpenAIClient _azureOpenAIClient;
        private readonly ILogger<GatewayTextEmbeddingService> _logger;

        public GatewayTextEmbeddingService(
            string accountEndpoint,
            ILogger<GatewayTextEmbeddingService> logger)
        {
            _accountEndpoint = accountEndpoint;
            _azureOpenAIClient = new AzureOpenAIClient(new Uri(_accountEndpoint), DefaultAuthentication.AzureCredential);
            _logger = logger;
        }

        public async Task<TextEmbeddingResult> GetEmbeddingsAsync(IList<TextChunk> textChunks, string modelName = "text-embedding-ada-002")
        {
            try
            {
                var embeddingClient = _azureOpenAIClient.GetEmbeddingClient(modelName);
                var result = await embeddingClient.GenerateEmbeddingsAsync(textChunks.Select(tc => tc.Content!).ToList());

                return new TextEmbeddingResult
                {
                    InProgress = false,
                    TextChunks = Enumerable.Range(0, result.Value.Count).Select(i =>
                    {
                        var textChunk = textChunks[i];
                        textChunk.Embedding = new Common.Models.Vectorization.Embedding(result.Value[i].Vector);
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

        public Task<TextEmbeddingResult> GetEmbeddingsAsync(string operationId) => throw new NotImplementedException();
    }
}
