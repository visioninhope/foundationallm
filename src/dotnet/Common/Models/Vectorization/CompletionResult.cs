using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Gateway.Models;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Vectorization
{
    /// <summary>
    /// The result of a completion request.
    /// </summary>
    public class CompletionResult : OperationResult
    {
        /// <summary>
        /// The operation result
        /// </summary>
        [JsonPropertyName("result")]
        public CompletionResponse Result { get; set; }
    }
}
