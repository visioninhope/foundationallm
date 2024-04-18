using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Vectorization.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Vectorization.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="VectorizationRequest"/>.
    /// </summary>
    public static class VectorizationRequestExtensions
    {
        /// <summary>
        /// Updates the vectorization request resource using the vectorization resource provider.
        /// Also updates the vectorization pipeline state if request is part of a pipeline.
        /// </summary>
        /// <param name="request">The vectorization request</param>
        /// <param name="vectorizationResourceProvider">The vectorization resource provider</param>
        /// <param name="vectorizationStateService"></param>        
        public static async Task UpdateVectorizationRequestResource(
            this VectorizationRequest request,
            IResourceProviderService vectorizationResourceProvider,
            IVectorizationStateService vectorizationStateService
        )
        {            
            // use HandlePostAsync to go through Authorization layer using the managed identity of the vectorization API
            var jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();
            jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            var requestBody = JsonSerializer.Serialize(request, jsonSerializerOptions);
            var unifiedIdentity = new UnifiedUserIdentity
            {
                Name = "VectorizationAPI",
                UserId = "VectorizationAPI",
                Username = "VectorizationAPI"
            };
            // get the property ObjectId from the request object            
            await vectorizationResourceProvider.HandlePostAsync(request.ObjectId!, requestBody, unifiedIdentity);
            await request.UpdateVectorizationPipelineState(vectorizationStateService);
        }

        /// <summary>
        /// Updates the vectorization pipeline state.
        /// </summary>
        /// <param name="request">The vectorization request being updated in the pipeline state.</param>
        /// <param name="vectorizationStateService">The vectorization state service that persists the state.</param>        
        public static async Task UpdateVectorizationPipelineState(
            this VectorizationRequest request,
            IVectorizationStateService vectorizationStateService
        )
        {
            if (!string.IsNullOrWhiteSpace(request.PipelineObjectId) && !string.IsNullOrWhiteSpace(request.PipelineExecutionId))
            {
                var pipelineName = request.PipelineObjectId.Split('/').Last();
                var pipelineExecutionId = request.PipelineExecutionId;
                // get latest state of the pipeline execution.
                var pipelineState = await vectorizationStateService.ReadPipelineState(pipelineName, pipelineExecutionId);
                // update the request status with the current state.
                if (pipelineState.ProcessingState == VectorizationProcessingState.New && request.ProcessingState == VectorizationProcessingState.InProgress)
                {
                    pipelineState.ExecutionStart = DateTime.UtcNow;
                }

                // update the vectorization pipeline request states.
                pipelineState.VectorizationRequestStatuses[request.ObjectId!] = request.ProcessingState;

                if (pipelineState.ProcessingState == VectorizationProcessingState.Completed)
                {
                    pipelineState.ExecutionEnd = DateTime.UtcNow;
                }

                // persist the updated state of the pipeline.
                await vectorizationStateService.SavePipelineState(pipelineState);
            }
        }
    }
}
