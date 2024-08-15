﻿using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Core.Examples.Utils;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Core.Examples.LoadTests.ResourceProviders
{
    /// <summary>
    /// Provides the FoundationaLLM resource providers.
    /// </summary>
    public class LoadTestResourceProviders
    {
        private readonly IResourceProviderService _azureOpenAIResourceProvider;
        private readonly IResourceProviderService _attachmentResourceProvider;

        private readonly ITestOutputHelper _output;
        private readonly TimeProfiler _timeProfiler;

        public IResourceProviderService AzureOpenAIResourceProvider => _azureOpenAIResourceProvider;
        public IResourceProviderService AttachmentResourceProvider => _attachmentResourceProvider;

        public LoadTestResourceProviders(
            IServiceProvider serviceProvider,
            ITestOutputHelper output)
        {
            var resourceProviderServices = serviceProvider.GetService<IEnumerable<IResourceProviderService>>();

            _azureOpenAIResourceProvider = resourceProviderServices!
                .Single(rps => rps.Name == ResourceProviderNames.FoundationaLLM_AzureOpenAI);

            _attachmentResourceProvider = resourceProviderServices!
                .Single(rps => rps.Name == ResourceProviderNames.FoundationaLLM_Attachment);

            _output = output;
            _timeProfiler = new TimeProfiler(output);
        }

        public async Task InitializeAll()
        {
            await _timeProfiler.RunAsync(
                async () =>
                {
                    await _azureOpenAIResourceProvider.Initialize();
                    await _attachmentResourceProvider.Initialize();

                    // Add more resource providers here.
                },
                "Initialize FoundationaLLM.AzureOpenAI resource provider.");
        }
    }
}
