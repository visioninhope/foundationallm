using Core.Examples.LoadTests.ResourceProviders;
using FoundationaLLM.Core.Examples.LoadTests.Setup;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples.LoadTests
{
    /// <summary>
    /// Runs load tests on resource provider resource references.
    /// </summary>
    public class Example0001_ResourceProviderResourceReferences : BaseTest, IClassFixture<LoadTestFixture>
    {
        public Example0001_ResourceProviderResourceReferences(ITestOutputHelper output, LoadTestFixture fixture)
			: base(output, fixture.ServiceProviders)
        {
        }

        [Fact]
        public async Task RunAsync()
        {
            WriteLine("============ FoundationaLLM Resource Provider Resource References ============");

            var resourceProviders = ServiceProviders
                .Select(sp => new LoadTestResourceProviders(sp))
                .ToList();

           
        }
    }
}