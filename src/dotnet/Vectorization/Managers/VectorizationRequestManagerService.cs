using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Managers
{
    public class VectorizationRequestManagerService : IRequestManagerService
    {
        private readonly IRequestSourceService _firstRequestSourceService;
        private readonly IVectorizationStateService _vectorizationStateService;

        public VectorizationRequestManagerService(
            IRequestSourceService firstRequestSourceService,
            IVectorizationStateService vectorizationStateService)
        {
            _firstRequestSourceService = firstRequestSourceService;
            _vectorizationStateService = vectorizationStateService;
        }

        public async Task<VectorizationRequest> ReadCurrentRequest()
        {
            return null;
        }

        public async Task CreateNextRequest()
        {
            await _firstRequestSourceService.CreateRequest(new VectorizationRequest());
        }

        public async Task UpdateVectorizationState(string hash, VectorizationState state)
        {
            await _vectorizationStateService.UpdateState(hash, state);
        }
    }
}
