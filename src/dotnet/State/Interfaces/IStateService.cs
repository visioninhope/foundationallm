using FoundationaLLM.Common.Models.Orchestration;

namespace FoundationaLLM.State.Interfaces
{
    /// <summary>
    /// Provides methods for managing state for long-running operations.
    /// </summary>
    public interface IStateService
    {
        /// <summary>
        /// Retrieves all long-running operations.
        /// </summary>
        /// <returns></returns>
        Task<List<LongRunningOperation>> GetLongRunningOperationsAsync();

        /// <summary>
        /// Retrieves a long-running operation by its identifier.
        /// </summary>
        /// <param name="id">The long-running operation identifier.</param>
        /// <returns></returns>
        Task<LongRunningOperation> GetLongRunningOperationAsync(string id);

        /// <summary>
        /// Retrieves all log entries for a long-running operation.
        /// </summary>
        /// <param name="operationId">The long-running operation identifier.</param>
        /// <returns></returns>
        Task<List<LongRunningOperationLogEntry>> GetLongRunningOperationLogEntriesAsync(string operationId);

        /// <summary>
        /// Retrieves the result of a long-running operation.
        /// </summary>
        /// <typeparam name="T">Define the type used to deserialize the operation result object.</typeparam>
        /// <param name="operationId">The long-running operation identifier.</param>
        /// <returns></returns>
        Task<object?> GetLongRunningOperationResultAsync(string operationId);

        /// <summary>
        /// Inserts or updates a long-running operation and creates a log entry.
        /// </summary>
        /// <param name="operation">The long-running operation to insert or update.</param>
        /// <returns></returns>
        Task<LongRunningOperation> UpsertLongRunningOperationAsync(LongRunningOperation operation);

        /// <summary>
        /// Inserts or updates the result of a long-running operation.
        /// </summary>
        /// <param name="operationResult">The operation result to insert or update.</param>
        /// <returns></returns>
        Task<object?> UpsertLongRunningOperationResultAsync(dynamic operationResult);
    }
}
