namespace FoundationaLLM.Common.Constants
{
    /// <summary>
    /// Constants for the LLM Orchestration Service Names.
    /// </summary>
    public static class LLMOrchestrationServiceNames
    {
        /// <summary>
        /// Handles completion requests using real-time scoring endpoint deployed in Azure AI Studio.
        /// </summary>
        public const string AzureAIDirect = "AzureAIDirect";

        /// <summary>
        /// Handles completion requests using Azure OpenAI endpoints.
        /// </summary>
        public const string AzureOpenAIDirect = "AzureOpenAIDirect";

        /// <summary>
        /// Handles completion requests using LangChain.
        /// </summary>
        public const string LangChain = "LangChain";

        /// <summary>
        /// Handles completion requests using Semantic Kernel.
        /// </summary>
        public const string SemanticKernel = "SemanticKernel";
    }
}
