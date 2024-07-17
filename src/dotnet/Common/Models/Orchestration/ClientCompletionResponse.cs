using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Orchestration;

/// <summary>
/// Response from a language model.
/// </summary>
public class ClientCompletionResponse : CompletionResponseBase
{

    /// <summary>
    /// User prompt embedding.
    /// </summary>
    [JsonPropertyName("user_prompt_embedding")]
    public float[]? UserPromptEmbedding { get; set; }

    /// <summary>
    /// Initialize a completion response
    /// </summary>
    /// <param name="completion">The completion response from the language model.</param>
    /// <param name="userPrompt">The user prompt the language model responded to.</param>
    /// <param name="userPromptTokens">The number of tokens in the prompt.</param>
    /// <param name="responseTokens">The number of tokens in the completion.</param>
    /// <param name="userPromptEmbedding">User prompt embedding.</param>
    public ClientCompletionResponse(string completion, string userPrompt, int userPromptTokens, int responseTokens,
        float[]? userPromptEmbedding)
    {
        Completion = completion;
        UserPrompt = userPrompt;
        PromptTokens = userPromptTokens;
        CompletionTokens = responseTokens;
        UserPromptEmbedding = userPromptEmbedding;
    }

    /// <summary>
    /// Initialize a completion response.
    /// </summary>
    public ClientCompletionResponse()
    {
        Completion = string.Empty;
        UserPrompt = string.Empty;
    }
}
