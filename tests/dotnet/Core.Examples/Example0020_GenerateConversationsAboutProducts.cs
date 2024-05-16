using FoundationaLLM.Core.Examples.Constants;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.Core.Examples.Resources;
using FoundationaLLM.Core.Examples.Setup;
using System.Text.Json;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
    /// <summary>
    /// Example class for sending user queries to a Knowledge Management with inline context agent using the LangChain orchestrator.
    /// </summary>
    public class Example0020_GenerateConversationsAboutProducts : BaseTest, IClassFixture<TestFixture>
	{
        private record Product
        {
            public required int Id { get; set; }
            public required string Type { get; set; }
            public required string Brand { get; set; }
            public required string Name { get; set; }
            public required string Description { get; set; }
            public required decimal Price { get; set; }
        }

        private record Conversation
        {
            public required int Id { get; set; }
            public required string Tone { get; set; }
            public required List<Product> TargetProducts { get; set; }
            public required List<ConversationMessage> Messages { get; set; }
        }

        private record ConversationMessage
        {
            public required string Question { get; set; }
            public required string Answer { get; set; }
        }

		private readonly IAgentConversationTestService _agentConversationTestService;
        private readonly List<Product> _products;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        private readonly List<string> _questionTones = [
            "neutral",
            "optimistic",
            "enthusiastic",
            "pessimistic",
            "obstructionist",
            "concise",
            "verbose"
        ];
        private readonly int _conversationsCount = 1000;

		public Example0020_GenerateConversationsAboutProducts(ITestOutputHelper output, TestFixture fixture)
			: base(output, fixture.ServiceProvider)
		{
            _agentConversationTestService = GetService<IAgentConversationTestService>();
            _products = JsonSerializer.Deserialize<List<Product>>(
                EmbeddedResource.Read("ProductCatalog.json"))!;
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
            var conversationStarters = Enumerable.Range(0, _conversationsCount)
                .Select(i => GetConversationStarter(i + 1))
                .ToList();

            foreach (var conversationStarter in conversationStarters)
            {
                var conversation = conversationStarter.Conversation;
                WriteLine($"Asking the agent {agentName} agent to create conversation # {conversation.Id}...");
                var response = await _agentConversationTestService.RunAgentCompletionWithNoSession(
                    agentName, conversationStarter.UserPrompt, createAgent: false);
                
                conversation.Messages = ParseConversation(response.Text!);
                foreach (var message in conversation.Messages)
                    Assert.True(message.Answer != TestResponseMessages.FailedCompletionResponse, $"An invalid agent response was found.");
                WriteLine($"Conversation # {conversation.Id} was created successfully.");
            }

            var conversations = conversationStarters.Select(cs => cs.Conversation).ToList();
            File.WriteAllText(
                "e://temp//cosmosdb-conversation-analytics-data.json",
                JsonSerializer.Serialize(conversations, _jsonSerializerOptions));
        }

        private (Conversation Conversation, string UserPrompt) GetConversationStarter(int id)
        {
            var indexes = Enumerable.Range(0, _products.Count).ToList();
            var selectedProducts =
                Enumerable.Range(0, Random.Shared.Next(1, 4))
                    .Select(i =>
                    {
                        var randomPosition = Random.Shared.Next(indexes.Count);
                        var product = _products[indexes[randomPosition]];
                        indexes.RemoveAt(randomPosition);
                        return product;
                    }).ToList();

            var conversation = new Conversation
            {
                Id = id,
                Tone = _questionTones[Random.Shared.Next(_questionTones.Count)],
                TargetProducts = selectedProducts,
                Messages = []
            };

            var prompt = string.Join(Environment.NewLine, [
                $"TONE: {conversation.Tone}",
                string.Empty,
                "TARGET_PRODUCTS:",
                JsonSerializer.Serialize(selectedProducts, _jsonSerializerOptions).Replace("{", "{{").Replace("}", "}}")
            ]);

            return new(conversation, prompt);
        }

        private List<ConversationMessage> ParseConversation(string text)
        {
            var result = new List<ConversationMessage>();
            var currentMessage = default(ConversationMessage);
            var currentTurn = string.Empty;
            using var sr = new StringReader(text);
            string line;
            while ((line = sr.ReadLine()!) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (line == "User:")
                    {
                        if (currentMessage != null)
                            result.Add(currentMessage);

                        currentMessage = new ConversationMessage
                        {
                            Answer = string.Empty,
                            Question = string.Empty
                        };
                        currentTurn = "question";
                    }
                    else if (line == "Agent:")
                    {
                        currentTurn = "answer";
                    }
                    else
                    {
                        switch (currentTurn)
                        {
                            case "question":
                                currentMessage!.Question = line;
                                break;
                            case "answer":
                                currentMessage!.Answer = line;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
