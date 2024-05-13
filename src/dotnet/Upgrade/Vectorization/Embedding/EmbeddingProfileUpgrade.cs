using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Services.Storage;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace FoundationaLLM.Upgrade.Vectorization.Indexing
{
    public class EmbeddingProfileUpgrade : Upgrade
    {
        public EmbeddingProfileUpgrade(BlobStorageService blobStorageService,
            InstanceSettings instanceSettings,
            ILoggerFactory loggerFactory)
        {
            ObjectStartUpgradeVersion = Version.Parse("0.4.0");

            _blobStorageService = blobStorageService;
            _datasources = new Dictionary<string, string>();
            _dataSourceObjects = new Dictionary<string, object>();
            _instanceSettings = instanceSettings;

            TargetInstanceVersion = Version.Parse(_instanceSettings.Version);

            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<EmbeddingProfileUpgrade>();

            _defaultValues = new Dictionary<string, object>();
        }

        protected ILogger<EmbeddingProfileUpgrade> _logger;
        protected BlobStorageService _blobStorageService;

        protected Dictionary<string, string> _datasources { get; set; }
        protected Dictionary<string, object> _dataSourceObjects { get; set; }

        public void ConfigureDefaultValues()
        {
            if (_defaultValues == null)
                _defaultValues = new Dictionary<string, object>();

            _defaultValues.Add("CreatedBy", "System");
            _defaultValues.Add("CreatedOn", DateTime.Now.ToString());
            _defaultValues.Add("DisplayName", "{Name}");
            _defaultValues.Add("UpdatedBy", "System");
            _defaultValues.Add("UpdatedOn", DateTime.Now.ToString());

            _defaultValues.Add("Description", "{Name}");
            _defaultValues.Add("DataDescription", "{DataDescription}");
        }

        public virtual async Task<Dictionary<string, string>> LoadArtifacts()
        {
            var fileContent = await _blobStorageService.ReadFileAsync("resource-provider", "FoundationaLLM.Vectorization/vectorization-text-embedding-profiles.json");

            JsonElement referenceStore = JsonSerializer.Deserialize<dynamic>(Encoding.UTF8.GetString(fileContent.ToArray()));

            JsonElement data = referenceStore.GetProperty("Resources");

            foreach (var reference in data.EnumerateArray())
            {
                string fileName = reference.GetProperty("Filename").ToString();
                string type = reference.GetProperty("Type").ToString();

                string agent = Encoding.UTF8.GetString(_blobStorageService.ReadFileAsync("resource-provider", fileName).Result.ToArray());

                JsonNode jn = JsonNode.Parse(agent);
                JsonObject jo = jn.AsObject();

                if (jo["type"] == null)
                    jo.Add("type", type);

                _datasources.Add(fileName, jo.ToString());
            }

            return _datasources;
        }

        public virtual async Task<object> UpgradeProperties(object agent) => Task.FromResult(agent);

        public async override Task LoadAsync() =>
            //load all agents from storage
            await LoadArtifacts();

        public async override Task SaveAsync()
        {
            //save all agents to storage
            foreach (string agentName in _dataSourceObjects.Keys)
            {
                object agent = _dataSourceObjects[agentName];
                string strAgent = JsonSerializer.Serialize(agent);
                byte[] dataSetBytes = Encoding.UTF8.GetBytes(strAgent);
                Stream stream = new MemoryStream(dataSetBytes);

                string fileName = agentName;

                if (!fileName.Contains("FoundationaLLM.Vectorization"))
                    fileName = $"FoundationaLLM.Vectorization/{fileName}";

                await _blobStorageService.WriteFileAsync("resource-provider", $"{fileName}", stream, default, default);
            }
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
                        string type = $"FoundationaLLM.Common.Upgrade.Vectorization.Embedding.Vectorization_{agentVersion.ToString().Replace(".", "")}_{targetVersion.ToString().Replace(".", "")}";
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

                        var upgrader = (EmbeddingProfileUpgrade)Activator.CreateInstance(t, new object[] { _blobStorageService, _instanceSettings, _loggerFactory });

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
