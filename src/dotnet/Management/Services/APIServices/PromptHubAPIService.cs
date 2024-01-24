using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Services;
using FoundationaLLM.Management.Interfaces;
using FoundationaLLM.Management.Models.Configuration.Cache;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Management.Services.APIServices
{
    /// <summary>
    /// Contains base functionality for calling the PromptHubAPI service.
    /// </summary>
    /// <param name="httpClientFactoryService">The HTTP client factory service.</param>
    /// <param name="logger">The logging interface used to log under the
    /// <see cref="PromptHubAPIService"/> type name.</param>
    public class PromptHubAPIService(
        IHttpClientFactoryService httpClientFactoryService,
        ILogger<PromptHubAPIService> logger) : APIServiceBase(
            Common.Constants.HttpClients.PromptHubAPI,
            httpClientFactoryService,
            logger), IPromptHubAPIService
    {
        
    }
}
