using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs when configuration validation fails.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ConfigurationValidationException"/> class.
    /// </remarks>
    /// <param name="missingConfigurations"></param>
    /// <param name="missingKeyVaultSecrets"></param>
    public class ConfigurationValidationException(IEnumerable<string?> missingConfigurations, IEnumerable<string?> missingKeyVaultSecrets)
        : Exception("FoundationaLLM configuration validation failed.")
    {
        /// <summary>
        /// The list of keys for missing or empty configurations.
        /// </summary>
        public IEnumerable<string?> MissingConfigurations { get; } = missingConfigurations;
        /// <summary>
        /// The list of missing or empty Key Vault secrets.
        /// </summary>
        public IEnumerable<string?> MissingKeyVaultSecrets { get; } = missingKeyVaultSecrets;
    }
}
