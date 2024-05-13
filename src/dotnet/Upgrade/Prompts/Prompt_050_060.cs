using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Upgrade.Models._050;
using FoundationaLLM.Upgrade.Models._060;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Upgrade.Prompts
{
    public class Prompt_050_060 : PromptUpgrade
    {
        public Prompt_050_060(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<Prompt_050_060>();

            SourceInstanceVersion = Version.Parse("0.5.0");

            SourceType = typeof(Prompt050);
            TargetType = typeof(Prompt060);
        }

        private ILogger<Prompt_050_060> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public async override Task<object> UpgradeDoWorkAsync(object in_source)
        {
            ConfigureDefaultValues();

            string strSource = JsonSerializer.Serialize(in_source);
            Prompt050 source = JsonSerializer.Deserialize<Prompt050>(strSource);
            source.Version = source.Version ?? Version.Parse("0.5.0");

            Prompt060 target = JsonSerializer.Deserialize<Prompt060>(strSource);
            target.Version = target.Version ?? Version.Parse("0.6.0");

            if (source.Version == SourceInstanceVersion)
            {
                //TODO
                SetDefaultValues(target);

                target.Version = Version.Parse("0.6.0");

                _logger.LogInformation($"Upgraded {TypeName} : {source.Name} : from version {SourceInstanceVersion} to version {target.Version}");
            }

            return target;
        }

        public override Task<object> UpgradeProperties(object source) => Task.FromResult(source);
    }
}
