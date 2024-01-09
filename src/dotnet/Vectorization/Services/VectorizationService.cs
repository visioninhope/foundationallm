using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;

namespace FoundationaLLM.Vectorization.Services
{
    /// <summary>
    /// Implements the <see cref="IVectorizationService"/> interface.
    /// </summary>
    public class VectorizationService : IVectorizationService
    {
        private readonly Dictionary<string, IRequestSourceService> _requestSources;

        /// <summary>
        /// Creates a new instance of the <see cref="VectorizationService"/> service.
        /// </summary>
        /// <param name="requestSourcesCache">The <see cref="IRequestSourcesCache"/> cache of request sources.</param>
        public VectorizationService(
            IRequestSourcesCache requestSourcesCache) =>
            _requestSources = requestSourcesCache.RequestSources;

        /// <inheritdoc/>
        public async Task ProcessRequest(VectorizationRequest vectorizationRequest)
        {
            // TODO: Perform proper validation of the VectorizationRequest instance

            var firstRequestSource = _requestSources[vectorizationRequest.Steps.First().Id];
            await firstRequestSource.SubmitRequest(vectorizationRequest);
        }
    }
}
