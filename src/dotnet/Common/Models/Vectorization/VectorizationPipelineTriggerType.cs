namespace FoundationaLLM.Common.Models.Vectorization
{
    /// <summary>
    /// Types of vectorization pipeline triggers.
    /// </summary>
    public enum VectorizationPipelineTriggerType
    {
        /// <summary>
        /// The vectorization pipeline is triggered manually.
        /// </summary>
        Manual,

        /// <summary>
        /// The vectorization pipeline is triggered based on a regular schedule.
        /// </summary>
        Schedule,

        /// <summary>
        /// The vectorization pipeline is triggered based on content change events (e.g., an existing file is updated or a new file is added).
        /// </summary>
        Event
    }
}
