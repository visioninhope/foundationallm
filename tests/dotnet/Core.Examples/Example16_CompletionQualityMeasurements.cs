using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Constants.Configuration;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.AzureAIService;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.Core.Examples.Models;
using FoundationaLLM.Core.Examples.Setup;
using FoundationaLLM.Core.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
    /// <summary>
    /// Example class for running agent completions and evaluating the quality of the completions using Azure AI Studio.
    /// </summary>
    public class Example16_CompletionQualityMeasurements : BaseTest, IClassFixture<TestFixture>
	{
		private readonly ISessionManager _sessionManager;
		private readonly ICosmosDbService _cosmosDbService;
		private readonly IAzureAIService _azureAIService;
		private readonly JsonSerializerOptions _jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();

		public Example16_CompletionQualityMeasurements(ITestOutputHelper output, TestFixture fixture)
			: base(output, fixture.ServiceProvider)
		{
            _sessionManager = GetService<ISessionManager>();
			_cosmosDbService = GetService<ICosmosDbService>();
			_azureAIService = GetService<IAzureAIService>();
		}

		[Fact]
		public async Task RunAsync()
		{
			WriteLine("============ Agent Completions ============");
			await RunExampleAsync();
		}

		private async Task RunExampleAsync()
		{
			var agentPrompts = TestConfiguration.CompletionQualityMeasurementConfiguration.AgentPrompts;
            var sessionsToDelete = new List<string>();
			if (agentPrompts == null || agentPrompts.Length == 0)
			{
				WriteLine("No agent prompts found. Make sure you enter them in testsettings.json.");
				return;
			}
			foreach (var agentPrompt in agentPrompts)
			{
				var sessionId = await RunAgentCompletionAsync(agentPrompt);
                if (agentPrompt.SessionConfiguration is {CreateNewSession: true})
                {
                    sessionsToDelete.Add(sessionId);
                }
			}

            // Delete the sessions that were created.
            foreach (var sessionId in sessionsToDelete)
            {
                await _sessionManager.DeleteSessionAsync(sessionId);
            }
		}

		private async Task<string> RunAgentCompletionAsync(AgentPrompt agentPrompt)
        {
            var sessionId = agentPrompt.SessionConfiguration?.SessionId ?? string.Empty;
            WriteLine($"Agent: {agentPrompt.AgentName}");

			if (agentPrompt.SessionConfiguration is {CreateNewSession: true})
			{
                try
                {
					sessionId = await _sessionManager.CreateSessionAsync();
                    agentPrompt.SessionConfiguration.SessionId = sessionId;
                }
                catch (Exception e)
                {
                    WriteLine(e.Message);
                    throw;
                }
			}
			
			if (agentPrompt.SessionConfiguration != null)
			{
				var orchestrationRequest = new OrchestrationRequest
				{
					SessionId = agentPrompt.SessionConfiguration.SessionId,
					AgentName = agentPrompt.AgentName,
					UserPrompt = agentPrompt.UserPrompt ?? string.Empty,
					Settings = null
				};

                try
                {
					var completionResponse = await _sessionManager.SendCompletionRequestAsync(orchestrationRequest);
                    
                    var session =
                        await _cosmosDbService.GetSessionAsync(agentPrompt.SessionConfiguration.SessionId!);
                    var messages = await _cosmosDbService.GetSessionMessagesAsync(session.SessionId, session.UPN);
                    // Get the last message where the agent is the sender.
                    var lastAgentMessage = messages.LastOrDefault(m => m.Sender == nameof(Participants.Assistant));
                    if (lastAgentMessage != null && !string.IsNullOrWhiteSpace(lastAgentMessage.CompletionPromptId))
                    {
                        // Get the completion prompt from the last agent message.
                        var completionPrompt = await _cosmosDbService.GetCompletionPrompt(session.SessionId,
                            lastAgentMessage.CompletionPromptId);
                        // For the context, take everything in the prompt that comes after `\\n\\nContext:\\n`. If it doesn't exist, take the whole prompt.
                        var contextIndex =
                            completionPrompt.Prompt.IndexOf(@"\n\nContext:\n", StringComparison.Ordinal);
                        if (contextIndex != -1)
                        {
                            completionPrompt.Prompt = completionPrompt.Prompt[(contextIndex + 14)..];
                        }

                        var dataSet = new InputsMapping
                        {
                            Question = agentPrompt.UserPrompt,
                            Answer = completionResponse?.Text,
                            Context = completionPrompt.Prompt,
                            GroundTruth = agentPrompt.ExpectedCompletion,
                        };
                        // Create a new Azure AI evaluation from the data.
                        var dataSetName = $"{agentPrompt.AgentName}_{session.SessionId}";
                        var dataSetPath = await _azureAIService.CreateDataSet(dataSet, dataSetName);
                        var dataSetVersion = await _azureAIService.CreateDataSetVersion(dataSetName, dataSetPath);
                        _ = int.TryParse(dataSetVersion.DataVersion.VersionId, out var dataSetVersionNumber);
                        var jobId = await _azureAIService.SubmitJob(dataSetName, dataSetName,
                            dataSetVersionNumber == 0 ? 1 : dataSetVersionNumber,
                            string.Empty);
                        WriteLine($"Azure AI evaluation Job ID -> {jobId}");

                        WriteLine($"User prompt -> '{agentPrompt.UserPrompt}'");
                        WriteLine($"Agent completion -> '{completionResponse?.Text}'");
                        WriteLine($"Expected completion -> '{agentPrompt.ExpectedCompletion}'");
                        WriteLine("-------------------------------");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return sessionId;
        }
	}
}
