using FoundationaLLM.Common.Services.Events;

namespace FoundationaLLM.Common.Models.Configuration.Events
{
    /// <summary>
    /// Provides configuration settings to initialize the <see cref="LocalEventService"/> service.
    /// </summary>
    public class LocalEventServiceSettings
    {
        /// <summary>
        /// The time interval in seconds between successive event processing cycles.
        /// </summary>
        public int EventProcessingCycleSeconds { get; set; } = 10;
    }
}
