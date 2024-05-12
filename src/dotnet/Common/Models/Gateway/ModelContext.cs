using FoundationaLLM.Common.Instrumentation;

namespace FoundationaLLM.Common.Models.Gateway
{
    public class ModelContext(
        )
    {
        protected readonly object _syncRoot = new();

        public string ModelName { get; set; }

        public SlidingWindowRateLimiter RequestCount { get; set; }

        public SlidingWindowRateLimiter TokenCount { get; set; }
    }
}
