using FoundationaLLM.Common.Constants.ResourceProviders;

namespace FoundationaLLM.Common.Models.ResourceProviders.Endpoint
{
    /// <summary>
    /// External endpoint.
    /// </summary>
    public class ExternalEndpoint : EndpointBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ExternalEndpoint"/> endpoint.
        /// </summary>
        public ExternalEndpoint() =>
            Type = EndpointTypes.External;
    }
}
