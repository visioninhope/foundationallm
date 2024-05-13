using FoundationaLLM.Common.Models.Orchestration.DataSourceConfigurations;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Upgrade.Models._040
{
    public class CXOConfiguration040 : SearchServiceConfiguration040
    {
        /// <summary>
        /// The type of configuration. This value should not be changed.
        /// </summary>
        [JsonPropertyName("configuration_type")]
        public new string ConfigurationType => "cxo";
        /// <summary>
        /// Search filter elements.
        /// </summary>
        [JsonPropertyName("sources")]
        public string[]? Sources { get; set; }

        /// <summary>
        /// The vector database.
        /// </summary>
        [JsonPropertyName("retriever_mode")]
        public required string RetrieverMode { get; set; }

        /// <summary>
        /// The name of the CXO's company.
        /// </summary>
        [JsonPropertyName("company")]
        public required string Company { get; set; }
    }
}
