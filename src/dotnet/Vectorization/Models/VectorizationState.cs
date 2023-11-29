using FoundationaLLM.Vectorization.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.Vectorization.Models
{
    public class VectorizationState
    {
        /// <summary>
        /// The unique identifier of the content (i.e., document) being vectorized.
        /// </summary>
        [JsonPropertyOrder(1)]
        [JsonPropertyName("content_id")]
        public required string ContentId { get; set; }

        /// <summary>
        /// The unique identifier of the current vectorization request. Subsequent vectorization requests
        /// referring to the same content will have different unique identifiers.
        /// </summary>
        [JsonPropertyOrder(0)]
        [JsonPropertyName("request_id")]
        public required string CurrentRequestId { get; set; }

        [JsonPropertyOrder(18)]
        [JsonPropertyName("log")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<VectorizationLogEntry> LogEntries { get; set; } = new List<VectorizationLogEntry>();

        public void Log(IVectorizationStepHandler handler, string text)
        {
            LogEntries.Add(new VectorizationLogEntry(
                handler.StepId, text));
        }

        public void LogHandlerStart(IVectorizationStepHandler handler)
        {
            LogEntries.Add(new VectorizationLogEntry(
                handler.StepId, "Started handling step."));
        }

        public void LogHandlerEnd(IVectorizationStepHandler handler)
        {
            LogEntries.Add(new VectorizationLogEntry(
                handler.StepId, "Finished handling step."));
        }

        public void LogHandlerError(IVectorizationStepHandler handler, Exception ex)
        {
            LogEntries.Add(new VectorizationLogEntry(
                handler.StepId, $"ERROR: {ex.Message}"));
        }
    }
}
