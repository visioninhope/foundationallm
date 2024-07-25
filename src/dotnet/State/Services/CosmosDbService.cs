using FoundationaLLM.Common.Constants;
using FoundationaLLM.Common.Models.Configuration.CosmosDB;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.State.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace FoundationaLLM.State.Services
{
    /// <summary>
    /// Service to access Azure Cosmos DB for NoSQL.
    /// </summary>
    public class CosmosDbService : ICosmosDbService
    {
        private readonly Container _state;
        private readonly CosmosDbSettings _settings;
        private readonly ILogger _logger;

        private const string SoftDeleteQueryRestriction = " (not IS_DEFINED(c.deleted) OR c.deleted = false)";

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDbService"/> class.
        /// </summary>
        /// <param name="settings">The <see cref="CosmosDbSettings"/> settings retrieved
        /// by the injected <see cref="IOptions{TOptions}"/>.</param>
        /// <param name="client">The Cosmos DB client.</param>
        /// <param name="logger">The logging interface used to log under the
        /// <see cref="CosmosDbService"></see> type name.</param>
        /// <exception cref="ArgumentException">Thrown if any of the required settings
        /// are null or empty.</exception>
        public CosmosDbService(
            IOptions<CosmosDbSettings> settings,
            CosmosClient client,
            ILogger<CosmosDbService> logger)
        {
            _settings = settings.Value;
            ArgumentException.ThrowIfNullOrEmpty(_settings.Endpoint);
            ArgumentException.ThrowIfNullOrEmpty(_settings.Database);

            _logger = logger;
            _logger.LogInformation("Initializing Cosmos DB service.");

            if (!_settings.EnableTracing)
            {
                var defaultTrace =
                    Type.GetType("Microsoft.Azure.Cosmos.Core.Trace.DefaultTrace,Microsoft.Azure.Cosmos.Direct");
                var traceSource = (TraceSource) defaultTrace?.GetProperty("TraceSource")?.GetValue(null)!;
                traceSource.Switch.Level = SourceLevels.All;
                traceSource.Listeners.Clear();
            }

            var database = client?.GetDatabase(_settings.Database);

            if (database == null)
            {
                throw new ArgumentException("Unable to connect to existing Azure Cosmos DB database.");
            }

            _state = database?.GetContainer(CosmosDbContainers.State) ??
                 throw new ArgumentException(
                     $"Unable to connect to existing Azure Cosmos DB container ({CosmosDbContainers.State}).");

            _logger.LogInformation("Cosmos DB service initialized.");
        }

        /// <inheritdoc/>
        public async Task<List<LongRunningOperation>> GetLongRunningOperationsAsync(CancellationToken cancellationToken = default)
        {
            var query = new QueryDefinition(
                    $"SELECT DISTINCT * FROM c WHERE c.type = @type AND {SoftDeleteQueryRestriction} ORDER BY c._ts DESC")
                .WithParameter("@type", nameof(LongRunningOperation));

            var response = _state.GetItemQueryIterator<LongRunningOperation>(query);

            List<LongRunningOperation> output = [];
            while (response.HasMoreResults)
            {
                var results = await response.ReadNextAsync(cancellationToken);
                output.AddRange(results);
            }

            return output;
        }

        /// <inheritdoc/>
        public async Task<LongRunningOperation> GetLongRunningOperationAsync(string id, CancellationToken cancellationToken = default)
        {
            var record = await _state.ReadItemAsync<LongRunningOperation>(
                id: id,
                partitionKey: new PartitionKey(id),
                cancellationToken: cancellationToken);
            
            return record;
        }

        /// <inheritdoc/>
        public async Task<List<LongRunningOperationLogEntry>> GetLongRunningOperationLogEntriesAsync(string operationId, CancellationToken cancellationToken = default)
        {
            var query =
                new QueryDefinition($"SELECT * FROM c WHERE c.operation_id = @operationId AND c.type = @type AND {SoftDeleteQueryRestriction}")
                    .WithParameter("@operationId", operationId)
                    .WithParameter("@type", nameof(LongRunningOperationLogEntry));

            var results = _state.GetItemQueryIterator<LongRunningOperationLogEntry>(query);

            List<LongRunningOperationLogEntry> output = new();
            while (results.HasMoreResults)
            {
                var response = await results.ReadNextAsync(cancellationToken);
                output.AddRange(response);
            }

            return output;
        }

        /// <inheritdoc/>
        public async Task<T?> GetLongRunningOperationResultAsync<T>(string operationId, CancellationToken cancellationToken = default)
        {
            var query =
                new QueryDefinition($"SELECT TOP 1 * FROM c WHERE c.operation_id = @operationId AND c.type = @type AND {SoftDeleteQueryRestriction} ORDER BY c._ts DESC")
                    .WithParameter("@operationId", operationId)
                    .WithParameter("@type", nameof(T));

            var results = _state.GetItemQueryIterator<T>(query);

            // There should just be a single result that has the operation_id and type. Get that result and return it.
            if (results.HasMoreResults)
            {
                var response = await results.ReadNextAsync(cancellationToken);
                return response.FirstOrDefault();
            }

            return default;
        }

        /// <inheritdoc/>
        public async Task<bool> UpsertLongRunningOperationAsync(LongRunningOperation operation, CancellationToken cancellationToken = default)
        {
            PartitionKey partitionKey = new(operation.OperationId);
            var batch = _state.CreateTransactionalBatch(partitionKey);
            batch.UpsertItem(
                item: operation
            );
            batch.CreateItem(
                item: new LongRunningOperationLogEntry(operation.OperationId, operation.Status, operation.StatusMessage)
            );

            var result = await batch.ExecuteAsync(cancellationToken);

            return result.IsSuccessStatusCode;
        }

        /// <inheritdoc/>
        public async Task<bool> UpsertLongRunningOperationResultAsync(dynamic operationResult, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(operationResult.OperationId))
            {
                throw new ArgumentException("OperationResult must have an operation_id.");
            }

            PartitionKey partitionKey = new(operationResult.OperationId);
            var result = await _state.UpsertItemAsync(
                item: operationResult,
                partitionKey: partitionKey,
                cancellationToken: cancellationToken
            );

            return result.IsSuccessStatusCode;
        }
    }
}
