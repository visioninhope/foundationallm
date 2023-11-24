using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Services
{
    public class RequestManagerService : IRequestManagerService
    {
        private readonly RequestManagerServiceSettings _settings;
        private readonly Dictionary<string, IRequestSourceService> _requestSourceServices;
        private readonly IRequestSourceService _incomingRequestSourceService;
        private readonly IVectorizationStateService _vectorizationStateService;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;

        public RequestManagerService(
            RequestManagerServiceSettings settings,
            Dictionary<string, IRequestSourceService> requestSourceServices,
            IVectorizationStateService vectorizationStateService,
            ILogger<RequestManagerService> logger,
            CancellationToken cancellationToken)
        {
            _settings = settings;
            _requestSourceServices = requestSourceServices;
            _vectorizationStateService = vectorizationStateService;

            _logger = logger;
            _cancellationToken = cancellationToken;

            if (!_requestSourceServices.ContainsKey(_settings.RequestSourceName)
                || _requestSourceServices[_settings.RequestSourceName] == null)
                throw new ArgumentException($"Could not find a request source service for [{_settings.RequestSourceName}].");

            _incomingRequestSourceService = _requestSourceServices[_settings.RequestSourceName];
        }

        public Task Start()
        {
            return Task.Factory.StartNew(() => Run());
        }

        private async void Run()
        {
            while (true)
            {
                if (_cancellationToken.IsCancellationRequested)
                    return;

                var request = await _incomingRequestSourceService.ReadRequest();


            }
        }
    }
}
