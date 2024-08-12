using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Examples.LoadTests.ResourceProviders
{
    /// <summary>
    /// Provides the FoundationaLLM resource providers.
    /// </summary>
    public class LoadTestResourceProviders
    {
        private readonly IResourceProviderService _azureOpenAIResourceProvider;

        public IResourceProviderService AzureOpenAIResourceProvider => _azureOpenAIResourceProvider;

        public LoadTestResourceProviders(
            IServiceProvider serviceProvider)
        {
            var resourceProviderServices = serviceProvider.GetService<IEnumerable<IResourceProviderService>>();

            _azureOpenAIResourceProvider = resourceProviderServices!
                .Single(rps => rps.Name == ResourceProviderNames.FoundationaLLM_AzureOpenAI);
        }

        public async Task InitializeAll()
        {
            await _azureOpenAIResourceProvider.Initialize();
        }
    }
}
