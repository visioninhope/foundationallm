using FoundationaLLM.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FoundationaLLM.Common.Services.ResourceProviders
{
    /// <summary>
    /// Dependency injection extension methods for the resource provider services.
    /// </summary>
    public partial class DependencyInjection
    {
        /// <summary>
        /// Registers the resource provider services with the application.
        /// </summary>
        /// <param name="serviceProvider">The dependency injection service provider.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown when no service registrations for
        /// <see cref="IResourceProviderService"/> exist.</exception>
        public static async Task InitializeAgentResourceProvidersAsync(IServiceProvider serviceProvider)
        {
            var resourceProviderServices = serviceProvider.GetServices<IResourceProviderService>().ToList();

            // Check if any services are registered.
            if (resourceProviderServices.Count == 0)
            {
                throw new InvalidOperationException("No IResourceProviderService registrations found.");
            }

            // Initialize each service.
            foreach (var service in resourceProviderServices)
            {
                await service.Initialize();
            }
        }
    }
}
