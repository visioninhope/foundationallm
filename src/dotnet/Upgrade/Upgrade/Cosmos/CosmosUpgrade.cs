using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Utility.Upgrade;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FoundationaLLM.Utility.Upgrade.Cosmos
{
    public class CosmosUpgrade : Upgrade
    {
        public CosmosUpgrade(
            InstanceSettings instanceSettings,
            ILoggerFactory loggerFactory)
        {
            ObjectStartUpgradeVersion = Version.Parse("0.4.0");

            _datasources = new Dictionary<string, string>();
            _dataSourceObjects = new Dictionary<string, object>();
            _instanceSettings = instanceSettings;

            TargetInstanceVersion = Version.Parse(_instanceSettings.Version);

            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<CosmosUpgrade>();

            _defaultValues = new Dictionary<string, object>();
        }

        protected ILogger<CosmosUpgrade> _logger;

        protected Dictionary<string, string> _datasources { get; set; }
        protected Dictionary<string, object> _dataSourceObjects { get; set; }

        public void ConfigureDefaultValues()
        {
            if (_defaultValues == null)
                _defaultValues = new Dictionary<string, object>();
        }

        public async Task<Dictionary<string, string>> LoadArtifacts() => _datasources;

        public virtual async Task<object> UpgradeProperties(object agent) => Task.FromResult(agent);

        public async override Task LoadAsync() =>
            await LoadArtifacts();

        public async override Task SaveAsync()
        {

        }

        public async override Task UpgradeAsync()
        {
            await LoadAsync();

            foreach (string name in _datasources.Keys)
            {
                string strSource = _datasources[name];

                object source = JsonSerializer.Deserialize<object>(strSource);

                Version agentVersion = GetObjectVersion(source);

                Version targetVersion = FLLMVersions.NextVersion(agentVersion);

                while (agentVersion < TargetInstanceVersion)
                {
                    //Load the target agent upgrade class
                    try
                    {
                        string type = $"FoundationaLLM.Common.Upgrade.Cosmos.Cosmos_{agentVersion.ToString().Replace(".", "")}_{targetVersion.ToString().Replace(".", "")}";
                        Type t = Type.GetType(type);

                        if (t == null)
                        {
                            _logger.LogWarning($"No upgrade path found for {name} from {agentVersion} to {targetVersion}");

                            targetVersion = FLLMVersions.NextVersion(targetVersion);

                            //reached the end...
                            if (targetVersion == Version.Parse("0.0.0"))
                                break;

                            continue;
                        }

                        var upgrader = (Upgrade)Activator.CreateInstance(t, new object[] { _instanceSettings, _loggerFactory });

                        source = await upgrader.UpgradeDoWorkAsync(source);

                        agentVersion = GetObjectVersion(source);

                        targetVersion = FLLMVersions.NextVersion(agentVersion);
                    }
                    catch (Exception ex)
                    {
                        //try to move to the next version to see if an upgrade path exists
                        targetVersion = FLLMVersions.NextVersion(targetVersion);
                    }
                }

                _dataSourceObjects.Add(name, source);
            }

            await SaveAsync();

            return;
        }
        public override Task<object> UpgradeDoWorkAsync(object agent) => throw new NotImplementedException();
    }
}
