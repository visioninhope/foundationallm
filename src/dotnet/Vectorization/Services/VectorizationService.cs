using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;

namespace FoundationaLLM.Vectorization.Services
{
    /// <summary>
    /// Implements the <see cref="IVectorizationService"/> interface.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of the <see cref="VectorizationService"/> service.
    /// </remarks>
    /// <param name="requestSourcesCache">The <see cref="IRequestSourcesCache"/> cache of request sources.</param>
    public class VectorizationService(
        IRequestSourcesCache requestSourcesCache) : IVectorizationService
    {
        private readonly Dictionary<string, IRequestSourceService> _requestSources = requestSourcesCache.RequestSources;

        /// <inheritdoc/>
        public async Task ProcessRequest(VectorizationRequest vectorizationRequest)
        {
            ValidateRequest(vectorizationRequest);

            var firstRequestSource = _requestSources[vectorizationRequest.Steps.First().Id];
            await firstRequestSource.SubmitRequest(vectorizationRequest);
        }

        private static void ValidateRequest(VectorizationRequest vectorizationRequest)
        {
            if (vectorizationRequest == null)
                throw new VectorizationException("The vectorization request should not be null.");

            if (String.IsNullOrWhiteSpace(vectorizationRequest.Id))
                throw new VectorizationException("The vectorization request id should not be null.");

            if (vectorizationRequest.ContentIdentifier == null
                || String.IsNullOrWhiteSpace(vectorizationRequest.ContentIdentifier.UniqueId)
                || String.IsNullOrWhiteSpace(vectorizationRequest.ContentIdentifier.CanonicalId))
                throw new VectorizationException("The vectorization request content identifier is invalid.");

            if (vectorizationRequest.Steps == null || vectorizationRequest.Steps.Count == 0)
                throw new VectorizationException("The list of the vectorization steps should not be empty.");

            if (vectorizationRequest.Steps.Select(x=>x.Id).Distinct().Count() != vectorizationRequest.Steps.Count)
                throw new VectorizationException("The list of vectorization steps must contain unique names.");

            if (vectorizationRequest.CompletedSteps != null && vectorizationRequest.CompletedSteps!.Count > 0)
                throw new VectorizationException("The completed steps of the vectorization request must be empty.");

            if (vectorizationRequest.RemainingSteps == null || vectorizationRequest.RemainingSteps.Count == 0)
                throw new VectorizationException("The list of the remaining steps of the vectorization request should not be empty.");
        }
    }
}
