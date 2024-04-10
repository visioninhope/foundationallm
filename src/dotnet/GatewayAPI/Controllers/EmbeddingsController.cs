using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Gateway.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Gateway.API.Controllers
{
    /// <summary>
    /// Methods for managing embedding requests.
    /// </summary>
    /// <param name="gatewayService">The <see cref="IGatewayService"/> that provides LLM gateway services.</param>
    [ApiController]
    [APIKeyAuthentication]
    [Route("[controller]")]
    public class EmbeddingsController(
        IGatewayService gatewayService)
    {
        readonly IGatewayService _gatewayService = gatewayService;

        /// <summary>
        /// Handles an incoming text embedding request by starting a new embedding operation.
        /// </summary>
        /// <param name="embeddingRequest">The <see cref="TextEmbeddingRequest"/> object with the details of the embedding request.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object with the outcome of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> StartEmbeddingOperation(
            [FromBody] TextEmbeddingRequest embeddingRequest) =>
            new OkObjectResult(await _gatewayService.StartEmbeddingOperation(embeddingRequest));

        /// <summary>
        /// Retrieves the outcome of a text embedding operation.
        /// </summary>
        /// <param name="operationId">The unique identifier of the text embedding operation.</param>
        /// <returns>A <see cref="TextEmbeddingResult"/> object with the outcome of the operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetEmbeddingOperationResult(string operationId) =>
            new OkObjectResult(await _gatewayService.GetEmbeddingOperationResult(operationId));
    }
}
