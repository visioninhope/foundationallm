using FluentValidation;
using FoundationaLLM.Common.Models.ResourceProviders.Endpoint;
using FoundationaLLM.Common.Validation.ResourceProvider;

namespace FoundationaLLM.Endpoint.Validation
{
    /// <summary>
    /// Validator for the <see cref="EndpointBase"/> model.
    /// </summary>
    public class EndpointBaseValidator : AbstractValidator<EndpointBase>
    {
        /// <summary>
        /// Configures the validation rules for the <see cref="EndpointBase"/> model.
        /// </summary>
        public EndpointBaseValidator() =>
            Include(new ResourceBaseValidator());
    }
}
