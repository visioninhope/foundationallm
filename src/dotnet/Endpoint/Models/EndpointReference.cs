using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Endpoint;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Endpoint.Models
{
    /// <summary>
    /// Contains a reference to an endpoint.
    /// </summary>
    public class EndpointReference : ResourceReference
    {
        /// <summary>
        /// The object type of the endpoint.
        /// </summary>
        [JsonIgnore]
        public Type EndpointType =>
            Type switch
            {
                EndpointTypes.Basic => typeof(EndpointBase),
                EndpointTypes.AzureAI => typeof(AzureAIEndpoint),
                EndpointTypes.AzureOpenAI => typeof(AzureOpenAIEndpoint),
                EndpointTypes.OpenAI => typeof(OpenAIEndpoint),
                EndpointTypes.External => typeof(ExternalEndpoint),
                _ => throw new ResourceProviderException($"The endpoint type {Type} is not supported.")
            };
    }
}
