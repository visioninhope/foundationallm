using FoundationaLLM.Common.Models.Configuration.Instance;
using FoundationaLLM.Core.Examples.Interfaces;
using FoundationaLLM.Core.Examples.Setup;
using Xunit.Abstractions;

namespace FoundationaLLM.Core.Examples
{
    public class Example0004_SynchronousVectorizationOfPDFFromDataLake: BaseTest, IClassFixture<TestFixture>
    {
        private readonly IVectorizationTestService _vectorizationTestService;
        private InstanceSettings _instanceSettings;

        public Example0004_SynchronousVectorizationOfPDFFromDataLake(ITestOutputHelper output, TestFixture fixture)
            : base(output, fixture.ServiceProvider)
        {
            _vectorizationTestService = GetService<IVectorizationTestService>();
            _instanceSettings = GetService<InstanceSettings>();
        }
    }
}
