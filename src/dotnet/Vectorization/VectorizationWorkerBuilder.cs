using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using FoundationaLLM.Vectorization.Models.Configuration;

namespace FoundationaLLM.Vectorization
{
    public class VectorizationWorkerBuilder
    {
        private VectorizationWorkerSettings _settings;
        private IVectorizationStateService? _stateService;
        private VectorizationQueuing _queuingEngine;

        public VectorizationWorkerBuilder()
        {
            _settings = new VectorizationWorkerSettings
            {
                RequestManagers = new List<RequestManagerServiceSettings>()
            };
        }

        public VectorizationWorker Build()
        {
            if (_stateService == null)
                throw new VectorizationException("Cannot build a vectorization worker without a valid vectorization state service.");

            var vectorizationWorker = new VectorizationWorker(_stateService);

            return vectorizationWorker;
        }

        public VectorizationWorkerBuilder WithSettings(VectorizationWorkerSettings settings)
        {
            _settings = settings;
            return this;
        }

        public VectorizationWorkerBuilder WithStateService(IVectorizationStateService stateService)
        {
            _stateService = stateService;
            return this;
        }

        public VectorizationWorkerBuilder WithQueingEngine(VectorizationQueuing queuingEngine) 
        {
            _queuingEngine = queuingEngine;
            return this;
        }
    }
}
