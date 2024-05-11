using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.Orchestration;
using FoundationaLLM.Common.Models.Vectorization;
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
        /// Handles an incoming text embedding request by starting a new embedding operation.
        /// </summary>
        /// <param name="embeddingRequest">The <see cref="TextEmbeddingRequest"/> object with the details of the embedding request.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object with the outcome of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> StartCompletionOperation(
            [FromBody] CompletionRequest embeddingRequest) =>
            new OkObjectResult(await _gatewayCore.StartCompletionOperation(embeddingRequest));

        /// <summary>
        /// Retrieves the outcome of a text embedding operation.
        /// </summary>
        /// <param name="operationId">The unique identifier of the text embedding operation.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object with the outcome of the operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetCompletionOperationResult(string operationId) =>
            new OkObjectResult(await _gatewayCore.GetCompletionOperationResult(operationId));

        [HttpGet("TryConsume")]
        public async Task<IActionResult> TryConsume(string modelId, int tokenCount) =>
            new OkObjectResult(await _gatewayCore.TryConsume(modelId, tokenCount));

        [HttpGet("AddModel")]
        public async Task<IActionResult> AddModel(string modelId, int requestRateLimit, int requestRateRenewalPeriod, int tokenRateLimit, int tokenRateRenewalPeriod) =>
            new OkObjectResult(await _gatewayCore.AddModel( modelId,  requestRateLimit,  requestRateRenewalPeriod,  tokenRateLimit,  tokenRateRenewalPeriod));
    }
}
