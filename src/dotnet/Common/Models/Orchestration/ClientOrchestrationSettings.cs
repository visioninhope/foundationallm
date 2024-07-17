namespace FoundationaLLM.Common.Models.Orchestration
{
    /// <summary>
    /// Orchestration settings provided in the client request
    /// </summary>
    public class ClientOrchestrationSettings : OrchestrationSettingsBase
    {
        /// <summary>
        /// Key-value pairs of settings passed in
        /// </summary>
        public Dictionary<string, object> ModelParameters { get; set; } = new Dictionary<string, object>();
    }
}
