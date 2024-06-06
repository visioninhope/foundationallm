using FoundationaLLM.Common.Authentication;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Vectorization.Services.VectorizationServices;
using Microsoft.AspNetCore.Mvc;

namespace FoundationaLLM.Vectorization.API.Controllers
{
    /// <summary>
    /// Methods for managing vectorization requests.
    /// </summary>
    /// <remarks>
    /// Constructor for the vectorization request controller.
    /// </remarks>
    /// <param name="vectorizationServiceFactory">The factory responsible for determining the approprate vectorization service for the request.</param>
    [ApiController]
    [APIKeyAuthentication]
    [Route("[controller]")]
    public class VectorizationRequestController(
        VectorizationServiceFactory vectorizationServiceFactory) : ControllerBase
    {
        readonly VectorizationServiceFactory _vectorizationServiceFactory = vectorizationServiceFactory;

        /// <summary>
        /// Handles an incoming vectorization request by starting a new vectorization pipeline.
        /// </summary>
        /// <param name="vectorizationRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ProcessRequest([FromBody] VectorizationRequest vectorizationRequest)
        {
            var vectorizationService = _vectorizationServiceFactory.GetService(vectorizationRequest);
            return new OkObjectResult(await vectorizationService.ProcessRequest(vectorizationRequest));
        }
            
    }
}
