using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.State.Exceptions
{
    /// <summary>
    /// Implements a global exception handler for the State API.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public class StateExceptionHandler(
        ILogger<StateExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<StateExceptionHandler> _logger = logger;

        /// <inheritdoc/>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "ERROR: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Title = "FoundationaLLM State error"
            };

            if (exception is StateException stateException)
            {
                problemDetails.Status = stateException.StatusCode;
                problemDetails.Detail = stateException.Message;
            }
            else
            {
                problemDetails.Status = StatusCodes.Status500InternalServerError;
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
