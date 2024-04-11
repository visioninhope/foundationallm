using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Azure;

namespace FoundationaLLM.Gateway.Models
{
    /// <summary>
    /// Holds context associated with an embedding model.
    /// </summary>
    public class EmbeddingModelContext
    {
        /// <summary>
        /// The Azure OpenAI deployment for the model.
        /// </summary>
        public required AzureOpenAIAccountDeployment Deployment { get; set; }

        /// <summary>
        /// The <see cref="ITextEmbeddingService"/> exposing the capabilities to interact with the model.
        /// </summary>
        public required ITextEmbeddingService TextEmbeddingService { get; set; }
    }
}
