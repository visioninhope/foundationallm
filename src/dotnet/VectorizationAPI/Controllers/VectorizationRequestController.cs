using Asp.Versioning;
using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Vectorization.API.Controllers
{
    /// <summary>
    /// Methods for managing vectorization requests.
    /// </summary>
    [ApiVersion(1.0)]
    [ApiController]
    [APIKeyAuthentication]
    [Route("[controller]")]
    public class VectorizationRequestController : ControllerBase
    {
        readonly IVectorizationService _vectorizationService;

        /// <summary>
        /// Constructor for the vectorization request controller.
        /// </summary>
        /// <param name="vectorizationService"></param>
        public VectorizationRequestController(
            IVectorizationService vectorizationService) => _vectorizationService = vectorizationService;

        /// <summary>
        /// Handles an incoming vectorization request by starting a new vectorization pipeline.
        /// </summary>
        /// <param name="vectorizationRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task ProcessRequest([FromBody] VectorizationRequest vectorizationRequest) =>
            await _vectorizationService.ProcessRequest(vectorizationRequest);
    }
}
