using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._050
{
    /// <summary>
    /// Provides details about a text partitioning profile.
    /// </summary>
    public class TextPartitioningProfile050 : VectorizationProfileBase050
    {
        /// <summary>
        /// The type of the text splitter.
        /// </summary>
        [JsonPropertyName("text_splitter")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required TextSplitterType TextSplitter { get; set; }
    }
}
