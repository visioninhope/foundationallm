using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.ResourceProviders.AIModel
{
    public static class AuthenticationType
    {
        /// <summary>
        /// ApiKey shared secret
        /// </summary>
        public const string ApiKey = "key";
        /// <summary>
        /// Caller has a JWT access token 
        /// </summary>
        public const string Token = "token";
    }
}
