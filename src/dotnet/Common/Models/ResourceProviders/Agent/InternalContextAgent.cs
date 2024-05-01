namespace FoundationaLLM.Common.Models.ResourceProviders.Agent
{
    /// <summary>
    /// The Internal Context agent metadata model.
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
