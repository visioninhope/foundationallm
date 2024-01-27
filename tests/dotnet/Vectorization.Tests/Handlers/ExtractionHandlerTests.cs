using FakeItEasy;
using FoundationaLLM.Vectorization.Handlers;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Vectorization.Tests
{
    public class ExtractionHandlerTests
    {
        [Fact]
        public void TestProcessRequest()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string> { { "content_source_name", "BlobStorage" } }).Build();
            IConfigurationSection stepsConfiguration = configurationRoot.GetSection();
            IVectorizationStateService stateService = A.Fake<IVectorizationStateService>();
            IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();
            ILoggerFactory loggerFactory = new LoggerFactory();
            var handler = new ExtractionHandler(
                "",
                new Dictionary<string, string> { },
                stepsConfiguration,
                stateService,
                serviceProvider,
                loggerFactory
            );

            var contentIdentifier = new VectorizationContentIdentifier
            {
                MultipartId = new List<string> { },
                CanonicalId = ""
            };
            VectorizationRequest request = new VectorizationRequest {
                Id = "",
                ContentIdentifier = contentIdentifier,
                Steps = new List<VectorizationStep>
                {
                    new VectorizationStep { Id = "1", Parameters = new Dictionary<string, string> { { } } }
                },
                CompletedSteps = new List<string> {},
                RemainingSteps = new List<string> {}
            };
            VectorizationState state = new VectorizationState
            {
                CurrentRequestId = "1",
                ContentIdentifier = contentIdentifier
            };
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            handler.ProcessRequest(
                request,
                state,
                configurationRoot.GetSection(""),
                tokenSource.Token
            );
        }
    }
}