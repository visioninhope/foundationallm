using Azure.Core;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


/// <summary>
/// Splits text based on number of tokens.
/// </summary>
/// <param name="tokenizerService">The <see cref="ITokenizerService"/> used to tokenize the input text.</param>
/// <param name="options">The <see cref="IOptions{TOptions}"/> providing the settings for the service.</param>
public class TokenTextSplitterService(
    ITokenizerService tokenizerService,
    IOptions<TokenTextSplitterServiceSettings> options,
    ILogger<TokenTextSplitterService> logger) : ITextSplitterService
{
    private readonly ITokenizerService _tokenizerService = tokenizerService;
    private readonly TokenTextSplitterServiceSettings _settings = options.Value;
    private readonly ILogger<TokenTextSplitterService> _logger = logger;

    /// <inheritdoc/>
    public List<string> SplitPlainText(string text)
    {
        var tokens = tokenizerService.Encode(text, _settings.TokenizerEncoder);

        if (tokens != null)
        {
            _logger.LogInformation("The tokenizer identified {TokensCount} tokens.", tokens.Count);

            var chunksCount = (tokens!.Count - _settings.OverlapSizeTokens) / (_settings.ChunkSizeTokens - _settings.OverlapSizeTokens);

            return new List<string> { text };
        }
        else
            throw new TextProcessingException("The tokenizer service failed to split the text into tokens.");
    }
}
