using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Middleware { 

    /// <summary>
    /// Middleware that ensures the application is served only from a specific base path, not the root.
    /// This middleware should be registered in the application's Startup.Configure method.
    /// </summary>
    public class EnforcePathBaseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _basePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnforcePathBaseMiddleware"/> class.
        /// </summary>
        /// <param name="next">next pipeline request delegate.</param>
        /// <param name="basePath">base path from where the site is served.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public EnforcePathBaseMiddleware(RequestDelegate next, string basePath)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        }

        /// <summary>
        /// Executes the middleware.
        /// </summary>
        /// <param name="context">The current HTTP request context.</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // If the request path does not start with the base path, return 404 Not Found.  
            if (context.Request.PathBase != _basePath)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }
            // Otherwise, continue processing the request.  
            await _next(context);
        }
    }
}
