using FoundationaLLM.Common.Models.ResourceProvider;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Provides the core services implemented by all resource providers. 
    /// </summary>
    public interface IResourceProviderService
    {
        /// <summary>
        /// The name of the resource provider.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Indicates whether the resource provider is initialized or not.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Initializes the resource provider.
        /// </summary>
        /// <returns></returns>
        Task Initialize();

        /// <summary>
        /// Gets a resource based on its logical path.
        /// </summary>
        /// <typeparam name="T">The type of the requested resource.</typeparam>
        /// <param name="resourcePath">The logical path of the requested resource.</param>
        /// <returns>The instance of the resource corresponding to the specified logical path.</returns>
        Task<T> GetResourceAsync<T>(string resourcePath) where T: class;

        /// <summary>
        /// Gets a resource based on its logical path.
        /// </summary>
        /// <typeparam name="T">The type of the requested resource.</typeparam>
        /// <param name="resourcePath">The logical path of the requested resource.</param>
        /// <returns>The instance of the resource corresponding to the specified logical path.</returns>
        T GetResource<T>(string resourcePath) where T : class;

        /// <summary>
        /// Executes an action based on its logical path.
        /// </summary>
        /// <param name="actionPath">The logical path of the action to be executed.</param>
        /// <returns>The <see cref="ResourceProviderActionResult"/> that contains details about the result of the execution.</returns>
        Task<ResourceProviderActionResult> ExecuteAction(string actionPath);
    }
}
