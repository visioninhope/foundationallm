using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Services
{
    public class RequestManagerService : IRequestManagerService
    {
        private readonly RequestManagerServiceSettings _settings;
        private readonly IRequestSourceService _currentRequestSourceService;
        private readonly IRequestSourceService _nextRequestSourceService;
        private readonly IVectorizationStateService _vectorizationStateService;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;

        public RequestManagerService(
            RequestManagerServiceSettings settings,
            IRequestSourceService currentRequestSourceService,
            IRequestSourceService nextRequestSourceService,
            IVectorizationStateService vectorizationStateService,
            ILogger<RequestManagerService> logger,
            CancellationToken cancellationToken)
        {
            _settings = settings;
            _currentRequestSourceService = currentRequestSourceService;
            _nextRequestSourceService = nextRequestSourceService;
            _vectorizationStateService = vectorizationStateService;

            _logger = logger;
            _cancellationToken = cancellationToken;
        }

        public Task Start()
        {
            return Task.Factory.StartNew(() => Run());
        }

        private void Run()
        {
            while (true)
            {
                if (_cancellationToken.IsCancellationRequested)
                    return;

            }
        }
    }
}
