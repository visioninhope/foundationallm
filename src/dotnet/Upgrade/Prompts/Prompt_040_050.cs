using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Upgrade.Models._040;
using FoundationaLLM.Upgrade.Models._050;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Upgrade.Prompts
{
    public class Prompt_040_050 : PromptUpgrade
    {
        public Prompt_040_050(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<Prompt_040_050>();

            SourceInstanceVersion = Version.Parse("0.4.0");

            SourceType = typeof(Prompt040);
            TargetType = typeof(Prompt050);
        }

        private ILogger<Prompt_040_050> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public override Task<Dictionary<string, string>> LoadArtifacts() => base.LoadArtifacts();

        public async override Task<object> UpgradeDoWorkAsync(object in_source)
        {
            ConfigureDefaultValues();

            string strSource = JsonSerializer.Serialize(in_source);
            Prompt040 source = JsonSerializer.Deserialize<Prompt040>(strSource);
            source.Version = source.Version ?? Version.Parse("0.4.0");

            Prompt050 target = JsonSerializer.Deserialize<Prompt050>(strSource);
            target.Version = target.Version ?? Version.Parse("0.5.0");

            if (source.Version == SourceInstanceVersion)
            {
                //TODO
                SetDefaultValues(target);

                target.Version = Version.Parse("0.5.0");

                _logger.LogInformation($"Upgraded {TypeName} : {source.Name} : from version {SourceInstanceVersion} to version {target.Version}");
            }

            return target;
        }

        public override Task<object> UpgradeProperties(object source) => Task.FromResult(source);
    }
}
