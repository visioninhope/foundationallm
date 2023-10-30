using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Middleware;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Tests.Middleware
{
    public class AgentHintMiddlewareTests
    {
        [Fact]
        public async void InvokeAsync_ShouldSetAgentHint_WhenHeaderExists()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var agentHintContext = Substitute.For<IAgentHintContext>();
            var middleware = new AgentHintMiddleware(next: (innerHttpContext) => Task.FromResult(0));
            var expectedAgentHint = "test-agent-hint";
            context.Request.Headers[Constants.HttpHeaders.AgentHint] = expectedAgentHint;

            // Act
            await middleware.InvokeAsync(context, agentHintContext);

            // Assert
            Assert.Equal(expectedAgentHint, agentHintContext.AgentHint);
        }

        [Fact]
        public async void InvokeAsync_ShouldNotSetAgentHint_WhenHeaderDoesNotExist()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var agentHintContext = Substitute.For<IAgentHintContext>();
            var middleware = new AgentHintMiddleware(next: (innerHttpContext) => Task.FromResult(0));

            // Act
            await middleware.InvokeAsync(context, agentHintContext);

            // Assert
            agentHintContext.DidNotReceive().AgentHint = Arg.Any<string>();
        }
    }
}
