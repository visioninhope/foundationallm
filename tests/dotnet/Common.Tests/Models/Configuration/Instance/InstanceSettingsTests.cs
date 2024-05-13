using FoundationaLLM.Common.Models.Configuration.Instance;

namespace FoundationaLLM.Common.Tests.Models.Configuration.Instance
{
    public class InstanceSettingsTests
    {
        [Fact]
        public void InstanceSettings_Id_Property_SetCorrectly()
        {
            // Arrange
            string expectedId = "123e4567-e89b-12d3-a456-426614174000";
            string version = "0.6.0";

            // Act
            var instanceSettings = new InstanceSettings { Id = expectedId, Version =  version};

            // Assert
            Assert.Equal(expectedId, instanceSettings.Id);
        }
    }
}
