using FoundationaLLM.Common.Constants;
using FoundationaLLM.Vectorization.Exceptions;
using FoundationaLLM.Vectorization.Interfaces;
using Microsoft.Extensions.Logging;

namespace FoundationaLLM.Vectorization.Handlers
{
    /// <summary>
    /// Implements a factory that creates vectorization step handlers.
    /// </summary>
    public class VectorizationStepHandlerFactory
    {
        /// <summary>
        /// Creates a vectorization step handler capable of handling a specified vectorization pipeline step.
        /// </summary>
        /// <param name="step">The identifier of the vectorization pipeline step for which the handler is created.</param>
        /// <param name="parameters">The parameters used to initialize the vectorization step handler.</param>
        /// <param name="loggerFactory">The logger factory used to create loggers.</param>
        /// <returns>A class implementing <see cref="IVectorizationStepHandler"/>.</returns>
        public static IVectorizationStepHandler Create(string step, Dictionary<string, string> parameters, ILoggerFactory loggerFactory) =>
            step switch
            {
                VectorizationSteps.Extract => new ExtractionHandler(parameters, loggerFactory.CreateLogger<ExtractionHandler>()),
                VectorizationSteps.Partition => new PartitionHandler(parameters, loggerFactory.CreateLogger<PartitionHandler>()),
                VectorizationSteps.Embed => new EmbeddingHandler(parameters, loggerFactory.CreateLogger<EmbeddingHandler>()),
                VectorizationSteps.Index => new IndexingHandler(parameters, loggerFactory.CreateLogger<IndexingHandler>()),
                _ => throw new VectorizationException($"There is no handler available for the vectorization pipeline step [{step}]."),
            };
    }
}
