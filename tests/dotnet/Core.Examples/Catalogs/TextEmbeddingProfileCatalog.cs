using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;

namespace FoundationaLLM.Core.Examples.Catalogs
{
    public static class TextEmbeddingProfileCatalog
    {
        public static readonly List<TextEmbeddingProfile> Items =
        [
            new TextEmbeddingProfile { Name = "", TextEmbedding = TextEmbeddingType.GatewayTextEmbedding }
        ];

        public static List<TextEmbeddingProfile> GetTextEmbeddingProfiles()
        {
            var items = new List<TextEmbeddingProfile>();
            items.AddRange(Items);
            return items;
        }
    }
}
