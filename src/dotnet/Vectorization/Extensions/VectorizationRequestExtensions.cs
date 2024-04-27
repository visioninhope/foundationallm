using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Interfaces;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Vectorization;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Vectorization.Interfaces;
using FoundationaLLM.Vectorization.Services;
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
            IVectorizationStateService? vectorizationStateService = null
        )
        {            
            // use HandlePostAsync to go through Authorization layer using the managed identity of the vectorization API
            var jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();
            jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            var requestBody = JsonSerializer.Serialize(request, jsonSerializerOptions);
            var unifiedIdentity = new VectorizationServiceUnifiedUserIdentity();
            
            if (request.ObjectId == null)
            {               
                //build the minimal object id for new requests
                request.ObjectId = $"/{VectorizationResourceTypeNames.VectorizationRequests}/{request.Id}";
            }
            
            var response = (await vectorizationResourceProvider.HandlePostAsync(request.ObjectId, requestBody, unifiedIdentity)) as ResourceProviderUpsertResult;
            // in the case of a new request, this updates the object id with the fully qualified object id, otherwise it remains the same.
            request.ObjectId = response!.ObjectId;                    
        }

      
        /// <summary>
        /// Issues the "process" action on the vectorization request resource using the vectorization resource provider.        
        /// </summary>
        /// <param name="request">The vectorization request</param>
        /// <param name="vectorizationResourceProvider">The vectorization resource provider</param>              
        public static async Task<VectorizationResult> ProcessVectorizationRequest(
            this VectorizationRequest request,
            IResourceProviderService vectorizationResourceProvider
        )
        {
            // use HandlePostAsync to go through Authorization layer using the managed identity of the vectorization API
            var jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();
            jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            var requestBody = JsonSerializer.Serialize(request, jsonSerializerOptions);
            var unifiedIdentity = new VectorizationServiceUnifiedUserIdentity();
            var response = (await vectorizationResourceProvider.HandlePostAsync(request.ObjectId! +"/process", requestBody, unifiedIdentity)) as VectorizationResult;
            return response!;
        }
    }
}
