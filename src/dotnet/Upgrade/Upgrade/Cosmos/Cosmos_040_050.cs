using FoundationaLLM.Common.Models.Configuration.Instance;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Utility.Upgrade.Cosmos
{
    public class Cosmos_040_050 : CosmosUpgrade
    {
        public Cosmos_040_050(
            InstanceSettings settings,
            ILoggerFactory loggerFactory) : base(settings, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Cosmos_040_050>();

            SourceInstanceVersion = Version.Parse("0.4.0");
        }

        private ILogger<Cosmos_040_050> _logger;

        public void ConfigureDefaultValues() => base.ConfigureDefaultValues();

        public async override Task<object> UpgradeDoWorkAsync(object in_source) => null;

        public override Task<object> UpgradeProperties(object source) => Task.FromResult(source);
    }
}
