namespace FoundationaLLM.Client.Core.Interfaces
{
    /// <summary>
    /// Provides methods to manage calls to the Core API's status endpoints.
    /// </summary>
    public interface IStatusRESTClient
    {
        /// <summary>
        /// Returns the status of the Core API service.
        /// </summary>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task<string> GetServiceStatusAsync(string token);

        /// <summary>
        /// Returns OK if the requester is authenticated and allowed to execute
        /// requests against the Core API service.
        /// </summary>
        /// <param name="token">The authentication token to send with the request.</param>
        Task<string> GetAuthStatusAsync(string token);
    }
}
