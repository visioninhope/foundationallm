using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.State.Interfaces;
using FoundationaLLM.State.Models.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FoundationaLLM.State.Services
{
    /// <summary>
    /// Provides methods for managing state for long-running operations.
    /// </summary>
    /// <param name="options">Provides the options with the <see cref="StateServiceSettings"/> settings for configuration.</param>
    /// <param name="cosmosDbService">Provides methods to interact with Cosmos DB.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    public class StateService(
        IOptions<StateServiceSettings> options,
        ICosmosDbService cosmosDbService,
        ILogger<StateService> logger) : IStateService
    {
        /// <inheritdoc/>
        public async Task<List<LongRunningOperation>> GetLongRunningOperationsAsync()
        {
            logger.LogInformation("Getting long running operations.");
            return await cosmosDbService.GetLongRunningOperationsAsync();
        }

        /// <inheritdoc/>
        public async Task<LongRunningOperation> GetLongRunningOperationAsync(string id)
        {
            logger.LogInformation("Getting long running operation with ID: {id}", id);
            return await cosmosDbService.GetLongRunningOperationAsync(id);
        }

        /// <inheritdoc/>
        public async Task<List<LongRunningOperationLogEntry>> GetLongRunningOperationLogEntriesAsync(string operationId)
        {
            logger.LogInformation("Getting long running operation log entries for operation ID: {operationId}", operationId);
            return await cosmosDbService.GetLongRunningOperationLogEntriesAsync(operationId);
        }

        /// <inheritdoc/>
        public async Task<object?> GetLongRunningOperationResultAsync(string operationId)
        {
            logger.LogInformation("Getting long running operation result for operation ID: {operationId}", operationId);
            return await cosmosDbService.GetLongRunningOperationResultAsync(operationId);
        }

        /// <inheritdoc/>
        public async Task<bool> UpsertLongRunningOperationAsync(LongRunningOperation operation)
        {
            logger.LogInformation("Upserting long running operation.");
            return await cosmosDbService.UpsertLongRunningOperationAsync(operation);
        }

        /// <inheritdoc/>
        public async Task<bool> UpsertLongRunningOperationResultAsync(dynamic operationResult)
        {
            logger.LogInformation("Upserting long running operation result.");
            return await cosmosDbService.UpsertLongRunningOperationResultAsync(operationResult);
        }
    }
}
