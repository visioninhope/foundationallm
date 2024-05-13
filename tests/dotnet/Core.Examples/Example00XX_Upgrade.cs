using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Core.Examples.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;
using FoundationaLLM.Upgrade.Configuration;
using FoundationaLLM.Upgrade.Agent;
using FoundationaLLM.Upgrade.DataSource;
using FoundationaLLM.Upgrade.Prompts;
using FoundationaLLM.Upgrade.Cosmos;
using FoundationaLLM.Upgrade.Vectorization.Indexing;
using FoundationaLLM.Upgrade.Authorization;

namespace FoundationaLLM.Core.Examples
{
    /// <summary>
    /// Example class for running agent completions and evaluating the quality of the completions using Azure AI Studio.
    /// </summary>
    public class Example00XX_Upgrade : BaseTest, IClassFixture<TestFixture>
	{
		private BlobStorageServiceSettings _settings;
        private InstanceSettings _instanceSettings;
        private ILoggerFactory _loggerFactory;

        public Example00XX_Upgrade(ITestOutputHelper output, 
            TestFixture fixture            
            )
			: base(output, fixture.ServiceProvider)
		{            
        }

		[Fact]
        public async Task RunAsync()
        {
            WriteLine("============ Upgrade ============");
			await RunExampleAsync();
		}

		private async Task RunExampleAsync()
		{
            _instanceSettings = ServiceProvider.GetRequiredService<IOptions<InstanceSettings>>().Value;

            _loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();

            _settings = ServiceProvider.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization);

            //setup blob storage account (main)
            BlobStorageService blobStorageService = new BlobStorageService(Options.Create<BlobStorageServiceSettings>(_settings), ServiceProvider.GetRequiredService<ILogger<BlobStorageService>>());

            //setup blob storage account (auth)
            BlobStorageService authBlobStorageService = new BlobStorageService(Options.Create<BlobStorageServiceSettings>(_settings), ServiceProvider.GetRequiredService<ILogger<BlobStorageService>>());

            //create upgrade container
            //blobStorageService.CreateContainer("Backup");

            //upgrade agents
            AgentUpgrade agentUpgrade = new AgentUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            //await agentUpgrade.UpgradeAsync();

            //upgrade configuration
            ConfigurationUpgrade configUpgrade = new ConfigurationUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            //await configUpgrade.UpgradeAsync();

            //upgrade data sources
            DatasourceUpgrade dsUpgrade = new DatasourceUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            //await dsUpgrade.UpgradeAsync();

            //upgrade prompts
            PromptUpgrade promptUpgrade = new PromptUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            await promptUpgrade.UpgradeAsync();

            //upgrade vectorization

            //upgrade vectorization (content sources)
            ContentSourceProfileUpgrade contentSourceProfileUpgrade = new ContentSourceProfileUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            //contentSourceProfileUpgrade.UpgradeAsync().Wait();

            //upgrade vectorization (indexing)
            IndexingProfileUpgrade indexingProfileUpgrade = new IndexingProfileUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            //indexingProfileUpgrade.UpgradeAsync().Wait();

            //upgrade indexes

            //upgrade vectorization (embedding)
            EmbeddingProfileUpgrade embeddingProfileUpgrade = new EmbeddingProfileUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            //embeddingProfileUpgrade.UpgradeAsync().Wait();

            //upgrade vectorization (partitioning)
            PartitioningProfileUpgrade paritioningProfileUpgrade = new PartitioningProfileUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            //paritioningProfileUpgrade.UpgradeAsync().Wait();

            //upgrade vectorization (state)
            StateUpgrade stateUpgrade = new StateUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            //stateUpgrade.UpgradeAsync().Wait();

            //upgrade cosmos db
            CosmosUpgrade cosmosUpgrade = new CosmosUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            //await cosmosUpgrade.UpgradeAsync();

            //upgrade authorization
            AuthorizationUpgrade authorizationUpgrade = new AuthorizationUpgrade(blobStorageService, _instanceSettings, _loggerFactory);
            //await authorizationUpgrade.UpgradeAsync();
        }
    }
}
