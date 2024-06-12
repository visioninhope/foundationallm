using FoundationaLLM.Client.Core.Interfaces;
using FoundationaLLM.Common.Models.Chat;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;
using FoundationaLLM.Common.Models.ResourceProviders;
using NSubstitute;

namespace FoundationaLLM.Client.Core.Tests
{
    public class CoreClientTests
    {
        private readonly ICoreRESTClient _coreRestClient;
        private readonly CoreClient _coreClient;

        public CoreClientTests()
        {
            _coreRestClient = Substitute.For<ICoreRESTClient>();
            _coreClient = new CoreClient(_coreRestClient);
        }

        [Fact]
        public async Task CreateChatSessionAsync_WithName_CreatesAndRenamesSession()
        {
            // Arrange
            var token = "valid-token";
            var sessionName = "TestSession";
            var sessionId = "session-id";
            _coreRestClient.Sessions.CreateSessionAsync(token).Returns(Task.FromResult(sessionId));

            // Act
            var result = await _coreClient.CreateChatSessionAsync(sessionName, token);

            // Assert
            Assert.Equal(sessionId, result);
            await _coreRestClient.Sessions.Received(1).CreateSessionAsync(token);
            await _coreRestClient.Sessions.Received(1).RenameChatSession(sessionId, sessionName, token);
        }

        [Fact]
        public async Task SendCompletionWithSessionAsync_WithNewSession_CreatesSessionAndSendsCompletion()
        {
            // Arrange
            var token = "valid-token";
            var userPrompt = "Hello, World!";
            var agentName = "TestAgent";
            var sessionId = "new-session-id";
            var completion = new Completion();
            _coreRestClient.Sessions.CreateSessionAsync(token).Returns(Task.FromResult(sessionId));
            _coreRestClient.Sessions.SendSessionCompletionRequestAsync(Arg.Any<OrchestrationRequest>(), token).Returns(Task.FromResult(completion));

            // Act
            var result = await _coreClient.SendCompletionWithSessionAsync(null, "NewSession", userPrompt, agentName, token);

            // Assert
            Assert.Equal(completion, result);
            await _coreRestClient.Sessions.Received(1).CreateSessionAsync(token);
            await _coreRestClient.Sessions.Received(1).SendSessionCompletionRequestAsync(Arg.Is<OrchestrationRequest>(
                r => r.SessionId == sessionId && r.AgentName == agentName && r.UserPrompt == userPrompt), token);
        }

        [Fact]
        public async Task SendSessionlessCompletionAsync_ValidRequest_SendsCompletion()
        {
            // Arrange
            var token = "valid-token";
            var userPrompt = "Hello, World!";
            var agentName = "TestAgent";
            var completion = new Completion();
            _coreRestClient.Orchestration.SendOrchestrationCompletionRequestAsync(Arg.Any<CompletionRequest>(), token).Returns(Task.FromResult(completion));

            // Act
            var result = await _coreClient.SendSessionlessCompletionAsync(userPrompt, agentName, token);

            // Assert
            Assert.Equal(completion, result);
            await _coreRestClient.Orchestration.Received(1).SendOrchestrationCompletionRequestAsync(Arg.Is<CompletionRequest>(
                r => r.AgentName == agentName && r.UserPrompt == userPrompt), token);
        }

        [Fact]
        public async Task AttachFileAndAskQuestionAsync_UsesSession_UploadsFileAndSendsSessionCompletion()
        {
            // Arrange
            var token = "valid-token";
            var fileStream = new MemoryStream();
            var fileName = "test.txt";
            var contentType = "text/plain";
            var agentName = "TestAgent";
            var question = "What is this file about?";
            var sessionId = "session-id";
            var objectId = "object-id";
            var completion = new Completion();
            _coreRestClient.Attachments.UploadAttachmentAsync(fileStream, fileName, contentType, token).Returns(Task.FromResult(objectId));
            _coreRestClient.Sessions.CreateSessionAsync(token).Returns(Task.FromResult(sessionId));
            _coreRestClient.Sessions.SendSessionCompletionRequestAsync(Arg.Any<OrchestrationRequest>(), token).Returns(Task.FromResult(completion));

            // Act
            var result = await _coreClient.AttachFileAndAskQuestionAsync(fileStream, fileName, contentType, agentName, question, true, null, "NewSession", token);

            // Assert
            Assert.Equal(completion, result);
            await _coreRestClient.Attachments.Received(1).UploadAttachmentAsync(fileStream, fileName, contentType, token);
            await _coreRestClient.Sessions.Received(1).CreateSessionAsync(token);
            await _coreRestClient.Sessions.Received(1).SendSessionCompletionRequestAsync(Arg.Is<OrchestrationRequest>(
                r => r.AgentName == agentName && r.SessionId == sessionId && r.UserPrompt == question && r.Attachments.Contains(objectId)), token);
        }

        [Fact]
        public async Task GetChatSessionMessagesAsync_ValidRequest_ReturnsMessages()
        {
            // Arrange
            var token = "valid-token";
            var sessionId = "session-id";
            var messages = new List<Message> { new Message(sessionId, "TestSender", null, "Hello", null, null, "test@foundationallm.ai") };
            _coreRestClient.Sessions.GetChatSessionMessagesAsync(sessionId, token).Returns(Task.FromResult<IEnumerable<Message>>(messages));

            // Act
            var result = await _coreClient.GetChatSessionMessagesAsync(sessionId, token);

            // Assert
            Assert.Equal(messages, result);
            await _coreRestClient.Sessions.Received(1).GetChatSessionMessagesAsync(sessionId, token);
        }

        [Fact]
        public async Task GetAgentsAsync_ValidRequest_ReturnsAgents()
        {
            // Arrange
            var token = "valid-token";
            var agents = new List<ResourceProviderGetResult<AgentBase>> { new ResourceProviderGetResult<AgentBase>
                {
                    Resource = new KnowledgeManagementAgent
                    {
                        Name = "TestAgent",
                        Description = "Test Agent Description"
                    },
                    Actions = [],
                    Roles = []
                }
            };
            _coreRestClient.Orchestration.GetAgentsAsync(token).Returns(Task.FromResult<IEnumerable<ResourceProviderGetResult<AgentBase>>>(agents));

            // Act
            var result = await _coreClient.GetAgentsAsync(token);

            // Assert
            Assert.Equal(agents, result);
            await _coreRestClient.Orchestration.Received(1).GetAgentsAsync(token);
        }

        [Fact]
        public async Task DeleteSessionAsync_ValidRequest_DeletesSession()
        {
            // Arrange
            var token = "valid-token";
            var sessionId = "session-id";

            // Act
            await _coreClient.DeleteSessionAsync(sessionId, token);

            // Assert
            await _coreRestClient.Sessions.Received(1).DeleteSessionAsync(sessionId, token);
        }
    }
}