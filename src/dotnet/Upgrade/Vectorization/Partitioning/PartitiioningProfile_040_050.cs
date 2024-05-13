using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Upgrade.Models._050;
using FoundationaLLM.Upgrade.Models._060;
using FoundationaLLM.Upgrade.Vectorization.Indexing;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Upgrade.Vectorization.Partitioning
{
    public class PartitiioningProfile_040_050 : ContentSourceProfileUpgrade
    {
        public PartitiioningProfile_040_050(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<PartitiioningProfile_040_050>();

            TypeName = "ContentSourceProfile";

            SourceInstanceVersion = Version.Parse("0.4.0");

            SourceType = typeof(TextPartitioningProfile050);
            TargetType = typeof(TextPartitioningProfile050);
        }

        private ILogger<PartitiioningProfile_040_050> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public override Task<Dictionary<string, string>> LoadArtifacts() => base.LoadArtifacts();

        public async override Task<object> UpgradeDoWorkAsync(object in_agent)
        {
            ConfigureDefaultValues();

            string strAgent = JsonSerializer.Serialize(in_agent);

            TextPartitioningProfile050 source = JsonSerializer.Deserialize<TextPartitioningProfile050>(strAgent);

            TextPartitioningProfile050 target = JsonSerializer.Deserialize<TextPartitioningProfile050>(strAgent);

            if (source.Version == SourceInstanceVersion)
            {
                SetDefaultValues(target);

                target.Version = Version.Parse("0.5.0");

                _logger.LogInformation($"Upgraded {TypeName} {source.Name} from version {source.Version} to version {target.Version}");
            }

            return target;
        }

        public override Task<object> UpgradeProperties(object agent) => Task.FromResult(agent);
    }
}
