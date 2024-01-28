using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Agent.Models.Resources
{
    /// <summary>
    /// Models the content of the agent reference store managed by the FoundationaLLM.Agent resource provider.
    /// </summary>
    public class AgentReferenceStore
    {
        /// <summary>
        /// The list of all agents registered in the system.
        /// </summary>
        public required List<AgentReference> AgentReferences { get; set; }
    }
}
