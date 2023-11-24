using FoundationaLLM.Vectorization.Exceptions;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Vectorization.Models
{
    public class VectorizationRequest
    {
        [JsonPropertyOrder(0)]
        [JsonPropertyName("id")]
        public required string Id { get;set; }

        [JsonPropertyOrder(1)]
        [JsonPropertyName("content_id")]
        public required string ContentId { get; set; }

        [JsonPropertyOrder(10)]
        [JsonPropertyName("steps")]
        public required List<VectorizationStep> Steps { get; set; }

        [JsonPropertyOrder(11)]
        [JsonPropertyName("completed_steps")]
        public required List<string> CompletedSteps { get; set; }

        [JsonPropertyOrder(12)]
        [JsonPropertyName("remaining_steps")]
        public required List<string> RemainingSteps { get; set; }

        [JsonIgnore]
        public bool Complete => this.RemainingSteps.Count == 0;

        /// <summary>
        /// Change the vectorization pipeline to the next step, returning the name of the next step to execute.
        /// The name returned is used to choose the next request source to which the vectorization request will be added.
        /// </summary>
        public string MoveToNextStep()
        {
            if (this.RemainingSteps.Count == 0)
            {
                throw new VectorizationException("The list of remaining steps is empty");
            }

            var stepName = this.RemainingSteps.First();
            this.RemainingSteps.RemoveAt(0);
            this.CompletedSteps.Add(stepName);

            return stepName;
        }

        /// <summary>
        /// Change the vectorization pipeline to the previous step, returning the name of the step to execute
        /// </summary>
        public string RollbackToPreviousStep()
        {
            if (this.CompletedSteps.Count == 0)
            {
                throw new VectorizationException("The list of completed steps is empty");
            }

            var stepName = this.CompletedSteps.Last();
            this.CompletedSteps.RemoveAt(this.CompletedSteps.Count - 1);
            this.RemainingSteps.Insert(0, stepName);

            return stepName;
        }

        /// <summary>
        /// Gets the vectorization pipeline step that has a specific identifier.
        /// </summary>
        /// <param name="step">The identifier of the vectorization pipeline step.</param>
        /// <returns>An instances of the <see cref="VectorizationStep"/> class with the details required by the step handler.</returns>
        public VectorizationStep this[string step]
        { get { return Steps.Single(s => s.Id == step); } }
    }
}
