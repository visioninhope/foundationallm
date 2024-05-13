using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Upgrade.Models._051;
using FoundationaLLM.Upgrade.Models._060;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Upgrade.Agent
{
    public class Agent_051_060 : AgentUpgrade
    {
        public Agent_051_060(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<Agent_051_060>();

            SourceInstanceVersion = Version.Parse("0.5.1");

            SourceType = typeof(Agent051);
            TargetType = typeof(Agent060);
        }

        private ILogger<Agent_051_060> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public async override Task<object> UpgradeDoWorkAsync(object in_agent)
        {
            ConfigureDefaultValues();

            string strAgent = JsonSerializer.Serialize(in_agent);
            Agent051 agent = JsonSerializer.Deserialize<Agent051>(strAgent);
            Agent060 targetAgent = JsonSerializer.Deserialize<Agent060>(strAgent);

            if (agent.Version == SourceInstanceVersion)
            {
                //TODO
                SetDefaultValues(targetAgent);

                targetAgent.Version = Version.Parse("0.6.0");

                _logger.LogInformation($"Upgraded agent {agent.Name} from version {SourceInstanceVersion} to version {agent.Version}");
            }

            return targetAgent;
        }

        public override Task<object> UpgradeProperties(object agent) => Task.FromResult(agent);
    }
}
