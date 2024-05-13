using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Common.Upgrade.Models._050;
using FoundationaLLM.Common.Upgrade.Models._051;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Common.Upgrade.Agent
{
    public class Agent_050_051 : AgentUpgrade
    {
        public Agent_050_051(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<Agent_050_051>();

            SourceInstanceVersion = Version.Parse("0.5.0");

            SourceType = typeof(Agent050);
            TargetType = typeof(Agent051);
        }

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        private ILogger<Agent_050_051> _logger;

        public async override Task<object> UpgradeDoWorkAsync(object in_agent)
        {
            ConfigureDefaultValues();

            string strAgent = JsonSerializer.Serialize(in_agent);
            Agent050 agent = JsonSerializer.Deserialize<Agent050>(strAgent);
            Agent051 targetAgent = JsonSerializer.Deserialize<Agent051>(strAgent);

            if (agent.Version == SourceInstanceVersion)
            {
                //TODO
                SetDefaultValues(targetAgent);

                targetAgent.Version = Version.Parse("0.5.1");

                _logger.LogInformation($"Upgraded agent {agent.Name} from version {SourceInstanceVersion} to version {agent.Version}");
            }

            return targetAgent;
        }

        public override Task<object> UpgradeProperties(object agent) => Task.FromResult(agent);
    }
}
