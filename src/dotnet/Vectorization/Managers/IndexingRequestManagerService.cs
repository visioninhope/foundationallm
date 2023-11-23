using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Managers
{
    public class IndexingRequestManagerService : IRequestManagerService
    {
        private readonly IRequestSourceService _currentRequestSourceService;
        private readonly IRequestSourceService _nextRequestSourceService;
        private readonly IVectorizationStateService _vectorizationStateService;

        public IndexingRequestManagerService(
            IRequestSourceService currentRequestSourceService,
            IRequestSourceService nextRequestSourceService,
            IVectorizationStateService vectorizationStateService)
        {
            _currentRequestSourceService = currentRequestSourceService;
            _nextRequestSourceService = nextRequestSourceService;
            _vectorizationStateService = vectorizationStateService;
        }

        public async Task<VectorizationRequest> ReadCurrentRequest()
        {
            return await _currentRequestSourceService.ReadRequest();
        }

        public async Task CreateNextRequest()
        {
            await _nextRequestSourceService.CreateRequest(new VectorizationRequest());
        }

        public async Task UpdateVectorizationState(string hash, VectorizationState state)
        {
            await _vectorizationStateService.UpdateState(hash, state);
        }
    }
}
