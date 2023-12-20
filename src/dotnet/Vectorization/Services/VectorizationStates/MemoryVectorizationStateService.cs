using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;

namespace FoundationaLLM.Vectorization.Services.VectorizationStates
{
    public class MemoryVectorizationStateService : IVectorizationStateService
    {
        private readonly Dictionary<string, VectorizationState> _vectorizationStateDictionary = new Dictionary<string, VectorizationState>();

        /// <inheritdoc/>
        public async Task<VectorizationState> ReadState(string id)
        {
            await Task.CompletedTask;

            if (!_vectorizationStateDictionary.ContainsKey(id))
                throw new ArgumentException($"Vectorization state with id [{id}] could not be found.");

            return _vectorizationStateDictionary[id];
        }

        /// <inheritdoc/>
        public async Task SaveState(VectorizationState state)
        {
            await Task.CompletedTask;

            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if(!_vectorizationStateDictionary.ContainsKey(state.CurrentRequestId))
                throw new ArgumentException($"Vectorization state with id [{state.CurrentRequestId}] could not be found.");

            _vectorizationStateDictionary[state.CurrentRequestId] = state;
        }
    }
}
