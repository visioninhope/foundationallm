using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoundationaLLM.Common.Models.Configuration.CosmosDB;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
	public class Example01_AgentCompletions : BaseTest
	{
		public Example01_AgentCompletions(ITestOutputHelper output) : base(output)
		{
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

			// Assert
			Assert.NotNull(cosmosDbSettings.Endpoint);
		}
	}
}
