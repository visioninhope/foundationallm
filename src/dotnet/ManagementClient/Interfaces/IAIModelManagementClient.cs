using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.AIModel;

namespace FoundationaLLM.Client.Management.Interfaces
{
    /// <summary>
    /// Provides methods to manage AI model resources.
    /// </summary>
    public interface IAIModelManagementClient
    {
        /// <summary>
        /// Upserts an ai model resource. If an ai model does not exist, it will be created.
        /// If an ai model configuration does exist, it will be updated.
        /// </summary>
        /// <param name="aiModel">The api endpoint configuration resource to create or update.</param>
        /// <returns>Returns a <see cref="ResourceProviderUpsertResult"/>, which contains the
        /// Object ID of the resource.</returns>
        Task<ResourceProviderUpsertResult> UpsertAIModel(AIModelBase aiModel);
    }
}
