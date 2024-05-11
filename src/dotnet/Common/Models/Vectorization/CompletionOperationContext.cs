using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Vectorization;

namespace FoundationaLLM.Gateway.Models
{
    /// <summary>
    /// Maintains the context for a completion operation.
    /// </summary>
    public class CompletionOperationContext : OperationContext
    {
        private readonly object _syncRoot = new();

        public required GatewayCompletionRequest CompletionRequest { get; set; }

        /// <summary>
        /// The <see cref="CompletionResult"/> holding the result of the completion operation.
        /// </summary>
        public required CompletionResult Result { get; set; }

        /// <summary>
        /// Sets a specified error message on the context of the completion operation.
        /// </summary>
        /// <param name="errorMessage">The error message to be set.</param>
        public void SetError(string errorMessage)
        {
            lock (_syncRoot)
            {
                Result.ErrorMessage = errorMessage;
                Result.Failed = true;
                Result.InProgress = false;
            }
        }

        /// <summary>
        /// Marks the completion operation as complete.
        /// </summary>
        public void SetComplete()
        {
            lock ( _syncRoot)
            {
                Result.InProgress = false;
            }
        }
        
        public void SetCompletion(CompletionResponse response)
        {
            lock(_syncRoot)
            {
                Result.Result = response;
                Result.InProgress = false;
            }
        }
    }
}
