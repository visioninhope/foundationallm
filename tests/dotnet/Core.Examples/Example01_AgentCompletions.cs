using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Configuration.CosmosDB;
using FoundationaLLM.Core.Examples.Setup;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
    public class Example01_AgentCompletions : BaseTest, IClassFixture<TestFixture>
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public Example01_AgentCompletions(ITestOutputHelper output, TestFixture fixture)
			: base(output, fixture.ServiceProvider)
		{
			_httpClientFactory = GetService<IHttpClientFactory>();
		}

		[Fact]
		public async Task RunAsync()
		{
			this.WriteLine("============ Agent Completions ============");
			await RunExampleAsync();
		}

		public async Task RunExampleAsync()
		{
			// Arrange
			var cosmosDbSettings = TestConfiguration.CosmosDbSettings;
			var client = _httpClientFactory.CreateClient(HttpClients.CoreAPI);

			// Assert
			Assert.NotNull(cosmosDbSettings.Endpoint);
			Assert.NotNull(client.BaseAddress);
		}
	}
}
