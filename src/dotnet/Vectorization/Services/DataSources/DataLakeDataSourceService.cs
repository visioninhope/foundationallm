using FoundationaLLM.Common.Models.Configuration.Storage;
using FoundationaLLM.Common.Models.TextEmbedding;
using FoundationaLLM.Common.Services.Storage;
using FoundationaLLM.Vectorization.Interfaces;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Services.DataSources
{
    /// <summary>
    /// Implements a vectorization data source for content residing in blob storage.
    /// </summary>
    public class DataLakeDataSourceService : DataSourceServiceBase, IDataSourceService
    {
        private readonly BlobStorageServiceSettings _storageSettings;
        private readonly ILogger<DataLakeDataSourceService> _logger;
        private readonly DataLakeStorageService _dataLakeStorageService;

        /// <summary>
        /// Creates a new instance of the vectorization data source service.
        /// </summary>
        public DataLakeDataSourceService(
            BlobStorageServiceSettings storageSettings,
            ILoggerFactory loggerFactory)
        {
            _storageSettings = storageSettings;
            _logger = loggerFactory.CreateLogger<DataLakeDataSourceService>();
            _dataLakeStorageService = new DataLakeStorageService(
                _storageSettings,
                loggerFactory.CreateLogger<DataLakeStorageService>());
        }

        /// <inheritdoc/>
        /// <remarks>
        /// contentId[0] = the URL of the storage account.
        /// contentId[1] = the container name.
        /// contentId[2] = path of the file relative to the container name.
        /// </remarks>
        public async Task<string> ExtractTextAsync(ContentIdentifier contentId, CancellationToken cancellationToken)
        {   
            contentId.ValidateMultipartId(3);

            var binaryContent = await _dataLakeStorageService.ReadFileAsync(
                contentId[1],
                contentId[2],
                cancellationToken);

            return await ExtractTextFromFileAsync(contentId.FileName, binaryContent);
        }
    }
}
