using FoundationaLLM.Vectorization.Interfaces;
using System;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Services.ContentSources
{
    /// <summary>
    /// Implements a vectorization content source for content residing in blob storage.
    /// </summary>
    public class BlobStorageContentSourceService : ContentSourceServiceBase, IContentSourceService
    {
        /// <summary>
        /// Creates a new instance of the vectorization content source.
        /// </summary>
        public BlobStorageContentSourceService()
        {
        }

        /// <inheritdoc/>
        public async Task<BinaryData> ReadFileAsync(string filePath)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
