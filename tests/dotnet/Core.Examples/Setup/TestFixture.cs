using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Environment = FoundationaLLM.Core.Examples.Utils.Environment;

namespace FoundationaLLM.Core.Examples.Setup
{
	/// <summary>
	/// Test fixture to initialize services for the tests.
	/// </summary>
	public class TestFixture : IDisposable
	{
		public ServiceProvider ServiceProvider { get; private set; }

		public TestFixture()
		{
			var serviceCollection = new ServiceCollection();

			var configRoot = new ConfigurationBuilder()
				.AddJsonFile("appsettings.Development.json", true)
				.AddEnvironmentVariables()
				.AddUserSecrets<Environment>()
				.Build();

			TestServicesInitializer.InitializeServices(serviceCollection, configRoot);

			ServiceProvider = serviceCollection.BuildServiceProvider();
		}

		public void Dispose()
		{
			if (ServiceProvider is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
	}
}
