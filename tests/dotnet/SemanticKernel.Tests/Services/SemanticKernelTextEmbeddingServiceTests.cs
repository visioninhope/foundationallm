using FoundationaLLM.Common.Settings;
using FoundationaLLM.SemanticKernel.Core.Models.Configuration;
using FoundationaLLM.SemanticKernel.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SemanticKernel.Tests.Services
{
    public class SemanticKernelTextEmbeddingServiceTests
    {
        private SemanticKernelTextEmbeddingService _semanticKernelTextEmbeddingService;
        
        public SemanticKernelTextEmbeddingServiceTests()
        {
            _semanticKernelTextEmbeddingService = new SemanticKernelTextEmbeddingService(
                Options.Create(
                    new SemanticKernelTextEmbeddingServiceSettings { 
                        AuthenticationType = AzureOpenAIAuthenticationTypes.AzureIdentity,
                        DeploymentName = "embeddings",
                        Endpoint = Environment.GetEnvironmentVariable("SemanticKernelTextEmbeddingServiceTestsOpenAiEndpoint") ?? ""
                    }
                ),
                LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SemanticKernelTextEmbeddingService>()
            );
        }

        [Fact]
        public async void TestGetEmbedding()
        {
            var embeddingResult = await _semanticKernelTextEmbeddingService.GetEmbeddingAsync("Some Test Text");
            Assert.True(embeddingResult.Embedding.Length > 0);
            Assert.IsType<int>(embeddingResult.TokenCount);
        }

        [Fact]
        public async void TestGetEmbeddings()
        {
            var embeddingResult = await _semanticKernelTextEmbeddingService.GetEmbeddingsAsync(new List<string> { "Some Test Text" });
            Assert.True(embeddingResult.Embeddings.Count > 0);
            Assert.IsType<int>(embeddingResult.TokenCount);
        }
    }
}
