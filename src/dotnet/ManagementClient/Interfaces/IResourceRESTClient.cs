using FoundationaLLM.Common.Models.ResourceProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Client.Management.Interfaces
{
    /// <summary>
    /// Provides methods to manage calls to the Management API's resource endpoints.
    /// </summary>
    public interface IResourceRESTClient
    {
        /// <summary>
        /// Gets one or more resource.
        /// </summary>
        /// <param name="resourceProvider">The name of the resource provider that should handle the request.</param>
        /// <param name="resourcePath">The logical path of the resource type.</param>
        /// <returns></returns>
        Task<T> GetResourcesAsync<T>(string resourceProvider, string resourcePath);
        /// <summary>
        /// Creates or updates resources.
        /// </summary>
        /// <param name="resourceProvider">The name of the resource provider that should handle the request.</param>
        /// <param name="resourcePath">The logical path of the resource type.</param>
        /// <param name="serializedResource">The serialized resource to be created or updated.</param>
        /// <returns>The ObjectId of the created or updated resource.</returns>
        Task<ResourceProviderUpsertResult> UpsertResourceAsync(string resourceProvider, string resourcePath, object serializedResource);
        /// <summary>
        /// Deletes a resource.
        /// </summary>
        /// <param name="resourceProvider">The name of the resource provider that should handle the request.</param>
        /// <param name="resourcePath">The logical path of the resource type.</param>
        Task DeleteResourceAsync(string resourceProvider, string resourcePath);
    }
}
