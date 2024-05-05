using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;

namespace FoundationaLLM.Core.Examples.Catalogs
{
    public static class IndexingProfilesCatalog
    {
        public static readonly List<IndexingProfile> Items =
        [
            new IndexingProfile { Name = "indexing_profile", Indexer = IndexerType.AzureAISearchIndexer, Settings = new Dictionary<string, string>{ { "IndexName", "blah" }, { "TopN", "3" }, { "Filters", "" }, { "EmbeddingFieldName", "Embedding" }, { "TextFieldName", "Text" } }, ConfigurationReferences = new Dictionary<string, string>{ { "AuthenticationType", "FoundationaLLM:Vectorization:AzureAISearchIndexingService:AuthenticationType" }, { "Endpoint", "FoundationaLLM:Vectorization:AzureAISearchIndexingService:Endpoint" } } }
        ];

        public static List<IndexingProfile> GetIndexingProfiles()
        {
            var items = new List<IndexingProfile>();
            items.AddRange(Items);
            return items;
        }
    }
}
