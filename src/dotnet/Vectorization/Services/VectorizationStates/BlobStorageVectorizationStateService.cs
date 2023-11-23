using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using System;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Services.VectorizationStates
{
    public class BlobStorageVectorizationStateService : IVectorizationStateService
    {
        public BlobStorageVectorizationStateService()
        {
        }

        public async Task<VectorizationState> ReadState(string hash)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateState(string hash, VectorizationState state)
        {
            throw new NotImplementedException();
        }
    }
}
