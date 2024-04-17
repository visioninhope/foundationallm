using FoundationaLLM.Common.Models.Authorization;

namespace FoundationaLLM.Authorization.Interfaces
{
    /// <summary>
    /// Defines the methods for authorization core.
    /// </summary>
    public interface IAuthorizationCore
    {
        /// <summary>
        /// Processes an authorization request.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="authorizationRequest">The <see cref="ActionAuthorizationRequest"/> containing the details of the authorization request.</param>
        /// <returns>An <see cref="ActionAuthorizationResult"/> indicating whether the requested authorization was successfull or not.</returns>
        ActionAuthorizationResult ProcessAuthorizationRequest(string instanceId, ActionAuthorizationRequest authorizationRequest);

        /// <summary>
        /// Processes a batch of authorization requests.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="authorizationRequests">The <see cref="List<ActionAuthorizationRequest>"/> containing the list of details for the authorization request.</param>
        /// <returns>List of <see cref="ActionAuthorizationResult"/> indicating whether the requested authorizations was successfull or not.</returns>
        List<ActionAuthorizationResult> ProcessAuthorizationRequests(string instanceId, List<ActionAuthorizationRequest> authorizationRequests);

        /// <summary>
        /// Checks if a specified security principal is allowed to process authorization requests. 
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance id.</param>
        /// <param name="securityPrincipalId">The id of the security principal whose authorization is checked.</param>
        /// <returns>True if the security principal is allowed to process authorization requests.</returns>
        bool AllowAuthorizationRequestsProcessing(string instanceId, string securityPrincipalId);
    }
}
