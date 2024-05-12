using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Gateway.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Gateway.API.Controllers
{
    /// <summary>
    /// Methods for managing embedding requests.
    /// </summary>
    /// <param name="gatewayCore">The <see cref="IGatewayCore"/> that provides LLM gateway services.</param>
    [ApiController]
    [APIKeyAuthentication]
    [Route("[controller]")]
    public class EmbeddingsController(
        IGatewayCore gatewayCore)
    {
        readonly IGatewayCore _gatewayCore = gatewayCore;

        /// <summary>
        /// Handles an incoming text embedding request by starting a new embedding operation.
        /// </summary>
        /// <param name="embeddingRequest">The <see cref="TextEmbeddingRequest"/> object with the details of the embedding request.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object with the outcome of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> StartEmbeddingOperation(
            [FromBody] TextEmbeddingRequest embeddingRequest) =>
            new OkObjectResult(await _gatewayCore.StartEmbeddingOperation(embeddingRequest));

        /// <summary>
        /// Retrieves the outcome of a text embedding operation.
        /// </summary>
        /// <param name="operationId">The unique identifier of the text embedding operation.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object with the outcome of the operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetEmbeddingOperationResult(string operationId) =>
            new OkObjectResult(await _gatewayCore.GetEmbeddingOperationResult(operationId));

        [HttpGet("TryConsume")]
        public async Task<IActionResult> TryConsume(string modelId, int tokenCount) =>
            new OkObjectResult(await _gatewayCore.TryConsume(modelId, tokenCount));

        [HttpGet("AddCompletionModel")]
        public async Task<IActionResult> AddCompletionModel(string modelId, int requestRateLimit, int requestRateRenewalPeriod, int tokenRateLimit, int tokenRateRenewalPeriod) =>
            new OkObjectResult(await _gatewayCore.AddCompletionModel(modelId, requestRateLimit, requestRateRenewalPeriod, tokenRateLimit, tokenRateRenewalPeriod));

        [HttpGet("AddEmbeddingModel")]
        public async Task<IActionResult> AddEmbeddingModel(string modelId, int requestRateLimit, int requestRateRenewalPeriod, int tokenRateLimit, int tokenRateRenewalPeriod) =>
            new OkObjectResult(await _gatewayCore.AddEmbeddingModel(modelId, requestRateLimit, requestRateRenewalPeriod, tokenRateLimit, tokenRateRenewalPeriod));

    }
}
