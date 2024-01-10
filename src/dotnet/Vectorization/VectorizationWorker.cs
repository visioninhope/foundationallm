using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Services;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization
{
    public class VectorizationWorker(
        IEnumerable<IRequestManagerService> requestManagerServices)
    {
        private readonly IEnumerable<IRequestManagerService> _requestManagerServices = requestManagerServices;

        public async Task Run()
        {
            var requestManagerTasks = _requestManagerServices
                .Select(async rms => await rms.Run())
                .ToArray();

            await Task.WhenAll(requestManagerTasks);
        }
    }
}
