using FoundationaLLM.Agent.Models.Resources;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Upgrade;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace FoundationaLLM.Upgrade.Agent
{
    public class AgentUpgrade : Upgrade
    {
        public AgentUpgrade(BlobStorageService blobStorageService,
            InstanceSettings instanceSettings,
            ILoggerFactory loggerFactory)
        {
            ObjectStartUpgradeVersion = Version.Parse("0.4.0");

            _blobStorageService = blobStorageService;
            _agents = new Dictionary<string, string>();
            _agentObjects = new Dictionary<string, object>();
            _instanceSettings = instanceSettings;

            TargetInstanceVersion = Version.Parse(_instanceSettings.Version);

            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<AgentUpgrade>();
        }

        protected ILogger<AgentUpgrade> _logger;
        protected BlobStorageService _blobStorageService;

        protected Dictionary<string, string> _agents { get; set; }
        protected Dictionary<string, object> _agentObjects { get; set; }

        public void ConfigureDefaultValues()
        {
            if (_defaultValues == null)
                _defaultValues = new Dictionary<string, object>();

            _defaultValues.Add("CreatedBy", "System");
            _defaultValues.Add("CreatedOn", DateTime.Now.ToString());
            _defaultValues.Add("DisplayName", "{Name}");
            _defaultValues.Add("UpdatedBy", "System");
            _defaultValues.Add("UpdatedOn", DateTime.Now.ToString());
        }

        public virtual async Task<Dictionary<string, string>> LoadArtifacts()
        {
            var fileContent = await _blobStorageService.ReadFileAsync("resource-provider", "FoundationaLLM.Agent/_agent-references.json");

            AgentReferenceStore agentReferenceStore = JsonSerializer.Deserialize<AgentReferenceStore>(Encoding.UTF8.GetString(fileContent.ToArray()));

            agentReferenceStore.AgentReferences.ForEach(agentReference =>
            {
                string agent = Encoding.UTF8.GetString(_blobStorageService.ReadFileAsync("resource-provider", agentReference.Filename).Result.ToArray());
                _agents.Add(agentReference.Filename, agent);
            });

            return _agents;
        }

        public virtual async Task<object> UpgradeProperties(object agent) => Task.FromResult(agent);

        public async override Task LoadAsync() =>
            //load all agents from storage
            await LoadArtifacts();

        public async override Task SaveAsync()
        {
            //save all agents to storage
            foreach (string agentName in _agentObjects.Keys)
            {
                object agent = _agentObjects[agentName];
                string strAgent = JsonSerializer.Serialize(agent);
                byte[] dataSetBytes = Encoding.UTF8.GetBytes(strAgent);
                Stream stream = new MemoryStream(dataSetBytes);

                string fileName = agentName;

                if (!fileName.Contains("FoundationaLLM.Agent"))
                    fileName = $"FoundationaLLM.Agent/{fileName}";

                await _blobStorageService.WriteFileAsync("resource-provider", $"{fileName}", stream, default, default);
            }
        }

        public async override Task UpgradeAsync()
        {
            await LoadAsync();

            foreach (string agentName in _agents.Keys)
            {
                string strAgent = _agents[agentName];

                object agent = JsonSerializer.Deserialize<object>(strAgent);

                Version agentVersion = GetObjectVersion(agent);

                Version targetVersion = FLLMVersions.NextVersion(agentVersion);

                while (agentVersion < TargetInstanceVersion)
                {
                    //Load the target agent upgrade class
                    try
                    {
                        string type = $"FoundationaLLM.Common.Upgrade.Agent_{agentVersion.ToString().Replace(".", "")}_{targetVersion.ToString().Replace(".", "")}";
                        Type t = Type.GetType(type);

                        if (t == null)
                        {
                            targetVersion = FLLMVersions.NextVersion(targetVersion);

                            //reached the end...
                            if (targetVersion == Version.Parse("0.0.0"))
                                break;

                            continue;
                        }

                        var agentUpgrade = (AgentUpgrade)Activator.CreateInstance(t, new object[] { _blobStorageService, _instanceSettings, _loggerFactory });

                        agent = await agentUpgrade.UpgradeDoWorkAsync(agent);

                        agentVersion = GetObjectVersion(agent);

                        targetVersion = FLLMVersions.NextVersion(agentVersion);
                    }
                    catch (Exception ex)
                    {
                        //try to move to the next version to see if an upgrade path exists
                        targetVersion = FLLMVersions.NextVersion(targetVersion);
                    }
                }

                _agentObjects.Add(agentName, agent);
            }

            await SaveAsync();

            return;
        }

        public override Task<object> UpgradeDoWorkAsync(object agent) => throw new NotImplementedException();
    }
}
