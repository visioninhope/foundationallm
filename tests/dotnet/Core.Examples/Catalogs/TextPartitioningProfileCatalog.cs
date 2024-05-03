using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;

namespace FoundationaLLM.Core.Examples.Catalogs
{
    public static class TextPartitioningProfileCatalog
    {
        public static readonly List<TextPartitioningProfile> Items =
        [
            new TextPartitioningProfile { Name = "", TextSplitter = TextSplitterType.TokenTextSplitter }
        ];

        public static List<TextPartitioningProfile> GetTextPartitioningProfiles()
        {
            var items = new List<TextPartitioningProfile>();
            items.AddRange(Items);
            return items;
        }
    }
}
