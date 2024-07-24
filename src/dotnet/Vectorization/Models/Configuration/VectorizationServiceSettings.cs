namespace FoundationaLLM.Vectorization.Models.Configuration
{
    /// <summary>
    /// The configuration settings for the Vectorization API service.
    /// </summary>
    public class VectorizationServiceSettings
    {
        /// <summary>
        /// The URL of the Vectorization API service.
        /// </summary>
        public required string APIUrl { get; set; }

        /// <summary>
        /// The API key for the Vectorization API service.
        /// </summary>
        public required string APIKey { get; set; }
    }
}
