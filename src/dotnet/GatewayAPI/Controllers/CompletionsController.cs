using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Gateway.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Gateway.API.Controllers
{
    [ApiController]
    [APIKeyAuthentication]
    [Route("[controller]")]
    public class CompletionsController(
        IGatewayCore gatewayCore)
    {
        readonly IGatewayCore _gatewayCore = gatewayCore;

        /// <summary>
        /// Handles an incoming text completion request by starting a new completion operation.
        /// </summary>
        /// <param name="embeddingRequest">The <see cref="GatewayCompletionRequest"/> object with the details of the completion request.</param>
        /// <returns>A <see cref="GatewayCompletionResult"/> object with the outcome of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> StartCompletionOperation(
            [FromBody] GatewayCompletionRequest embeddingRequest) =>
            new OkObjectResult(await _gatewayCore.StartCompletionOperation(embeddingRequest));

        /// <summary>
        /// Retrieves the outcome of a text completion operation.
        /// </summary>
        /// <param name="operationId">The unique identifier of the text completion operation.</param>
        /// <returns>A <see cref="GatewayCompletionResult"/> object with the outcome of the operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetCompletionOperationResult(string operationId) =>
            new OkObjectResult(await _gatewayCore.GetCompletionOperationResult(operationId));

        [HttpGet("TryConsume")]
        public async Task<IActionResult> TryConsume(string modelId, int tokenCount) =>
            new OkObjectResult(await _gatewayCore.TryConsume(modelId, tokenCount));

        [HttpGet("AddCompletionModel")]
        public async Task<IActionResult> AddCompletionModel(string modelId, int requestRateLimit, int requestRateRenewalPeriod, int tokenRateLimit, int tokenRateRenewalPeriod) =>
            new OkObjectResult(await _gatewayCore.AddCompletionModel( modelId,  requestRateLimit,  requestRateRenewalPeriod,  tokenRateLimit,  tokenRateRenewalPeriod));

        [HttpGet("AddEmbeddingModel")]
        public async Task<IActionResult> AddEmbeddingModel(string modelId, int requestRateLimit, int requestRateRenewalPeriod, int tokenRateLimit, int tokenRateRenewalPeriod) =>
            new OkObjectResult(await _gatewayCore.AddEmbeddingModel(modelId, requestRateLimit, requestRateRenewalPeriod, tokenRateLimit, tokenRateRenewalPeriod));
    }
}
