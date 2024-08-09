using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Core.Examples.Setup;
using FoundationaLLM.Utility.Backup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
    /// <summary>
    /// Example class for running agent completions and evaluating the quality of the completions using Azure AI Studio.
    /// </summary>
    public class Example00XX_BackupRestore : BaseTest, IClassFixture<TestFixture>
	{
		private BlobStorageServiceSettings _instanceStorageSettings;
        private BlobStorageServiceSettings _backupStorageSettings;
        private BlobStorageServiceSettings _authStorageSettings;
        private InstanceSettings _instanceSettings;
        private ILoggerFactory _loggerFactory;
        private IHttpClientFactory _httpClientFactory;
        IConfigurationRoot _configRoot;

        public Example00XX_BackupRestore(ITestOutputHelper output, 
            TestFixture fixture            
            )
			: base(output, fixture.ServiceProvider)
		{            
        }

		[Fact]
        public async Task RunAsync()
        {
            WriteLine("============ Backup/Restore ============");
			await RunExampleAsync();
		}

		private async Task RunExampleAsync()
		{
            _configRoot = ServiceProvider.GetRequiredService<IConfigurationRoot>();

            _instanceSettings = ServiceProvider.GetRequiredService<IOptions<InstanceSettings>>().Value;

            _loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();

            _httpClientFactory = ServiceProvider.GetRequiredService<IHttpClientFactory>();

            _instanceStorageSettings = ServiceProvider.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>().Get(DependencyInjectionKeys.FoundationaLLM_ResourceProviders_Vectorization);
            
            _backupStorageSettings = ServiceProvider.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>().Get(DependencyInjectionKeys.FoundationaLLM_Backup);

            _authStorageSettings = ServiceProvider.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>().Get(AuthorizationDependencyInjectionKeys.FoundationaLLM_ResourceProviders_Authorization);

            BlobStorageService instanceStorageService = new BlobStorageService(Options.Create<BlobStorageServiceSettings>(_instanceStorageSettings), ServiceProvider.GetRequiredService<ILogger<BlobStorageService>>());

            //setup blob storage account (main)
            BlobStorageService backupStorageService = new BlobStorageService(Options.Create<BlobStorageServiceSettings>(_backupStorageSettings), ServiceProvider.GetRequiredService<ILogger<BlobStorageService>>());            

            //setup blob storage account (auth)
            //BlobStorageService authBlobStorageService = new BlobStorageService(Options.Create<BlobStorageServiceSettings>(_authStorageSettings), ServiceProvider.GetRequiredService<ILogger<BlobStorageService>>());

            //create backup container
            //blobStorageService.CreateContainer("Backup");

            BackupConfig config = new BackupConfig();
            config.SubscriptionId = "5ca9085f-8515-44fb-90c0-f563b03fc302";
            config.ResourceGroup = "rg-fllm";
            config.TenantId = "d280491c-b27a-41bf-9623-21b60cf430b3";
            config.DeploymentMode = "ACA";
            config.EncryptionString = "HelloWorld";
            config.AgentsEnabled = true;
            config.ConfigurationEnabled = true;
            config.PromptsEnabled = true;
            config.AuthorizationEnabled = true;
            config.VectorizationEnabled = true;
            config.CosmosEnabled = true;
            config.CosmosDatabases = new List<string> { "database" };
            config.CosmosCollections = new List<string> { "leases", "Sessions", "UserProfiles", "UserSessions", "State" };
            config.AppConfigFilters = new List<string> { "FoundationaLLM:DataSources:*" };
                
            BackupManager bm = new BackupManager(_loggerFactory, backupStorageService, instanceStorageService, _instanceSettings, config, _httpClientFactory, _configRoot);

            //backup agents
            //await bm.BackupAgents();

            //backup configuration
            //await bm.BackupConfiguration();

            //backup data sources
            //bm.BackupDataSources();

            //backup prompts
            //await bm.BackupPrompts();

            //backup vectorization
            //await bm.BackupVectorization();

            //backup vectorization (content sources)

            //backup vectorization (indexing)

            //backup indexes

            //backup vectorization (embedding)

            //backup vectorization (partitioning)

            //backup vectorization (state)

            //backup cosmos db
            await bm.BackupCosmos();

            //backup authorization
            //await bm.BackupAuthorization();

            //restart all services
            //await bm.RestartServices();
        }
    }
}
