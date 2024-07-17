using FoundationaLLM.Common.Models.ResourceProviders.AIModel;

namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// Completion request for invoking the LLM
    /// </summary>
    public class LLMOrchestrationSettings : OrchestrationSettingsBase
    {
        /// <summary>
        /// AIModel resource based on the agent definition
        /// </summary>
        public AIModelBase? AIModel { get; set; }
        /// <summary>
        /// Key-value pairs for client request to override configured model parameters
        /// </summary>
        public Dictionary<string,object>? ModelParameters { get; set; }
    }
}
