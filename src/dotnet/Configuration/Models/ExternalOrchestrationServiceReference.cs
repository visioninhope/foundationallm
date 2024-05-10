using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Configuration;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Configuration.Models
{
    /// <summary>
    /// Holds a reference to an external orchestration service.
    /// </summary>
    public class ExternalOrchestrationServiceReference : ResourceReference
    {
        /// <summary>
        /// The object type of the agent.
        /// </summary>
        [JsonIgnore]
        public Type ResourceType =>
            Type switch
            {
                ConfigurationTypes.ExternalOrchestrationService => typeof(ExternalOrchestrationService),
                _ => throw new ResourceProviderException($"The resource type {Type} is not supported.")
            };
    }
}
