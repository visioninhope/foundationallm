using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Upgrade.Models._070;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Upgrade.Agent
{
    public class Agent_070_080 : AgentUpgrade
    {
        public Agent_070_080(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<Agent_070_080>();

            SourceInstanceVersion = Version.Parse("0.7.0");

            SourceType = typeof(Agent070);
        }

        private ILogger<Agent_070_080> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public async override Task<object> UpgradeDoWorkAsync(object in_agent)
        {
            ConfigureDefaultValues();

            string strAgent = JsonSerializer.Serialize(in_agent);
            Agent070 agent = JsonSerializer.Deserialize<Agent070>(strAgent);
            Agent070 targetAgent = JsonSerializer.Deserialize<Agent070>(strAgent);

            if (agent.Version == SourceInstanceVersion)
            {
                //TODO
                SetDefaultValues(targetAgent);

                targetAgent.Version = Version.Parse("0.8.0");

                _logger.LogInformation($"Upgraded agent {agent.Name} from version {SourceInstanceVersion} to version {agent.Version}");
            }

            return targetAgent;
        }

        public override Task<object> UpgradeProperties(object agent) => Task.FromResult(agent);

    }
}
