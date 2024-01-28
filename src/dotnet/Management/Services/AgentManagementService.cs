using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Vectorization.Models.Resources;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoundationaLLM.Vectorization.ResourceProviders;

namespace FoundationaLLM.Management.Services
{
    public class AgentManagementService(
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_Vectorization_ResourceProviderService)]
        IResourceProviderService vectorizationResourceProviderService,
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_Agent_ResourceProviderService)]
        IResourceProviderService agentResourceProviderService)
    {
        private readonly IResourceProviderService _vectorizationResourceProviderService =
            vectorizationResourceProviderService;
        private readonly IResourceProviderService _agentResourceProviderService =
            agentResourceProviderService;

        public async List<ContentSourceProfile> GetVectorContentSourceProfiles()
        {
            var contentSourceProfiles = await _vectorizationResourceProviderService.GetResourceAsync<ContentSourceProfile>(
                                           $"/{VectorizationResourceTypeNames.ContentSourceProfiles}");

            return contentSourceProfiles;
        }
    }
}
