using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.State.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.State.API.Controllers
{
    /// <summary>
    /// Provides methods for managing long-running operations.
    /// </summary>
    /// <param name="stateService">Provides methods for managing state for long-running operations.</param>
    [Authorize(Policy = "DefaultPolicy")]
    [ApiController]
    [Route("instances/{instanceId}/[controller]")]
    public class OperationsController(IStateService stateService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all long-running operations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLongRunningOperations()
        {
            var operations = await stateService.GetLongRunningOperationsAsync();
            return Ok(operations);
        }

        /// <summary>
        /// Retrieves a long-running operation by its identifier.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance ID.</param>
        /// <param name="operationId">The long-running operation identifier.</param>
        /// <returns></returns>
        [HttpGet("{operationId}")]
        public async Task<IActionResult> GetLongRunningOperation(string instanceId, string operationId)
        {
            var operation = await stateService.GetLongRunningOperationAsync(operationId);
            if (operation == null)
            {
                return NotFound();
            }
            return Ok(operation);
        }

        /// <summary>
        /// Retrieves all log entries for a long-running operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance ID.</param>
        /// <param name="operationId">The long-running operation identifier.</param>
        /// <returns></returns>
        [HttpGet("{operationId}/logs")]
        public async Task<IActionResult> GetLongRunningOperationLogs(string instanceId, string operationId)
        {
            var logEntries = await stateService.GetLongRunningOperationLogEntriesAsync(operationId);
            return Ok(logEntries);
        }

        /// <summary>
        /// Inserts a long-running operation and creates a log entry.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance ID.</param>
        /// <param name="operation">The long-running operation entry to create.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateLongRunningOperation(string instanceId, [FromBody] LongRunningOperation operation)
        {
            var success = await stateService.UpsertLongRunningOperationAsync(operation);
            if (success)
            {
                return CreatedAtAction(nameof(GetLongRunningOperation), new { id = operation.Id }, operation);
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates a long-running operation and creates a log entry.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance ID.</param>
        /// <param name="operationId">The long-running operation identifier.</param>
        /// <param name="operation">The long-running operation entry to update.</param>
        /// <returns></returns>
        [HttpPut("{operationId}")]
        public async Task<IActionResult> UpdateLongRunningOperation(string instanceId, string operationId, [FromBody] LongRunningOperation operation)
        {
            if (operationId != operation.Id)
            {
                return BadRequest("The operation ID in the request path does not match the operation ID of the object in the request body.");
            }
            var success = await stateService.UpsertLongRunningOperationAsync(operation);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }

        /// <summary>
        /// Retrieves the result of a long-running operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance ID.</param>
        /// <param name="operationId">The long-running operation identifier.</param>
        /// <returns></returns>
        [HttpGet("{operationId}/result")]
        public async Task<IActionResult> GetLongRunningOperationResult(string instanceId, string operationId)
        {
            var operation = await stateService.GetLongRunningOperationResultAsync(operationId);
            if (operation == null)
            {
                return NotFound();
            }
            return Ok(operation);
        }

        /// <summary>
        /// Inserts the result of a long-running operation.
        /// </summary>
        /// <param name="instanceId">The FoundationaLLM instance ID.</param>
        /// <param name="operationId">The long-running operation identifier.</param>
        /// <param name="operationResult">The operation result to insert.</param>
        /// <returns></returns>
        [HttpPost("{operationId}/result")]
        public async Task<IActionResult> CreateLongRunningOperationResult(string instanceId, string operationId, [FromBody] dynamic operationResult)
        {
            if (operationId != operationResult.Id)
            {
                return BadRequest("The operation ID in the request path does not match the operation ID of the object in the request body.");
            }
            var success = await stateService.UpsertLongRunningOperationResultAsync(operationResult);
            if (success)
            {
                return CreatedAtAction(nameof(GetLongRunningOperation), new { id = operationResult.Id }, operationResult);
            }
            return BadRequest();
        }
    }
}
