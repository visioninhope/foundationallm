using FoundationaLLM.Client.Management.Clients.Resources;
using FoundationaLLM.Client.Management.Interfaces;
using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.AIModel;
using NSubstitute;

namespace Management.Client.Tests.Clients.Resources
{
    public class AIModelManagementClientTests
    {
        private readonly IManagementRESTClient _mockRestClient;
        private readonly AIModelManagementClient _aiModelClient;

        public AIModelManagementClientTests()
        {
            _mockRestClient = Substitute.For<IManagementRESTClient>();
            _aiModelClient = new AIModelManagementClient(_mockRestClient);
        }

        [Fact]
        public async Task UpsertAIModel_ShouldReturnUpsertResult()
        {
            // Arrange
            var aiModel = new AIModelBase
            {
                Name = "test-configuration",
                EndpointObjectId = ""
            };
            var expectedUpsertResult = new ResourceProviderUpsertResult
            {
                ObjectId = "test-object-id"
            };

            _mockRestClient.Resources
                .UpsertResourceAsync(
                    ResourceProviderNames.FoundationaLLM_AIModel,
                    $"{AIModelResourceTypeNames.AIModels}/{aiModel.Name}",
                    aiModel
                )
                .Returns(Task.FromResult(expectedUpsertResult));

            // Act
            var result = await _aiModelClient.UpsertAIModel(aiModel);

            // Assert
            Assert.Equal(expectedUpsertResult, result);
            await _mockRestClient.Resources.Received(1).UpsertResourceAsync(
                ResourceProviderNames.FoundationaLLM_AIModel,
                $"{AIModelResourceTypeNames.AIModels}/{aiModel.Name}",
                aiModel
            );
        }
    }
}
