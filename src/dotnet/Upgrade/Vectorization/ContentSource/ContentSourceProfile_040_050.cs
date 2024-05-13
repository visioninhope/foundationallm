using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Upgrade.Models._050;
using FoundationaLLM.Upgrade.Vectorization.Indexing;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Upgrade.Agent
{
    namespace FoundationaLLM.Upgrade.Vectorization.ContentSource
    {
        public class ContentSourceProfile_040_050 : ContentSourceProfileUpgrade
        {
            public ContentSourceProfile_040_050(BlobStorageService blobStorageService,
                InstanceSettings settings,
                ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
            {
                _blobStorageService = blobStorageService;
                _logger = loggerFactory.CreateLogger<ContentSourceProfile_040_050>();

                TypeName = "ContentSourceProfile";

                SourceInstanceVersion = Version.Parse("0.4.0");

                SourceType = typeof(ContentSourceProfile050);
                TargetType = typeof(ContentSourceProfile050);
            }

            private ILogger<ContentSourceProfile_040_050> _logger;

            public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

            public override Task<Dictionary<string, string>> LoadArtifacts() => base.LoadArtifacts();

            public async override Task<object> UpgradeDoWorkAsync(object in_agent)
            {
                ConfigureDefaultValues();

                string strAgent = JsonSerializer.Serialize(in_agent);

                ContentSourceProfile050 source = JsonSerializer.Deserialize<ContentSourceProfile050>(strAgent);

                ContentSourceProfile050 target = JsonSerializer.Deserialize<ContentSourceProfile050>(strAgent);

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
}
