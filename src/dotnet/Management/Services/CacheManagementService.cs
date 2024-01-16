using FoundationaLLM.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Management.Interfaces;

namespace FoundationaLLM.Management.Services
{
    public class CacheManagementService(
        ILogger<ConfigurationManagementService> logger,
        IAgentFactoryAPIService agentFactoryApiService)
    {
        private readonly IAgentFactoryAPIService _agentFactoryApiService = agentFactoryApiService;
        readonly JsonSerializerSettings _jsonSerializerSettings = Common.Settings.CommonJsonSerializerSettings.GetJsonSerializerSettings();

        public async Task<bool> ClearAgentCache()
        {
            try
            {
                // TODO: Create a Task collection and await all tasks to run them concurrently.
                await _agentFactoryApiService.RemoveCacheCategory(CacheCategories.Agent);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to clear agent cache.");
                return false;
            }
        }
        }

    }
}
