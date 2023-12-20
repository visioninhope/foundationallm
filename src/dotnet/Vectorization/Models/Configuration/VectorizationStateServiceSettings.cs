namespace FoundationaLLM.Vectorization.Models.Configuration
{
    public class VectorizationStateServiceSettings
    {
        /// <summary>
        /// Connexion to the blob storage
        /// </summary>
        public required string BlobStorageConnection { get; set; }
    }
}
