using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;

namespace FoundationaLLM.Vectorization.Services.VectorizationStates
{
    public class MemoryVectorizationStateService : IVectorizationStateService
    {
        private readonly Dictionary<string, VectorizationState> _vectorizationStateDictionary = new Dictionary<string, VectorizationState>();

        /// <inheritdoc/>
        public async Task<bool> HasState(VectorizationRequest request)
        {
            await Task.CompletedTask;

            return _vectorizationStateDictionary.ContainsKey(request.Content.UniqueId);
        }

        /// <inheritdoc/>
        public async Task<VectorizationState> ReadState(VectorizationRequest request)
        {
            await Task.CompletedTask;

            if (!_vectorizationStateDictionary.ContainsKey(request.Content.UniqueId))
                throw new ArgumentException($"Vectorization state for content id [{request.Content.UniqueId}] could not be found.");

            return _vectorizationStateDictionary[request.Content.UniqueId];
        }

        /// <inheritdoc/>
        public async Task SaveState(VectorizationState state)
        {
            await Task.CompletedTask;

            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if(_vectorizationStateDictionary.ContainsKey(state.ContentId))
                _vectorizationStateDictionary[state.ContentId] = state;
            else
                _vectorizationStateDictionary.Add(state.ContentId, state);
        }
    }
}
