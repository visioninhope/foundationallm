using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Core.Examples.Setup;
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
		private BlobStorageServiceSettings _settings;
        private InstanceSettings _instanceSettings;
        private ILoggerFactory _loggerFactory;

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
            _instanceSettings = ServiceProvider.GetRequiredService<IOptions<InstanceSettings>>().Value;

            _loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();

            string encryptionKey = "";

            _settings = ServiceProvider.GetRequiredService<IOptionsMonitor<BlobStorageServiceSettings>>()
                    .Get(DependencyInjectionKeys.FoundationaLLM_ResourceProvider_Vectorization);

            //setup blob storage account (main)
            BlobStorageService blobStorageService = new BlobStorageService(Options.Create<BlobStorageServiceSettings>(_settings), ServiceProvider.GetRequiredService<ILogger<BlobStorageService>>());

            //setup blob storage account (auth)
            BlobStorageService authBlobStorageService = new BlobStorageService(Options.Create<BlobStorageServiceSettings>(_settings), ServiceProvider.GetRequiredService<ILogger<BlobStorageService>>());

            //create backup container
            //blobStorageService.CreateContainer("Backup");

            //backup agents
            
            //backup configuration

            //backup data sources
            
            //backup prompts

            //backup vectorization

            //backup vectorization (content sources)

            //backup vectorization (indexing)

            //backup indexes

            //backup vectorization (embedding)

            //backup vectorization (partitioning)

            //backup vectorization (state)

            //backup cosmos db

            //backup authorization
        }
    }
}
