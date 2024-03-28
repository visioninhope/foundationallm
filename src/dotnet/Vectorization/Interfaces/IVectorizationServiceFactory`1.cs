using FoundationaLLM.Common.Models.ResourceProvider;

namespace FoundationaLLM.Vectorization.Interfaces
{
    /// <summary>
    /// Creates typed service instances.
    /// </summary>
    public interface IVectorizationServiceFactory<T>

    {
        /// <summary>
        /// Retrieves a service instance of type T specified by name.
        /// </summary>
        /// <param name="serviceName">The name of the service instance to create.</param>
        /// <returns>The service instance created by name.</returns>
        T GetService(string serviceName);

        /// <summary>
        /// Retrieves a service instance of type T specified by name and its associated vectorization profile.
        /// </summary>
        /// <param name="serviceName">The name of the service instance to create.</param>
        /// <returns>The service instance and its associated vectorization profile.</returns>
        (T Service, ResourceBase Resource) GetServiceWithResource(string serviceName);
    }
}
