using FoundationaLLM.AgentFactory.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.AgentFactory.Core.Services
{
    /// <summary>
    /// Implements warmup capabilities
    /// </summary>
    public class WarmupService(
        IAgentHubAPIService agentHubService,
        IPromptHubAPIService promptHubService,
        IDataSourceHubAPIService dataSourceHubService) : IWarmupService
    {
    }
}
