using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization
{
    public class VectorizationWorker
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Dictionary<string, IRequestSourceService> _requestSourceServices;
        private readonly IVectorizationStateService _vectorizationStateService;
        private readonly List<RequestManagerService> _requestManagerServices;

        public VectorizationWorker(
            IVectorizationStateService vectorizationStateService)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _requestSourceServices = new Dictionary<string, IRequestSourceService>();
            _vectorizationStateService = vectorizationStateService;
            _requestManagerServices = new List<RequestManagerService>();
        }
    }
}
