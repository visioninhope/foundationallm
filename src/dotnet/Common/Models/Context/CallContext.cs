using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;

namespace FoundationaLLM.Common.Models.Context
{
    /// <inheritdoc/>
    public class CallContext : ICallContext
    {
        /// <inheritdoc/>
        public UnifiedUserIdentity? CurrentUserIdentity { get; set; }
        /// <inheritdoc/>
        public string? InstanceId { get; set; }
    }
}
