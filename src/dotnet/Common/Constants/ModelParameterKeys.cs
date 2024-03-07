namespace FoundationaLLM.Common.Constants
{
    /// <summary>
    /// Contains constants of the keys for all overridable model settings.
    /// </summary>
    public static class ModelParameterKeys
    {
        /// <summary>
        /// The key name for the deployment_name model parameter.
        /// This value should be a string representing the name of the model deployment in Azure OpenAI.
        /// </summary>
        public const string Deployment_Name = "deployment_name";
        /// <summary>
        /// The key name for the temperature parameter.
        /// This value should be a float between 0.0 and 1.0.
        /// </summary>
        public const string Temperature = "temperature";

        public const string TopK = "top_k";
        public const string TopP = "top_p";
        public const string DoSample = "do_sample";
        public const string MaxNewTokens = "max_new_tokens";
        public const string ReturnFullText = "return_full_text";
        public const string IgnoreEOS = "ignore_eos";
    }
}
