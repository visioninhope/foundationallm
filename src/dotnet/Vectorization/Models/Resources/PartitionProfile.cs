using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Models.Resources
{
    /// <summary>
    /// Provides details about a text partitioning profile.
    /// </summary>
    public class PartitionProfile
    {
        /// <summary>
        /// The name of the text partitioning profile.
        /// </summary>
        [JsonPropertyOrder(0)]
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        /// <summary>
        /// The type of the text splitter.
        /// </summary>
        [JsonPropertyOrder(1)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required TextSplitterType TextSplitter { get; set; }

        /// <summary>
        /// The settings used to configure the text splitter.
        /// </summary>
        [JsonPropertyOrder(2)]
        [JsonPropertyName("text_splitter_settings")]
        public object? TextSplitterSettings { get; set; }
    }
}
