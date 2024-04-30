using FoundationaLLM.Common.Constants.ResourceProviders;
using FoundationaLLM.Common.Exceptions;
using FoundationaLLM.Common.Models.ResourceProviders;
using FoundationaLLM.Common.Models.ResourceProviders.Model;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Model.Models
{
    /// <summary>
    /// Contains a reference to a model.
    /// </summary>
    public class ModelReference : ResourceReference
    {
        /// <summary>
        /// The object type of the model.
        /// </summary>
        [JsonIgnore]
        public Type ModelType =>
            Type switch
            {
                // Determine the types of models to include in the switch statement.
                ModelTypes.Basic => typeof(ModelBase),
                // TODO: Add additional model types here.
                _ => throw new ResourceProviderException($"The model type {Type} is not supported.")
            };
    }
}
