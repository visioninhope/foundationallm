using FoundationaLLM.Common.Interfaces;
using Microsoft.SemanticKernel.Connectors.Memory.AzureAISearch;
using FoundationaLLM.SemanticKernel.Core.Services;
using Microsoft.Extensions.Options;
using FoundationaLLM.SemanticKernel.Core.Models.Configuration;
using FoundationaLLM.Common.Settings;
using Microsoft.Extensions.Logging;
using FoundationaLLM.Common.Models.TextEmbedding;

#pragma warning disable SKEXP0021 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
namespace FoundationaLLM.SemanticKernel.Tests.Services
{
    public class AzureAISearchIndexingServiceTests
    {
        private AzureAISearchMemoryStore _azureAISearchMemoryStoreClient;
        private IIndexingService _indexingService;

        public AzureAISearchIndexingServiceTests() {
            var endpoint = Environment.GetEnvironmentVariable("AzureAISearchIndexingServiceTestsSearchEndpoint") ?? "";
            var key = Environment.GetEnvironmentVariable("AzureAISearchIndexingServiceTestsSearchKey") ?? "";
            _azureAISearchMemoryStoreClient = new AzureAISearchMemoryStore(
                endpoint,
                key
            );
            _indexingService = new AzureAISearchIndexingService(
                Options.Create(
                    new AzureAISearchIndexingServiceSettings
                    {
                        Endpoint = endpoint,
                        AuthenticationType = AzureAISearchAuthenticationTypes.APIKey,
                        APIKey = key
                    }
                ),
                LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<AzureAISearchIndexingService>()
            );
        }

        [Fact]
        public async void TestIndexEmbeddingsAsync()
        {
            var indexName = Environment.GetEnvironmentVariable("AzureAISearchIndexingServiceTestsCollectionName") ?? "semantickernel-integration-tests";
            
            await _azureAISearchMemoryStoreClient.CreateCollectionAsync(
                indexName
            );

            EmbeddedContent embeddedContent = new EmbeddedContent
            {
                ContentId = new ContentIdentifier {
                    MultipartId = new List<string> {
                        "https://somesa.blob.core.windows.net",
                        "vectorization-input",
                        "somedata.pdf"
                    },
                    ContentSourceProfileName = "SomePDFData",
                    CanonicalId = "SomeBusinessUnit/SomePDFData"
                },
                ContentParts = new List<EmbeddedContentPart>
                {
                    new EmbeddedContentPart {
                        Id = "1",
                        Content = "This is Phrase #1",
                        Embedding = new Embedding
                        {
                            Vector = new ReadOnlyMemory<float> {}
                        }
                    },
                    new EmbeddedContentPart {
                        Id = "2",
                        Content = "This is Phrase #2",
                        Embedding = new Embedding
                        {
                            Vector = new ReadOnlyMemory<float> {}
                        }
                    },
                    new EmbeddedContentPart {
                        Id = "3",
                        Content = "This is Phrase #3",
                        Embedding = new Embedding
                        {
                            Vector = new ReadOnlyMemory<float> {}
                        }
                    }
                }
            };

            Assert.Equal(
                3,
                (await _indexingService.IndexEmbeddingsAsync(embeddedContent, indexName)).Count
            );

            await _azureAISearchMemoryStoreClient.DeleteCollectionAsync(indexName);
        }
    }
}
#pragma warning restore SKEXP0021 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.