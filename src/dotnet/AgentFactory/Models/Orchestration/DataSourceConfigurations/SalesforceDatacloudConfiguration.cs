using FoundationaLLM.AgentFactory.Core.Models.Orchestration.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.AgentFactory.Core.Models.Orchestration.DataSourceConfigurations
{
    /// <summary>
    /// Blob storage configuration settings.
    /// </summary>
    public class SalesforceDatacloudConfiguration
    {
        /// <summary>
        /// The connection string key vault secret name that is retrieved from key vault.
        /// </summary>
        [JsonProperty("client_id")]
        public string? ClientId { get; set; }

        /// <summary>
        /// The name of the container
        /// </summary>
        [JsonProperty("client_secret")]
        public string? ClientSecret { get; set; }

        /// <summary>
        /// The list of files to get
        /// </summary>
        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        /// <summary>
        /// The list of files to get
        /// </summary>
        [JsonProperty("instance_url")]
        public string? InstanceUrl { get; set; }
    }
}