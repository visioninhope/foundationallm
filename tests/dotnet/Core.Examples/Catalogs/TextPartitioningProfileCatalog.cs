using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;

namespace FoundationaLLM.Core.Examples.Catalogs
{
    public static class TextPartitioningProfileCatalog
    {
        public static readonly List<TextPartitioningProfile> Items =
        [
            new TextPartitioningProfile { Name = "text_partition_profile", TextSplitter = TextSplitterType.TokenTextSplitter, Settings = new Dictionary<string, string>{ { "Tokenizer", "MicrosoftBPETokenizer" }, { "TokenizerEncoder", "cl100k_base" }, { "ChunkSizeTokens", "500" }, { "OverlapSizeTokens", "50" } } }
        ];

        public static List<TextPartitioningProfile> GetTextPartitioningProfiles()
        {
            var items = new List<TextPartitioningProfile>();
            items.AddRange(Items);
            return items;
        }
    }
}
