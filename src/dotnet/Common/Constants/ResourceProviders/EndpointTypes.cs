namespace FoundationaLLM.Common.Constants.ResourceProviders
{
    /// <summary>
    /// String constants for endpoint types.
    /// </summary>
    public static class EndpointTypes
    {
        /// <summary>
        /// Basic endpoint with practical functionality. Used as base for all other endpoints.
        /// </summary>
        public const string Basic = "basic";

        /// <summary>
        /// Azure AI endpoint.
        /// </summary>
        public const string AzureAI = "azure-ai";

        /// <summary>
        /// Azure OpenAI endpoint.
        /// </summary>
        public const string AzureOpenAI = "azure-openai";

        /// <summary>
        /// OpenAI endpoint.
        /// </summary>
        public const string OpenAI = "openai";

        /// <summary>
        /// External endpoint.
        /// </summary>
        public const string External = "external";
    }
}
