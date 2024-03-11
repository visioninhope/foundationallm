using FoundationaLLM.Authorization.Models;

namespace FoundationaLLM.Authorization.Interfaces
{
    /// <summary>
    /// Defines methods exposed by the Authorization service.
    /// </summary>
    public interface IAuthorizationAPIService
    {
        /// <summary>
        /// Processes an action authorization request.
        /// </summary>
        /// <param name="authorizationRequest">The <see cref="ActionAuthorizationRequest"/> to process.</param>
        /// <returns>An <see cref="ActionAuthorizationResult"/> containing the result of the processing.</returns>
        Task<ActionAuthorizationResult> ProcessAuthorizationRequest(ActionAuthorizationRequest authorizationRequest);
    }
}
