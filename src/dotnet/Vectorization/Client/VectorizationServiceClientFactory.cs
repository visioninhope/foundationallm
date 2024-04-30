using FoundationaLLM.Vectorization.Interfaces;
using Microsoft.Extensions.DependencyInjection;

/// <inheritdoc/>
public class VectorizationServiceClientFactory : IVectorizationServiceClientFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="VectorizationServiceClientFactory"/> class.
    /// </summary>
    /// <param name="serviceProvider"></param>   
    public VectorizationServiceClientFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    /// <inheritdoc/>
    public IVectorizationServiceClient CreateClient()
    {
        using var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IVectorizationServiceClient>();
    }
}
