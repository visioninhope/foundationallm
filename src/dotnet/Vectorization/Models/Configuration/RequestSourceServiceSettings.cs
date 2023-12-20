namespace FoundationaLLM.Vectorization.Models.Configuration
{
    public class RequestSourceServiceSettings
    {
        /// <summary>
        /// Connection to the storage queue
        /// </summary>
        public required string StorageQueueConnection { get; set; }

        /// <summary>
        /// Specifies the new visibility timeout value, in seconds, relative to server time.
        /// The default value is 30 seconds.
        /// </summary>
        public int VisibilityTimeout { get; set; }
    }
}
