using FoundationaLLM.Common.Constants;
using FoundationaLLM.Vectorization.DataFormats.PDF;
using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Services.ContentSources
{
    /// <summary>
    /// Implements a vectorization content source for content residing in Azure SQL.
    /// </summary>
    public class AzureSQLDatabaseContentSourceService : ContentSourceServiceBase, IContentSourceService
    {
        private readonly ILogger<AzureSQLDatabaseContentSourceService> _logger;
        private readonly AzureSQLDatabaseContentSourceServiceSettings _settings;

        /// <summary>
        /// Creates a new instance of the vectorization content source.
        /// </summary>
        public AzureSQLDatabaseContentSourceService(
            AzureSQLDatabaseContentSourceServiceSettings settings,
            ILoggerFactory loggerFactory)
        {
            _settings = settings;
            _logger = loggerFactory.CreateLogger<AzureSQLDatabaseContentSourceService>();
        }

        /// <inheritdoc/>
        public async Task<string> ExtractTextFromFileAsync(List<string> multipartId, CancellationToken cancellationToken)
        {
            ValidateMultipartId(multipartId, 7);

            var binaryContent = await GetBinaryContent(
                multipartId[0],
                multipartId[1],
                multipartId[2],
                multipartId[3],
                multipartId[4],
                cancellationToken);

            var fileExtension = multipartId[5];

            return fileExtension.ToLower() switch
            {
                FileExtensions.Text => String.Empty,
                FileExtensions.PDF => PDFTextExtractor.GetText(binaryContent),
                FileExtensions.Word => String.Empty,
                FileExtensions.Excel => String.Empty,
                FileExtensions.PowerPoint => String.Empty,
                _ => throw new VectorizationException($"The file type for {fileExtension} is not supported."),
            };
        }

        /// <summary>
        /// Retrieves the binary content.
        /// </summary>
        /// <param name="schema">The database schema containing the target table.</param>
        /// <param name="tableName">The name of the table from which to retrieve binary content.</param>
        /// <param name="dataColumnName">The name of the column containing binary data.</param>
        /// <param name="identifierColumnName">The name of the column used for identifying the specific record.</param>
        /// <param name="identifierValue">The value identifying the specific record in the identifier column.</param>
        /// <param name="cancellationToken">The cancellation token that signals that operations should be cancelled</param>
        /// <returns>An object representing the binary contents.</returns>
        private async Task<BinaryData> GetBinaryContent(string schema, string tableName, string dataColumnName, string identifierColumnName, string identifierValue, CancellationToken cancellationToken)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
                {
                    await connection.OpenAsync(cancellationToken);

                    using (SqlCommand command = new SqlCommand($"SELECT {dataColumnName} FROM [{schema}].[{tableName}] WHERE {identifierColumnName} = `{identifierValue}`", connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
                        {
                            await reader.ReadAsync();
                            return new BinaryData(reader[dataColumnName]);
                        }
                    }
                }
            }
            catch
            {
                throw new VectorizationException($"Error when extracting content from file with ID {identifierValue} in Azure SQL Database.");
            }
        }
    }
}
