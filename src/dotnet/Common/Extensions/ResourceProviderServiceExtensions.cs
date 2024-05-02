using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.ResourceProviders;

namespace FoundationaLLM.Common.Extensions
{
    /// <summary>
    /// Extends the <see cref="IResourceProviderService"/> interface with helper methods.
    /// </summary>
    public static class ResourceProviderServiceExtensions
    {
        /// <summary>
        /// Gets a resource from the resource provider service.
        /// </summary>
        /// <typeparam name="T">The object type of the resource being retrieved.</typeparam>
        /// <param name="resourceProviderService">The <see cref="IResourceProviderService"/> providing the resource provider services.</param>
        /// <param name="objectId">The resource object identifier.</param>
        /// <param name="userIdentity">The <see cref="UnifiedUserIdentity"/> providing information about the calling user identity.</param>
        /// <returns>A resource object of type <typeparamref name="T"/>.</returns>
        public static async Task<T> GetResource<T>(
            this IResourceProviderService resourceProviderService,
            string objectId,
            UnifiedUserIdentity userIdentity)
            where T : ResourceBase
        {
            if (!resourceProviderService.IsInitialized)
                throw new ResourceProviderException($"The resource provider {resourceProviderService.Name} is not initialized.");

            var result = await resourceProviderService.HandleGetAsync(
                objectId,
                userIdentity);
            return (result as List<T>)!.First();
        }

        /// <summary>
        /// Gets a list of resources from the resource provider service.
        /// </summary>
        /// <typeparam name="T">The object type of the resources being retrieved.</typeparam>
        /// <param name="resourceProviderService">The <see cref="IResourceProviderService"/> providing the resource provider services.</param>
        /// <param name="userIdentity">The <see cref="UnifiedUserIdentity"/> providing information about the calling user identity.</param>
        /// <returns>A list of resource objects of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ResourceProviderException"></exception>
        public static async Task<List<T>> GetResources<T>(
            this IResourceProviderService resourceProviderService,
            UnifiedUserIdentity userIdentity)
            where T : ResourceBase
        {
            if (!resourceProviderService.IsInitialized)
                throw new ResourceProviderException($"The resource provider {resourceProviderService.Name} is not initialized.");

            var resourceTypeDescriptor = resourceProviderService.AllowedResourceTypes.Values
                .SingleOrDefault(rtd => rtd.TypeAllowedForHttpGet(typeof(T)))
                ?? throw new ResourceProviderException($"The resource provider {resourceProviderService.Name} does not support retrieving resources of type {typeof(T).Name}.");

            var result = await resourceProviderService.HandleGetAsync(
                $"/{resourceTypeDescriptor.ResourceType}",
                userIdentity);

            return (result as List<T>)!;
        }

        /// <summary>
        /// Waits for the resource provider service to be initialized.
        /// </summary>
        /// <param name="resourceProviderService">The <see cref="IResourceProviderService"/> providing the resource provider services.</param>
        /// <returns></returns>
        public static async Task WaitForInitialization(
            this IResourceProviderService resourceProviderService)
        {
            if (resourceProviderService.IsInitialized)
                return;

            for (int i = 0; i < 6; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
                if (resourceProviderService.IsInitialized)
                    return;
            }

            throw new ResourceProviderException($"The resource provider {resourceProviderService.Name} did not initialize within the expected time frame.");
        }
    }
}
