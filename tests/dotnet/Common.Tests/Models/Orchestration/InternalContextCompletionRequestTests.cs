using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.ResourceProviders.Agent;

namespace FoundationaLLM.Common.Tests.Models.Orchestration
{
    public class InternalContextCompletionRequestTests
    {
        [Fact]
        public void InternalContextCompletionRequest_Agent_Property_Test()
        {
            // Arrange
            var request = new LLMCompletionRequest() 
                { Agent = new InternalContextAgent() { Name = "Test_agent", ObjectId = "Test_objectid", Type = AgentTypes.InternalContext }, 
                    UserPrompt = "",
                    OperationId = Guid.NewGuid().ToString()};

            var agent = new InternalContextAgent() { Name = "Test_agent", ObjectId = "Test_objectid", Type = AgentTypes.InternalContext };

            // Act
            request.Agent = agent;

            // Assert
            Assert.Equal(agent, request.Agent);
        }
    }
}
