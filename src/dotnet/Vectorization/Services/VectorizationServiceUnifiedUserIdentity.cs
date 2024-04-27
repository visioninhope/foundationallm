using FoundationaLLM.Common.Models.Authentication;

namespace FoundationaLLM.Vectorization.Services
{
    /// <summary>
    /// Represents the unified user identity for the vectorization services.
    /// </summary>
    public class VectorizationServiceUnifiedUserIdentity : UnifiedUserIdentity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VectorizationServiceUnifiedUserIdentity"/> class that identifies the vectorization services.
        /// </summary>
        public VectorizationServiceUnifiedUserIdentity()
        {
            Name = "VectorizationAPI";
            UserId = "VectorizationAPI";
            Username = "VectorizationAPI";
        }
    }
}
