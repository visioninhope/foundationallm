using FoundationaLLM.Agent.Constants;
using FoundationaLLM.Agent.Models.Metadata;
using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Models.ResourceProvider;
using Newtonsoft.Json;

namespace FoundationaLLM.Agent.Models.Resources
{
    /// <summary>
    /// Provides details about an agent.
    /// </summary>
    public class AgentReference : ResourceReference
    {
        /// <summary>
        /// The object type of the agent.
        /// </summary>
        [JsonIgnore]
        public Type AgentType =>
            Type switch
            {
                AgentTypes.Basic => typeof(AgentBase),
                AgentTypes.KnowledgeManagement => typeof(KnowledgeManagementAgent),
                _ => throw new ResourceProviderException($"The agent type {Type} is not supported.")
            };
    }
}
