using FoundationaLLM.Common.Models.ResourceProviders.Agent;

namespace FoundationaLLM.Upgrade.Models._040
{
    /// <summary>
    /// The Internal Context agent metadata model.
    /// </summary>
    public class InternalContextAgent050 : Agent040
    {
        /// <summary>
        /// Set default property values.
        /// </summary>
        public InternalContextAgent050() =>
            Type = AgentTypes.InternalContext;
    }
}
