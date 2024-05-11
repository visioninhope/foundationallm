using FoundationaLLM.Common.Models.Chat;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration;

/// <summary>
/// The completion request object.
/// </summary>
public class GatewayCompletionRequest : CompletionRequest
{
    [JsonPropertyName("operation_id")]
    public string? OperationId { get; set; }

    /// <summary>
    /// The token count
    /// </summary>
    [JsonPropertyName("tokens_count")]
    public int TokensCount { get; init; }
}
