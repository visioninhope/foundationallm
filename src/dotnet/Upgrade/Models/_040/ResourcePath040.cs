using FoundationaLLM.Common.Models.ResourceProviders;

namespace FoundationaLLM.Upgrade.Models._040
{
    /// <summary>
    /// Provides the logic for handling FoundationaLLM resource identifiers.
    /// </summary>
    public class ResourcePath040
    {
        private readonly string? _instanceId;
        private readonly string? _resourceProvider;
        private readonly List<ResourceTypeInstance> _resourceTypeInstances;
        private readonly bool _isRootPath;

        private const string INSTANCE_TOKEN = "instances";
        private const string RESOURCE_PROVIDER_TOKEN = "providers";

        /// <summary>
        /// The instance id of the resource identifier. Can be null if the resource path does not contain an instance id.
        /// </summary>
        public string? InstanceId => _instanceId;

        /// <summary>
        /// The resource provider of the resource identifier. Can be null if the resource path does not contain a resource provider.
        /// </summary>
        public string? ResourceProvider => _resourceProvider;

        /// <summary>
        /// The resource type instances of the resource identifier.
        /// </summary>
        public List<ResourceTypeInstance> ResourceTypeInstances => _resourceTypeInstances;

        /// <summary>
        /// Indicates whether the resource path is a root path ("/") or not.
        /// </summary>
        public bool IsRootPath => _isRootPath;
    }
}
