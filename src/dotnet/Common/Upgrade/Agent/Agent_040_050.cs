using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Common.Upgrade.Models._040;
using FoundationaLLM.Common.Upgrade.Models._050;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Common.Upgrade.Agent
{
    public class Agent_040_050 : AgentUpgrade
    {
        public Agent_040_050(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<Agent_040_050>();

            TypeName = "Agent";

            SourceInstanceVersion = Version.Parse("0.4.0");

            SourceType = typeof(Agent040);
            TargetType = typeof(Agent050);
        }

        private ILogger<Agent_040_050> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public override Task<Dictionary<string, string>> LoadAgents()
        {
            //load the old "agents" container from the storage account

            return base.LoadAgents();
        }

        public async override Task<object> UpgradeDoWorkAsync(object in_agent)
        {
            ConfigureDefaultValues();

            string strAgent = JsonSerializer.Serialize(in_agent);

            Agent040 agent = JsonSerializer.Deserialize<Agent040>(strAgent);

            Agent050 targetAgent = JsonSerializer.Deserialize<Agent050>(strAgent);

            if (agent.Version == SourceInstanceVersion)
            {
                SetDefaultValues(targetAgent);

                targetAgent.Version = Version.Parse("0.5.0");

                _logger.LogInformation($"Upgraded {TypeName} {agent.Name} from version {SourceInstanceVersion} to version {agent.Version}");
            }

            return targetAgent;
        }

        public override Task<object> UpgradeProperties(object agent) => Task.FromResult(agent);
    }
}
