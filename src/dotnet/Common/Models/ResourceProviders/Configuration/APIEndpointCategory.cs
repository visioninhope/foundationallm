namespace FoundationaLLM.Common.Models.ResourceProviders.Configuration
{
    /// <summary>
    /// The category for api endpoint class.
    /// </summary>
    public enum APIEndpointCategory
    {
        /// <summary>
        /// Endpoints for internal orchestration services (e.g. LangChain API, SemanticKernel API).
        /// </summary>
        Orchestration,

        /// <summary>
        /// Endpoints for external orchestration services.
        /// </summary>
        ExternalOrchestration,

        /// <summary>
        /// Endpoints for Large Language Models.
        /// </summary>
        LLM,

        /// <summary>
        ///  Endpoints related to Gatekeeper.
        /// </summary>
        Gatekeeper,

        /// <summary>
        /// Endpoints for direct interactions with Microsoft Azure AI services.
        /// </summary>
        AzureAIDirect,

        /// <summary>
        /// Endpoints for direct interactions with Microsoft Azure OpenAI services.
        /// </summary>
        AzureOpenAIDirect,

        /// <summary>
        /// General endpoints (internal APIs other than Orchestration, ExternalOrchestration, LLM, Gatekeeper, AzureAIDirect, or AzureOpenAIDirect.
        /// </summary>
        General
    }
}
