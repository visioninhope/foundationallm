using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.Common.Constants.Agents
{
    /// <summary>
    /// Contains constants for the keys that can be added to the <see cref="LLMCompletionRequest.Objects"/> dictionary.
    /// </summary>
    public static class CompletionRequestObjectsKeys
    {
        /// <summary>
        /// The key name for the dictionary containing names and descriptions of agents other than the completion request's agent.
        /// This value should be a dictionary where keys are agent names and values are agent descriptions.
        /// </summary>
        public const string AllAgents = "AllAgents";

        /// <summary>
        /// All completion request objects dictionary keys.
        /// </summary>
        public readonly static string[] All = [
            AllAgents
        ];
    }
}
