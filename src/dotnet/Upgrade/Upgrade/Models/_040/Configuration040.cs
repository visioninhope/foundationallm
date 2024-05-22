using FoundationaLLM.Common.Models.Configuration.AppConfiguration;
using FoundationaLLM.Configuration.Catalog;

namespace FoundationaLLM.Utility.Upgrade.Models._040
{
    public class Configuration040
    {
        Version _target;

        public Configuration040() => _target = Version.Parse("0.4.0");

        public List<AppConfigurationEntry> GetVersionEntries()
        {
            List<AppConfigurationEntry> entries = AppConfigurationCatalog.GetRequiredConfigurationsForVersion(_target.ToString()).ToList();
            return entries;
        }
    }
}
