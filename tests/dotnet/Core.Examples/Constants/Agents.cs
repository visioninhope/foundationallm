using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Core.Examples.Constants
{
    /// <summary>
    /// Contains constants for agents and related messages and artifacts.
    /// </summary>
    public static class Agents
    {
        /// <summary>
        /// The name of the FoundationaLLM agent.
        /// </summary>
        public const string FoundationaLLMAgentName = "FoundationaLLM";
        /// <summary>
        /// Standard message returned when a completion request fails.
        /// </summary>
        public const string FailedCompletionResponse = "A problem on my side prevented me from responding.";
    }
}
