using FoundationaLLM.Client.Management;
using FoundationaLLM.Client.Management.Interfaces;
using NSubstitute;

namespace Management.Client.Tests
{
    public class ManagementClientTests
    {
        private readonly IManagementRESTClient _mockRestClient;
        private readonly ManagementClient _managementClient;

        public ManagementClientTests()
        {
            _mockRestClient = Substitute.For<IManagementRESTClient>();
            _managementClient = new ManagementClient();
            _managementClient.GetType().GetField("_managementRestClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(_managementClient, _mockRestClient);
            _managementClient.GetType().GetMethod("InitializeClients", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(_managementClient, null);
        }

        [Fact]
        public void InitializeClients_ShouldInitializeAllClients()
        {
            // Assert
            Assert.NotNull(_managementClient.Agents);
            Assert.NotNull(_managementClient.Attachments);
            Assert.NotNull(_managementClient.Configuration);
            Assert.NotNull(_managementClient.DataSources);
            Assert.NotNull(_managementClient.Prompts);
            Assert.NotNull(_managementClient.Vectorization);
        }
    }
}