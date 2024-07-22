using FoundationaLLM.State.Interfaces;
using FoundationaLLM.State.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.State.Services
{
    /// <summary>
    /// Implements the FoundationaLLM state service.
    /// </summary>
    /// <param name="options">Provides the options with the <see cref="StateServiceSettings"/> settings for configuration.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public class StateService(
        IOptions<StateServiceSettings> options,
        ILogger<StateService> logger) : IStateService
    {
        private readonly StateServiceSettings _settings = options.Value;
        private readonly ILogger<StateService> _logger = logger;
    }
}
