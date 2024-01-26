using System.Text.Json.Serialization;

namespace FoundationaLLM.Vectorization.Models.Resources
{
    /// <summary>
    /// Provides details about a text partitioning profile.
    /// </summary>
    public class TextPartitioningProfile
    {
        /// <summary>
        /// The name of the text partitioning profile.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The type of the text splitter.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required TextSplitterType TextSplitter { get; set; }

        /// <summary>
        /// The settings used to configure the text splitter.
        /// </summary>
        public Dictionary<string, string>? TextSplitterSettings { get; set; }
    }
}
