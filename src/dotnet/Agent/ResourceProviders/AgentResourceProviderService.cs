using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Services.ResourceProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.ResourceProviders
{
    public class AgentResourceProviderService(
        [FromKeyedServices(DependencyInjectionKeys.FoundationaLLM_Agent_ResourceProviderService)] IStorageService storageService,
        ILogger<AgentResourceProviderService> logger)
        : ResourceProviderServiceBase(
            storageService,
            logger)
    {
    }
}
