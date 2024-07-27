namespace FoundationaLLM.Common.Constants.Configuration
{
    /// <summary>
    /// Defines all keys used for named dependency injection.
    /// </summary>
    public static partial class DependencyInjectionKeys
    {        
        /// <summary>
        /// Dependency injection key used by the FoundationaLLM.AIModel resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_AIModel =
            "FoundationaLLM:ResourceProviders:AIModel";
        
        /// <summary>
        /// Dependency injection key used by the FoundationaLLM.Agent resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Agent =
            "FoundationaLLM:ResourceProviders:Agent";
        
        /// <summary>
        /// Dependency injection key used by the FoundationaLLM.Attachment resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Attachment =
            "FoundationaLLM:ResourceProviders:Attachment";
        
        /// <summary>
        /// Dependency injection key used by the FoundationaLLM.Configuration resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Configuration =
            "FoundationaLLM:ResourceProviders:Configuration";
        
        /// <summary>
        /// Dependency injection key used by the FoundationaLLM.DataSource resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_DataSource =
            "FoundationaLLM:ResourceProviders:DataSource";
        
        /// <summary>
        /// Dependency injection key used by the FoundationaLLM.Prompt resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Prompt =
            "FoundationaLLM:ResourceProviders:Prompt";
        
        /// <summary>
        /// Dependency injection key used by the FoundationaLLM.Vectorization resource provider.
        /// </summary>
        public const string FoundationaLLM_ResourceProviders_Vectorization =
            "FoundationaLLM:ResourceProviders:Vectorization";
    }
}
