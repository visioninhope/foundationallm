namespace FoundationaLLM.Core.Examples.Interfaces;

/// <summary>
/// Manages retrieving and appending headers to HttpClients.
/// </summary>
/// <param name="httpClientFactory"></param>
/// <param name="httpClientOptions"></param>
public interface IHttpClientManager
{
    /// <summary>
    /// Retrieves an HttpClient with the appropriate headers for the specified API type.
    /// </summary>
    /// <param name="apiType"></param>
    /// <returns></returns>
    Task<HttpClient> GetHttpClientAsync(string apiType);
}