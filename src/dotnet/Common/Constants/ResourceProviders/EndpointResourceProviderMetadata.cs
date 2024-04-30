using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Endpoint;

namespace FoundationaLLM.Common.Constants.ResourceProviders
{
    /// <summary>
    /// Provides metadata for the FoundationaLLM.Endpoint resource provider.
    /// </summary>
    public static class EndpointResourceProviderMetadata
    {
        /// <summary>
        /// The medatada describing the resource types allowed by the resource provider.
        /// </summary>
        public static Dictionary<string, ResourceTypeDescriptor> AllowedResourceTypes => new()
        {
            {
                EndpointResourceTypeNames.Endpoints,
                new ResourceTypeDescriptor(EndpointResourceTypeNames.Endpoints)
                {
                    AllowedTypes = [
                        new ResourceTypeAllowedTypes(HttpMethod.Get.Method, [], [], [typeof(EndpointBase)]),
                        new ResourceTypeAllowedTypes(HttpMethod.Post.Method, [], [typeof(EndpointBase)], [typeof(ResourceProviderUpsertResult)]),
                        new ResourceTypeAllowedTypes(HttpMethod.Delete.Method, [], [], []),
                    ],
                    Actions = [
                            new ResourceTypeAction(EndpointResourceProviderActions.CheckName, false, true, [
                                new ResourceTypeAllowedTypes(HttpMethod.Post.Method, [], [typeof(ResourceName)], [typeof(ResourceNameCheckResult)])
                            ])
                        ]
                }
            }
        };
    }
}
