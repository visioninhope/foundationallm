﻿using FoundationaLLM.Common.Models;
using FoundationaLLM.Common.Models.Authorization;
using FoundationaLLM.Common.Models.ResourceProviders;

namespace FoundationaLLM.Common.Constants.ResourceProviders
{
    /// <summary>
    /// Provides metadata for the FoundationaLLM.Authorization resource provider.
    /// </summary>
    public class AuthorizationResourceProviderMetadata
    {
        /// <summary>
        /// The metadata describing the resource types allowed by the resource provider.
        /// </summary>
        public static Dictionary<string, ResourceTypeDescriptor> AllowedResourceTypes => new()
        {
            {
                AuthorizationResourceTypeNames.RoleAssignments,
                new ResourceTypeDescriptor(
                        AuthorizationResourceTypeNames.RoleAssignments)
                {
                    AllowedTypes = [
                        new ResourceTypeAllowedTypes(HttpMethod.Post.Method, [], [typeof(RoleAssignment)], [typeof(ResourceProviderUpsertResult)]),
                        new ResourceTypeAllowedTypes(HttpMethod.Delete.Method, [], [], [])
                    ],
                    Actions = [
                        new ResourceTypeAction(ResourceProviderActions.Filter, false, true, [
                            new ResourceTypeAllowedTypes(HttpMethod.Post.Method, [], [typeof(RoleAssignmentQueryParameters)], [typeof(ResourceProviderGetResult<RoleAssignment>)])
                        ])
                    ]
                }
            },
            {
                AuthorizationResourceTypeNames.RoleDefinitions,
                new ResourceTypeDescriptor(
                        AuthorizationResourceTypeNames.RoleDefinitions)
                {
                    AllowedTypes = [
                        new ResourceTypeAllowedTypes(HttpMethod.Get.Method, [], [], [typeof(RoleDefinition)])
                    ],
                    Actions = []
                }
            }
        };
    }
}
