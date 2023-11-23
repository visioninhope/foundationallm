using FoundationaLLM.Vectorization.Interfaces;
using System;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Services.ContentSources
{
    public class BlobStorageContentSourceService : ContentSourceServiceBase, IContentSourceService
    {
        public BlobStorageContentSourceService()
        {
        }

        public async Task<BinaryData> ReadFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public async Task<BinaryData> ReadFileAsync(string index, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
