using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Stores the agent hint value extracted from the request header, if any.
    /// This is used in scenarios where the client wants to hint the API about
    /// a specific agent to resolve, and is passed through the API layers in
    /// the form of a header.
    /// </summary>
    public interface IAgentHintContext
    {
        /// <summary>
        /// The current agent hint. If empty, there is no associated header value.
        /// </summary>
        string? AgentHint { get; set; }
    }
}
