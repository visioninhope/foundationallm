using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Upgrade.Configuration;
using FoundationaLLM.Upgrade.Models._040;
using FoundationaLLM.Upgrade.Models._050;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Upgrade.Configuration
{
    public class Configuration_040_050 : ConfigurationUpgrade
    {
        public Configuration_040_050(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<Configuration_040_050>();

            SourceInstanceVersion = Version.Parse("0.4.0");
        }

        private ILogger<Configuration_040_050> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public async override Task<object> UpgradeDoWorkAsync(object in_source) => null;

        public override Task<object> UpgradeProperties(object source) => Task.FromResult(source);
    }
}
