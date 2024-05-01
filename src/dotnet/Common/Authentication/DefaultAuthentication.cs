using Azure.Core;
using Azure.Identity;
using FoundationaLLM.Common.Constants.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;

namespace FoundationaLLM.Common.Authentication
{
    /// <summary>
    /// Provides the default credentials for authentication.
    /// </summary>
    public static class DefaultAuthentication
    {
        /// <summary>
        /// Initializes the default authentication.
        /// </summary>
        /// <param name="production">Indicates whether the environment is production or not.</param>
        public static void Initialize(bool production)
        {
            Production = production;

            var credentials = GetAzureCredential();
            var tokenResult = credentials.GetToken(
                new(["api://FoundationaLLM-Authorization-Auth/.default"]),
                default);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(tokenResult.Token) as JwtSecurityToken;
            //var appId = token!.Claims.First(c => c.Type == "appid").Value;
        }

        /// <summary>
        /// Indicates whether the environment we run in is production or not.
        /// </summary>
        public static bool Production {  get; set; }

        /// <summary>
        /// The default Azure credential to use for authentication.
        /// </summary>
        public static TokenCredential GetAzureCredential() =>
            Production
            ? new ManagedIdentityCredential(Environment.GetEnvironmentVariable(EnvironmentVariables.AzureClientId))
            : new AzureCliCredential();
    }
}
