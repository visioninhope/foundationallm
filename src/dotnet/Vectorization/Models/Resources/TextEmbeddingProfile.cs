using System.Text.Json.Serialization;

namespace FoundationaLLM.Vectorization.Models.Resources
{
    /// <summary>
    /// Provides details about a text embedding profile.
    /// </summary>
    public class TextEmbeddingProfile
    {
        /// <summary>
        /// The name of the text embedding profile.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The type of the text splitter.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required TextEmbeddingType TextEmbedding { get; set; }

        /// <summary>
        /// The settings used to configure the text embedder.
        /// </summary>
        public Dictionary<string, string>? TextEmbeddingSettings { get; set; }
    }
}
