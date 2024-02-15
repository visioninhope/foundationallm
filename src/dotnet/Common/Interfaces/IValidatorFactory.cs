using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Provides methods for getting validators.
    /// </summary>
    public interface IValidatorFactory
    {
        /// <summary>
        /// Gets a validator for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object being validated.</typeparam>
        /// <returns></returns>
        IValidator<T>? GetValidator<T>() where T : class;
    }
}
