using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Configuration.Text;
using Microsoft.Extensions.Options;


/// <summary>
/// Splits text based on number of tokens.
/// </summary>
/// <param name="tokenizerService">The <see cref="ITokenizerService"/> used to tokenize the input text.</param>
/// <param name="options">The <see cref="IOptions{TOptions}"/> providing the settings for the service.</param>
public class TokenTextSplitterService(
    ITokenizerService tokenizerService,
    IOptions<TokenTextSplitterServiceSettings> options) : ITextSplitterService
{
    private readonly ITokenizerService _tokenizerService = tokenizerService;
    private readonly TokenTextSplitterServiceSettings _settings = options.Value;

    /// <inheritdoc/>
    public List<string> SplitPlainText(string text)
    {
        return new List<string> { text };
    }
}
