using System;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Interfaces
{
    public interface IContentSourceService
    {
        Task<BinaryData> ReadFileAsync(string filePath);

        Task<BinaryData> ReadFileAsync(string index, string fileName);
    }
}
