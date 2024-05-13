using FoundationaLLM.Core.Examples.Constants;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.Core.Examples.Setup;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
    /// <summary>
    /// Example class for the Knowledge Management agent with LangChain.
    /// </summary>
    public class Example0011_KnowledgeManagementAgentWithSemanticKernel : BaseTest, IClassFixture<TestFixture>
    {
        private readonly IAgentConversationTestService _agentConversationTestService;

        public Example0011_KnowledgeManagementAgentWithSemanticKernel(ITestOutputHelper output, TestFixture fixture)
            : base(output, fixture.ServiceProvider)
        {
            _agentConversationTestService = GetService<IAgentConversationTestService>();
        }

        [Fact]
        public async Task RunAsync()
        {
            WriteLine("============ Knowledge Management agent using LangChain ============");
            await RunExampleAsync();
        }

        private async Task RunExampleAsync()
        {
            var agentName = TestAgentNames.KnowledgeManagementWithLangChain;
            var userPrompts = new List<string>
            {
                "Who are you?",
                "What are some interesting facts about the San Diego Zoo?",
                "Which animal in the San Diego Zoo is the oldest?",
                "How does San Diego Zoo treat illness among it's inhabitants?"
            };

            WriteLine($"Send questions to the {agentName} agent.");

            var response = await _agentConversationTestService.RunAgentConversationWithSession(
                agentName, userPrompts, null, true);

            WriteLine($"Agent conversation history:");

            var invalidAgentResponsesFound = 0;
            foreach (var message in response)
            {
                WriteLine($"- {message.Sender}: {message.Text}");

                if (string.Equals(message.Sender, Common.Constants.Agents.InputMessageRoles.Assistant, StringComparison.CurrentCultureIgnoreCase) &&
                    message.Text == TestResponseMessages.FailedCompletionResponse)
                {
                    invalidAgentResponsesFound++;
                }
            }

            Assert.True(invalidAgentResponsesFound == 0, $"{invalidAgentResponsesFound} invalid agent responses found.");
        }
    }
}
