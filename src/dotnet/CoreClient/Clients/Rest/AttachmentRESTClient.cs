using System.Net.Http.Headers;
using FoundationaLLM.Client.Core.Interfaces;

namespace FoundationaLLM.Client.Core.Clients.Rest
{
    /// <summary>
    /// Provides methods to manage calls to the Core API's Attachments endpoints.
    /// </summary>
    public class AttachmentRESTClient(IHttpClientFactory httpClientFactory) : CoreRESTClientBase(httpClientFactory), IAttachmentRESTClient
    {
        /// <inheritdoc/>
        public async Task<string> UploadAttachmentAsync(Stream fileStream, string fileName, string contentType, string token)
        {
            var coreClient = GetCoreClient(token);
            var content = new MultipartFormDataContent
            {
                { new StreamContent(fileStream)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue(contentType)
                    }
                }, "file", fileName }
            };

            var responseMessage = await coreClient.PostAsync("attachments/upload", content);

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to upload attachment. Status code: {responseMessage.StatusCode}. Reason: {responseMessage.ReasonPhrase}");
            }

            return await responseMessage.Content.ReadAsStringAsync();
        }
    }
}
