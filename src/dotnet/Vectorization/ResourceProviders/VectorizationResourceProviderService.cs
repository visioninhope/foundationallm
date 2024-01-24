using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Services.ResourceProviders;
using FoundationaLLM.Vectorization.Models.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.ResourceProviders
{
    /// <summary>
    /// Implements the FoundationaLLM.Vectorization resource provider.
    /// </summary>
    public class VectorizationResourceProviderService(
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_Vectorization_ResourceProviderService)] IStorageService storageService,
        ILogger<VectorizationResourceProviderService> logger)
        : ResourceProviderServiceBase(
            storageService,
            logger)
    {
        private readonly Dictionary<string, ContentSource> _contentSources = [];
        private readonly Dictionary<string, PartitionProfile> _partitionProfiles = [];

        /// <inheritdoc/>
        protected override string _name => ResourceProviderNames.FoundationaLLM_Vectorization;

        /// <inheritdoc/>
        protected override Dictionary<string, ResourceTypeDescriptor> _resourceTypes => new Dictionary<string, ResourceTypeDescriptor>
        {
            {
                VectorizationResourceTypeNames.ContentSources,
                new ResourceTypeDescriptor(VectorizationResourceTypeNames.ContentSources)
            },
            {
                VectorizationResourceTypeNames.PartitionProfiles,
                new ResourceTypeDescriptor(VectorizationResourceTypeNames.PartitionProfiles)
            }
        };

        /// <inheritdoc/>
        protected override async Task InitializeInternal()
        {
            _logger.LogInformation("Starting to initialize the {ResourceProvider} resource provider.", _name);

            await Task.Delay(1000);

            _logger.LogInformation("The {ResourceProvider} resource provider was successfully initialized.", _name);
        }

        /// <inheritdoc/>
        protected override T GetResourceInternal<T>(List<ResourceTypeInstance> instances) where T: class =>
            instances[0].ResourceType switch
            {
                VectorizationResourceTypeNames.ContentSources => GetContentSource<T>(instances),
                VectorizationResourceTypeNames.PartitionProfiles => GetPartitionProfile<T>(instances),
                _ => throw new ResourceProviderException($"The resource type {instances[0].ResourceType} is not supported by the {_name} resource manager.")
            };

        private T GetContentSource<T>(List<ResourceTypeInstance> instances) where T: class
        {
            if (instances.Count != 1)
                throw new ResourceProviderException($"Invalid resource path");

            if (typeof(T) != typeof(ContentSource))
                throw new ResourceProviderException($"The type of requested resource ({typeof(T)}) does not match the resource type specified in the path ({instances[0].ResourceType}).");

            _contentSources.TryGetValue(instances[0].ResourceId!, out var contentSource);
            return contentSource as T
                ?? throw new ResourceProviderException($"The resource {instances[0].ResourceId!} of type {instances[0].ResourceType} was not found.");
        }

        private T GetPartitionProfile<T>(List<ResourceTypeInstance> instances) where T: class
        {
            if (instances.Count != 1)
                throw new ResourceProviderException($"Invalid resource path");

            if (typeof(T) != typeof(ContentSource))
                throw new ResourceProviderException($"The type of requested resource ({typeof(T)}) does not match the resource type specified in the path ({instances[0].ResourceType}).");

            _partitionProfiles.TryGetValue(instances[0].ResourceId!, out var partitionProfile);
            return partitionProfile as T
                ?? throw new ResourceProviderException($"The resource {instances[0].ResourceId!} of type {instances[0].ResourceType} was not found.");
        }
    }
}
