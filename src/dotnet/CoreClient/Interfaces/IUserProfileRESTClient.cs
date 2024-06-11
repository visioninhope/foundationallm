using FoundationaLLM.Common.Models.Configuration.Users;

namespace FoundationaLLM.Client.Core.Interfaces
{
    /// <summary>
    /// Provides methods to manage calls to the Core API's user profile endpoints.
    /// </summary>
    public interface IUserProfileRESTClient
    {
        /// <summary>
        /// Retrieves user profiles.
        /// </summary>
        /// <param name="token">The authentication token to send with the request.</param>
        /// <returns></returns>
        Task<IEnumerable<UserProfile>> GetUserProfilesAsync(string token);
    }
}
