using FoundationaLLM.Common.Models.ResourceProvider;
using FoundationaLLM.Configuration.Constants;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Configuration.Models
{
    /// <summary>
    /// Azure App Configuration key model.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(AppConfigurationKeyValue), ConfigurationTypes.AppConfigurationKeyValue)]
    [JsonDerivedType(typeof(AppConfigurationKeyVaultReference), ConfigurationTypes.AppConfigurationKeyVaultReference)]
    public class AppConfigurationKeyBase : ResourceBase
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override string? Type { get; set; }

        /// <summary>
        /// The name of the App Configuration key.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }
    }
}
