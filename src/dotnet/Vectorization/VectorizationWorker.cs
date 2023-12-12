using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Services;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization
{
    public class VectorizationWorker
    {
        private readonly IDictionary<string, IRequestSourceService> _requestSourceServices;
        private readonly IVectorizationStateService _vectorizationStateService;
        private readonly IEnumerable<IRequestManagerService> _requestManagerServices;
        private readonly ILogger<VectorizationWorker> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public VectorizationWorker(
            IVectorizationStateService vectorizationStateService,
            IDictionary<string, IRequestSourceService> requestSourceServices,
            IEnumerable<IRequestManagerService> requestManagerServices,
            ILogger<VectorizationWorker> logger,
            CancellationTokenSource cancellationTokenSource)
        {
            _vectorizationStateService = vectorizationStateService;
            _requestSourceServices = requestSourceServices;
            _requestManagerServices = requestManagerServices;
            _logger = logger;
            _cancellationTokenSource = cancellationTokenSource;
        }
    }
}
