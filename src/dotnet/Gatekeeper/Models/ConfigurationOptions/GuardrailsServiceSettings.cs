namespace FoundationaLLM.Gatekeeper.Core.Models.ConfigurationOptions
{
    /// <summary>
    /// Provides configuration options for the Encrypt AI Guardrails service.
    /// </summary>
    public record GuardrailsServiceSettings
    {
        /// <summary>
        /// The Encrypt AI Guardrails service endpoint.
        /// </summary>
        public required string APIUrl { get; init; }

        /// <summary>
        /// The Encrypt AI Guardrails service key.
        /// </summary>
        public required string APIKey { get; init; }
    }
}
