using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime;
using System.Text;
using System.Text.Encodings.Web;
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
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Core.Examples.Models;
using FoundationaLLM.Core.Interfaces;
using FoundationaLLM.Core.Services;

namespace FoundationaLLM.Core.Examples
{
    public class Example01_AgentCompletions : BaseTest, IClassFixture<TestFixture>
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ICosmosDbService _cosmosDbService;
		private readonly IAzureAIService _azureAIService;
		private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

		public Example01_AgentCompletions(ITestOutputHelper output, TestFixture fixture)
			: base(output, fixture.ServiceProvider)
		{
			_httpClientFactory = GetService<IHttpClientFactory>();
			_cosmosDbService = GetService<ICosmosDbService>();
			_azureAIService = GetService<IAzureAIService>();
		}

		[Fact]
		public async Task RunAsync()
		{
			WriteLine("============ Agent Completions ============");
			await RunExampleAsync();
		}

		public async Task RunExampleAsync()
		{
			var agentPrompts = TestConfiguration.AgentPromptConfiguration.AgentPrompts;
			if (agentPrompts == null || agentPrompts.Length == 0)
			{
				WriteLine("No agent prompts found. Make sure you enter them in testsettings.json.");
				return;
			}
			foreach (var agentPrompt in agentPrompts)
			{
				await RunAgentCompletionAsync(agentPrompt);
			}
		}

		private async Task RunAgentCompletionAsync(AgentPrompt agentPrompt)
		{
			WriteLine($"Agent: {agentPrompt.AgentName}");

			var cosmosDbSettings = TestConfiguration.CosmosDbSettings;
			var client = await GetHttpClient();
			if (agentPrompt.SessionConfiguration.CreateNewSession)
			{
				var responseSession = await client.PostAsync("sessions", null);

				if (responseSession.IsSuccessStatusCode)
				{
					var responseContent = await responseSession.Content.ReadAsStringAsync();
					var sessionResponse = JsonSerializer.Deserialize<Session>(responseContent, _jsonSerializerOptions);
					agentPrompt.SessionConfiguration.SessionId = sessionResponse.SessionId;

					var sessionName = "Test: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
					var responseUpdate = await client.PostAsync($"sessions/{agentPrompt.SessionConfiguration.SessionId}/rename?newChatSessionName={UrlEncoder.Default.Encode(sessionName)}", null);

					if (!responseUpdate.IsSuccessStatusCode)
					{
						WriteLine("Failed to update the session name.");
					}
				}
			}
			var orchestrationSettings = new OrchestrationSettings();

			var orchestrationRequest = new OrchestrationRequest
			{
				SessionId = agentPrompt.SessionConfiguration.SessionId,
				AgentName = agentPrompt.AgentName,
				UserPrompt = agentPrompt.UserPrompt,
				Settings = orchestrationSettings
			};

			var serializedRequest = JsonSerializer.Serialize(orchestrationRequest, _jsonSerializerOptions);

			var orchestrationUrl = "orchestration/completion"; // Sessionless - no message history and data is not retained in Cosmos DB.
			var sessionUrl = $"sessions/{agentPrompt.SessionConfiguration.SessionId}/completion"; // Session-based - message history and data is retained in Cosmos DB. Must create a session if it does not exist.
			var responseMessage = await client.PostAsync(sessionUrl,
				new StringContent(
					serializedRequest,
					Encoding.UTF8, "application/json"));

			if (responseMessage.IsSuccessStatusCode)
			{
				var responseContent = await responseMessage.Content.ReadAsStringAsync();
				var completionResponse = JsonSerializer.Deserialize<Completion>(responseContent, _jsonSerializerOptions);

				var session = await _cosmosDbService.GetSessionAsync(agentPrompt.SessionConfiguration.SessionId);
				var messages = await _cosmosDbService.GetSessionMessagesAsync(session.SessionId, session.UPN);
				// Get the last message where the agent is the sender.
				var lastAgentMessage = messages.LastOrDefault(m => m.Sender == nameof(Participants.Assistant));
				if (lastAgentMessage != null && !string.IsNullOrWhiteSpace(lastAgentMessage.CompletionPromptId))
				{
					// Get the completion prompt from the last agent message.
					var completionPrompt = await _cosmosDbService.GetCompletionPrompt(session.SessionId, lastAgentMessage.CompletionPromptId);
					// Create a new Azure AI evaluation from the data.
				}

				WriteLine($"User prompt -> '{agentPrompt.UserPrompt}'");
				WriteLine($"Agent completion -> '{completionResponse.Text}'");
				WriteLine($"Expected completion -> '{agentPrompt.ExpectedCompletion}'");
				WriteLine($"Completions match -> {completionResponse.Text.Equals(agentPrompt.ExpectedCompletion)}");
				WriteLine("-------------------------------");
			}
		}

		private async Task<HttpClient> GetHttpClient()
		{
			var httpClient = _httpClientFactory.CreateClient(HttpClients.CoreAPI);
			var coreApiScope = TestConfiguration.GetAppConfigValueAsync(AppConfigurationKeys.FoundationaLLM_Chat_Entra_Scopes).GetAwaiter().GetResult();

			// The scope needs to just be the base URI, not the full URI.
			coreApiScope = coreApiScope[..coreApiScope.LastIndexOf('/')];

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
