namespace FoundationaLLM.Common.Constants.ResourceProviders
{
    /// <summary>
    /// String constants for model types.
    /// </summary>
    public static class ModelTypes
    {
        /// <summary>
        /// Basic model with practical functionality. Used as base for all other models.
        /// </summary>
        public const string Basic = "basic";

        /// <summary>
        /// Azure OpenAI models (gpt-3-turbo, gpt-4, etc.).
        /// </summary>
        public const string AzureOpenAI = "azure-openai";

        /// <summary>
        /// OpenAI models (gpt-3.5-turbo, gpt-4, etc.).
        /// </summary>
        public const string OpenAI = "openai";

        /// <summary>
        /// MistralAI model.
        /// </summary>
        public const string MistralAI = "mistral-ai";

        /// <summary>
        /// Llama-2 model.
        /// </summary>
        public const string Llama2 = "llama-2";

        /// <summary>
        /// Llama-3 model.
        /// </summary>
        public const string Llama3 = "llama-3";
    }
}
