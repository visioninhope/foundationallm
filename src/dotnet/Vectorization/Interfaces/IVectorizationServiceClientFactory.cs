namespace FoundationaLLM.Vectorization.Interfaces
{
    /// <summary>
    /// Provides a factory for creating instances of <see cref="IVectorizationServiceClient"/>.
    /// </summary>
    public interface IVectorizationServiceClientFactory
    {
        /// <summary>
        /// Creates a new instance of <see cref="IVectorizationServiceClient"/>.
        /// </summary>
        /// <returns>The IVectorizationServcieClient</returns>
        IVectorizationServiceClient CreateClient();
    }
}
