using FoundationaLLM.Common.Models.Orchestration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Tests.Models.Orchestration
{
    public class OrchestrationRequestTests
    {
        [Fact]
        public void OrchestrationRequest_UserPrompt_SetCorrectly()
        {
            // Arrange
            var testUserPrompt = "User_Prompt";
            var orchestrationRequest = new OrchestrationRequest
            { UserPrompt = testUserPrompt };

            // Assert
            Assert.Equal(testUserPrompt, orchestrationRequest.UserPrompt);
        }
    }
}
