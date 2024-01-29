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
            ValidateMultipartId(multipartId, 5);

            var binaryContent = await GetBinaryContent(
                multipartId[0],
                multipartId[1],
                multipartId[2],
                multipartId[3],
                multipartId[4],
                cancellationToken);

            return await ExtractTextFromFileAsync(multipartId[4], binaryContent);
        }

        /// <summary>
        /// Retrieves the binary content.
        /// </summary>
        /// <param name="schema">The database schema containing the target table.</param>
        /// <param name="tableName">The name of the table from which to retrieve binary content.</param>
        /// <param name="contentColumnName">The name of the column containing binary data.</param>
        /// <param name="identifierColumnName">The name of the column used for identifying the specific record.</param>
        /// <param name="identifierValue">The value identifying the specific record in the identifier column.</param>
        /// <param name="cancellationToken">The cancellation token that signals that operations should be cancelled</param>
        /// <returns>An object representing the binary contents.</returns>
        private async Task<BinaryData> GetBinaryContent(string schema, string tableName, string contentColumnName, string identifierColumnName, string identifierValue, CancellationToken cancellationToken)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_settings.ConnectionString))
                {
                    await connection.OpenAsync(cancellationToken);

                    using (SqlCommand command = new SqlCommand($"SELECT TOP 1 {contentColumnName} FROM [{schema}].[{tableName}] WHERE {identifierColumnName} = `{identifierValue}`", connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
                        {
                            await reader.ReadAsync();
                            return new BinaryData(reader[contentColumnName]);
                        }
                    }
                }
            }
            catch
            {
                throw new VectorizationException($"Error when extracting content from file identified by {identifierValue} in Azure SQL Database.");
            }
        }
    }
}
