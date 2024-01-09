using Azure.Core;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;

namespace FoundationaLLM.Vectorization.Services.VectorizationStates
{
    public class MemoryVectorizationStateService : VectorizationStateServiceBase, IVectorizationStateService
    {
        private readonly Dictionary<string, VectorizationState> _vectorizationStateDictionary = new Dictionary<string, VectorizationState>();

        /// <inheritdoc/>
        public async Task<bool> HasState(VectorizationRequest request)
        {
            await Task.CompletedTask;

            return _vectorizationStateDictionary.ContainsKey(
                GetPersistenceIdentifier(request.ContentIdentifier));
        }

        /// <inheritdoc/>
        public async Task<VectorizationState> ReadState(VectorizationRequest request)
        {
            await Task.CompletedTask;
            var id = GetPersistenceIdentifier(request.ContentIdentifier);

            if (!_vectorizationStateDictionary.ContainsKey(id))
                throw new ArgumentException($"Vectorization state for content id [{id}] could not be found.");

            return _vectorizationStateDictionary[id];
        }

        /// <inheritdoc/>
        public async Task SaveState(VectorizationState state)
        {
            await Task.CompletedTask;
            var id = GetPersistenceIdentifier(state.ContentIdentifier);

            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if(_vectorizationStateDictionary.ContainsKey(id))
                _vectorizationStateDictionary[id] = state;
            else
                _vectorizationStateDictionary.Add(id, state);
        }

        protected override string GetPersistenceIdentifier(VectorizationContentIdentifier contentIdentifier) =>
            $"{contentIdentifier.CanonicalId}_state_{HashContentIdentifier(contentIdentifier)}";
    }
}
