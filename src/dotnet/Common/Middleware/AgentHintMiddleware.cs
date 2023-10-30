using FoundationaLLM.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Middleware
{
    /// <summary>
    /// Middleware that extracts the agent hint value from the request header, if it exists.
    /// This middleware should be registered in the application's Startup.Configure method.
    /// </summary>
    public class AgentHintMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentHintMiddleware"/> class.
        /// </summary>
        /// <param name="next"></param>
        public AgentHintMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Executes the middleware.
        /// </summary>
        /// <param name="context">The current HTTP request context.</param>
        /// <param name="agentHintContext">Stores the resolved agent hint string value populated
        /// by this middleware, if it exists.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, IAgentHintContext agentHintContext)
        {
            var agentHint = context.Request.Headers[Constants.HttpHeaders.AgentHint].FirstOrDefault();
            if (!string.IsNullOrEmpty(agentHint))
            {
                agentHintContext.AgentHint = agentHint;
            }

            // Call the next delegate/middleware in the pipeline:
            await _next(context);
        }
    }

}
