using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using FoundationaLLM.Utility.Upgrade.Models._040;
using FoundationaLLM.Utility.Upgrade.Models._050;
using FoundationaLLM.Utility.Upgrade.Models._051;
using FoundationaLLM.Utility.Upgrade.Models._060;

namespace FoundationaLLM.Utility.Upgrade.DataSource
{
    public class DataSource_040_050 : DatasourceUpgrade
    {
        public DataSource_040_050(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<DataSource_040_050>();

            SourceInstanceVersion = Version.Parse("0.4.0");

            SourceType = typeof(DataSourceBase040);
            TargetType = typeof(DataSourceBase050);
        }

        private ILogger<DataSource_040_050> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public override Task<Dictionary<string, string>> LoadArtifacts() => base.LoadArtifacts();

        public async override Task<object> UpgradeDoWorkAsync(object in_source)
        {
            ConfigureDefaultValues();

            string strSource = JsonSerializer.Serialize(in_source);
            DataSourceBase040 source = JsonSerializer.Deserialize<DataSourceBase040>(strSource);
            source.Version = source.Version ?? Version.Parse("0.4.0");

            DataSourceBase050 target = JsonSerializer.Deserialize<DataSourceBase050>(strSource);
            target.Version = target.Version ?? Version.Parse("0.5.0");

            if (source.Version == SourceInstanceVersion)
            {
                //TODO
                SetDefaultValues(target);

                target.Version = Version.Parse("0.5.0");

                _logger.LogInformation($"Upgraded {source.Name} from version {SourceInstanceVersion} to version {target.Version}");
            }

            return target;
        }

        public override Task<object> UpgradeProperties(object source) => Task.FromResult(source);
    }
}
