using FoundationaLLM.Client.Core.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoundationaLLM.Client.Core.Clients.Rest
{
    /// <summary>
    /// Provides methods to manage calls to the Core API's user profile endpoints.
    /// </summary>
    public class UserProfileRESTClient(IHttpClientFactory httpClientFactory) : CoreRESTClientBase(httpClientFactory), IUserProfileRESTClient
    {
        /// <inheritdoc/>
        public async Task<IEnumerable<UserProfile>> GetUserProfilesAsync(string token)
        {
            var coreClient = GetCoreClient(token);
            var responseMessage = await coreClient.GetAsync("userprofiles");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var userProfiles = JsonSerializer.Deserialize<IEnumerable<UserProfile>>(responseContent, SerializerOptions);
                return userProfiles ?? throw new InvalidOperationException("The returned user profiles are invalid.");
            }

            throw new Exception($"Failed to retrieve user profiles. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
        }
    }
}
