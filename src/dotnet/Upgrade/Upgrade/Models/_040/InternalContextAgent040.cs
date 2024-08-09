using FoundationaLLM.Common.Models.ResourceProviders.Agent;

namespace FoundationaLLM.Utility.Upgrade.Models._040
{
    /// <summary>
    /// The Internal Context agent metadata model.
    /// </summary>
    public class InternalContextAgent040 : Agent040
    {
        /// <summary>
        /// Set default property values.
        /// </summary>
        public InternalContextAgent040() =>
            Type = AgentTypes040.InternalContext;
    }
}
