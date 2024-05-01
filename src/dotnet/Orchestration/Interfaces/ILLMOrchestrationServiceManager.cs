namespace FoundationaLLM.Orchestration.Core.Interfaces
{
    /// <summary>
    /// Defines the interface for the LLM Orchestration Service Manager.
    /// </summary>
    public interface ILLMOrchestrationServiceManager
    {
        /// <summary>
        /// Gets an aggredate initialization status based on the initialization status of each subordinate orchestration service.
        /// </summary>
        string Status { get; }

        /// <summary>
        /// Gets an <see cref="ILLMOrchestrationService"/> instance based on the service name.
        /// </summary>
        /// <param name="serviceName">The name of the <see cref="ILLMOrchestrationService"/> to be retrieved.</param>
        /// <returns></returns>
        ILLMOrchestrationService GetService(string serviceName);
    }
}
