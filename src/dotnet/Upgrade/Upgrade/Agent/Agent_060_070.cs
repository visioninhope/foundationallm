using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using FoundationaLLM.Common.Services.Storage;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using FoundationaLLM.Utility.Upgrade.Models._040;
using FoundationaLLM.Utility.Upgrade.Models._050;
using FoundationaLLM.Utility.Upgrade.Models._051;
using FoundationaLLM.Utility.Upgrade.Models._060;
using FoundationaLLM.Utility.Upgrade.Models._070;

namespace FoundationaLLM.Utility.Upgrade.Agent
{
    public class Agent_060_070 : AgentUpgrade
    {
        public Agent_060_070(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<Agent_060_070>();

            SourceInstanceVersion = Version.Parse("0.6.0");

            SourceType = typeof(Agent060);
            TargetType = typeof(Agent070);
        }

        private ILogger<Agent_060_070> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public async override Task<object> UpgradeDoWorkAsync(object in_agent)
        {
            ConfigureDefaultValues();

            string strAgent = JsonSerializer.Serialize(in_agent);
            Agent060 agent = JsonSerializer.Deserialize<Agent060>(strAgent);
            Agent070 targetAgent = JsonSerializer.Deserialize<Agent070>(strAgent);

            if (agent.Version == SourceInstanceVersion)
            {
                //TODO
                SetDefaultValues(targetAgent);

                targetAgent.Version = Version.Parse("0.7.0");

                _logger.LogInformation($"Upgraded agent {agent.Name} from version {SourceInstanceVersion} to version {agent.Version}");
            }

            return targetAgent;
        }

        public override Task<object> UpgradeProperties(object agent) => Task.FromResult(agent);

    }
}
