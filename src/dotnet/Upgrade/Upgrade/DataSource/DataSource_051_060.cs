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
    public class DataSource_051_060 : DatasourceUpgrade
    {
        public DataSource_051_060(BlobStorageService blobStorageService,
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(blobStorageService, settings, loggerFactory)
        {
            _blobStorageService = blobStorageService;
            _logger = loggerFactory.CreateLogger<DataSource_051_060>();

            SourceInstanceVersion = Version.Parse("0.5.1");

            SourceType = typeof(DataSourceBase051);
            TargetType = typeof(DataSourceBase060);
        }

        private ILogger<DataSource_051_060> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public async override Task<object> UpgradeDoWorkAsync(object in_source)
        {
            ConfigureDefaultValues();

            string strSource = JsonSerializer.Serialize(in_source);
            DataSourceBase051 source = JsonSerializer.Deserialize<DataSourceBase051>(strSource);
            source.Version = source.Version ?? Version.Parse("0.5.1");

            DataSourceBase060 target = JsonSerializer.Deserialize<DataSourceBase060>(strSource);
            target.Version = target.Version ?? Version.Parse("0.6.0");

            if (source.Version == SourceInstanceVersion)
            {
                //TODO
                SetDefaultValues(target);

                target.Version = Version.Parse("0.6.0");

                _logger.LogInformation($"Upgraded {source.Name} from version {SourceInstanceVersion} to version {target.Version}");
            }

            return target;
        }

        public override Task<object> UpgradeProperties(object source) => Task.FromResult(source);
    }
}
