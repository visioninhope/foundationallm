using System.Text.Json.Serialization;

namespace FoundationaLLM.Agent.Models.Resources
{
    /// <summary>
    /// Provides details about an agent.
    /// </summary>
    public class AgentReference
    {
        /// <summary>
        /// The name of the agent.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// The filename of the agent.
        /// </summary>
        public required string Filename { get; set; }
        /// <summary>
        /// The type of the agent.
        /// </summary>
        public required string Type { get; set; }
    }
}
