using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Utility.Upgrade.Configuration
{
    public class Configuration_050_060 : ConfigurationUpgrade
    {
        public Configuration_050_060(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<Configuration_050_060>();

            SourceInstanceVersion = Version.Parse("0.5.0");
        }

        private ILogger<Configuration_050_060> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public async override Task<object> UpgradeDoWorkAsync(object in_source) => null;

        public override Task<object> UpgradeProperties(object source) => Task.FromResult(source);
    }
}
