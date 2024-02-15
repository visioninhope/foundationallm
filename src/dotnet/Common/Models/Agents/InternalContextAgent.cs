namespace FoundationaLLM.Common.Models.Agents
{
    /// <summary>
    /// The Knowledge Management agent metadata model.
    /// </summary>
    public class InternalContextAgent : AgentBase
    {
        /// <summary>
        /// Set default property values.
        /// </summary>
        public InternalContextAgent() =>
            Type = AgentTypes.InternalContext;
    }
}
