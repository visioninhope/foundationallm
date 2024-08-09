using FoundationaLLM.Common.Models.ResourceProviders.Agent;

namespace FoundationaLLM.Utility.Upgrade.Models._050
{
    /// <summary>
    /// The Internal Context agent metadata model.
    /// </summary>
    public class InternalContextAgent050 : Agent050
    {
        /// <summary>
        /// Set default property values.
        /// </summary>
        public InternalContextAgent050() =>
            Type = AgentTypes050.InternalContext;
    }
}
