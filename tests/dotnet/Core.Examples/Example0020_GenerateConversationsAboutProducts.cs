using FoundationaLLM.Core.Examples.Constants;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.Core.Examples.Setup;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
    /// <summary>
    /// Example class for sending user queries to a Knowledge Management with inline context agent using the LangChain orchestrator.
    /// </summary>
    public class Example0020_GenerateConversationsAboutProducts : BaseTest, IClassFixture<TestFixture>
	{
		private readonly IAgentConversationTestService _agentConversationTestService;

		public Example0020_GenerateConversationsAboutProducts(ITestOutputHelper output, TestFixture fixture)
			: base(output, fixture.ServiceProvider)
		{
            _agentConversationTestService = GetService<IAgentConversationTestService>();
		}

		[Fact]
		public async Task RunAsync()
		{
			WriteLine("============ Generate conversations about products ============");
			await RunExampleAsync();
		}

		private async Task RunExampleAsync()
        {
            var agentName = TestAgentNames.ConversationGeneratorAgent;
            var userPrompts = new List<string>
            {
                @"Alpine Fusion Goggles
Expedition Backpack"
            };

            foreach (var userPrompt in userPrompts)
            {
                WriteLine($"Ask the agent {agentName} agent to create the conversations.");
                var response = await _agentConversationTestService.RunAgentCompletionWithNoSession(
                    agentName, userPrompt, createAgent: true);
                WriteLine("User prompt:");
                WriteLine(userPrompt);
                WriteLine();
                WriteLine($"Conversation created by agent:");
                WriteLine(response.Text);
                Assert.True(response.Text != TestResponseMessages.FailedCompletionResponse, $"An invalid agent response was found.");
            }
        }
	}
}
