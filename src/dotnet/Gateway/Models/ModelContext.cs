using FoundationaLLM.Common.Instrumentation;

namespace FoundationaLLM.Gateway.Models
{
    public class ModelContext(
        )
    {
        protected readonly object _syncRoot = new();

        public required string ModelName { get; set; }

        public SlidingWindowRateLimiter RequestCount { get; set; }

        public SlidingWindowRateLimiter TokenCount { get; set; }
    }
}
