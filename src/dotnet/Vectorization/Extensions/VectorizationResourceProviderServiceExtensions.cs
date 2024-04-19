using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Models.Authentication;
using FoundationaLLM.Common.Models.Vectorization;
using FoundationaLLM.Common.Settings;
using FoundationaLLM.Vectorization.ResourceProviders;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Vectorization.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="VectorizationResourceProviderService"/>.
    /// </summary>
    public static class VectorizationResourceProviderServiceExtensions
    {
        /// <summary>
        /// Retrieves all active vectorization pipelines.
        /// </summary>
        /// <param name="vectorizationResourceProvider">An instance of the vectorization resource provider service</param>
        /// <returns>List of active pipelines.</returns>
        public static async Task<List<VectorizationPipeline>> GetActivePipelines(this VectorizationResourceProviderService vectorizationResourceProvider)
        {
            var pipelines = await vectorizationResourceProvider.HandleGetAsync(
                $"/{VectorizationResourceTypeNames.VectorizationPipelines}",
                GetUnifiedIdentity());
            return (pipelines as List<VectorizationPipeline>)!.Where(p => p.Active).ToList();
        }

        /// <summary>
        /// Sets the specified vectorization pipeline to active or inactive.
        /// </summary>
        /// <param name="vectorizationResourceProvider">An instance of the vectorization resource provider.</param>
        /// <param name="pipelineObjectId">The object id of the pipeline to deactivate</param>
        /// <param name="activate">true if the pipeline should be activated, false if it is to be deactivated.</param>
        /// <returns></returns>
        public static async Task TogglePipelineActivation(this VectorizationResourceProviderService vectorizationResourceProvider, string pipelineObjectId, bool activate)
        {
            var unifiedIdentity = GetUnifiedIdentity();
            var results = await vectorizationResourceProvider.HandleGetAsync(
                               pipelineObjectId,
                               unifiedIdentity) as List<VectorizationPipeline>;
            if (results == null || results.Count==0)
                //nothing to update
                return;

            var pipeline = results.FirstOrDefault();
                        
            if (pipeline == null || pipeline.Active == activate)
                // nothing to update
                return;

            // update the pipeline active state
            pipeline.Active = activate;
            var jsonSerializerOptions = CommonJsonSerializerOptions.GetJsonSerializerOptions();
            jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            var requestBody = JsonSerializer.Serialize(pipeline, jsonSerializerOptions);
            await vectorizationResourceProvider.HandlePostAsync(pipelineObjectId, requestBody, unifiedIdentity);
            
        }

        private static UnifiedUserIdentity GetUnifiedIdentity() => new UnifiedUserIdentity
        {
            Name = "VectorizationAPI",
            UserId = "VectorizationAPI",
            Username = "VectorizationAPI"
        };
    }
}
