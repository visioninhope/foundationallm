using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using IValidatorFactory = FoundationaLLM.Common.Interfaces.IValidatorFactory;

namespace FoundationaLLM.Common.Validation
{
    /// <inheritdoc/>
    public class ValidatorFactory(IServiceProvider serviceProvider) : IValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        /// <inheritdoc/>
        public IValidator<T>? GetValidator<T>() where T : class =>
            _serviceProvider.GetService<IValidator<T>>();

        /// <inheritdoc/>
        public object? GetValidator(Type type)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(type);
            return _serviceProvider.GetService(validatorType);
        }
    }
}
