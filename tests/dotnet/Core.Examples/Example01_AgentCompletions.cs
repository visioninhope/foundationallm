using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Configuration.CosmosDB;
using FoundationaLLM.Core.Examples.Setup;
using Xunit.Abstractions;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Settings;
using System.Text.Json;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;
using Microsoft.Graph.Models;
using Azure.Identity;

namespace FoundationaLLM.Core.Examples
{
    public class Example01_AgentCompletions : BaseTest, IClassFixture<TestFixture>
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

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
			var client = await GetHttpClient();
			var sessionId = Guid.NewGuid().ToString();
			var agentName = "FoundationaLLM";
			var userPrompt = "Hello, who are you?";
			var orchestrationSettings = new OrchestrationSettings();

			var orchestrationRequest = new OrchestrationRequest
			{
				SessionId = sessionId,
				AgentName = agentName,
				UserPrompt = userPrompt,
				Settings = orchestrationSettings
			};

			var serializedRequest = JsonSerializer.Serialize(orchestrationRequest, _jsonSerializerOptions);
			var responseMessage = await client.PostAsync("orchestration/completion",
				new StringContent(
					serializedRequest,
					Encoding.UTF8, "application/json"));

			if (responseMessage.IsSuccessStatusCode)
			{
				var responseContent = await responseMessage.Content.ReadAsStringAsync();
				var completionResponse = JsonSerializer.Deserialize<Completion>(responseContent);

				// TODO: Retrieve the expected completion value from Cosmos DB and compare to the completion response.
			}

			// Assert
			Assert.NotNull(cosmosDbSettings.Endpoint);
			Assert.NotNull(client.BaseAddress);
		}

		private async Task<HttpClient> GetHttpClient()
		{
			var httpClient = _httpClientFactory.CreateClient(HttpClients.CoreAPI);
			var coreApiScope = TestConfiguration.GetAppConfigValueAsync(AppConfigurationKeys.FoundationaLLM_Chat_Entra_Scopes).GetAwaiter().GetResult();
			//if (!coreApiScope.EndsWith("/.default"))
			//{
			//	coreApiScope += "/.default";
			//}

			//var credentials = new AzureCliCredential();
			var credentials = TestConfiguration.GetTokenCredential();
			var tokenResult = await credentials.GetTokenAsync(
				new([coreApiScope]),
				default);

			httpClient.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", tokenResult.Token);

			return httpClient;
		}
	}
}
