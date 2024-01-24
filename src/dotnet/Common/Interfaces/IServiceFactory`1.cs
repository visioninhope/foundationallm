using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Interfaces
{
    /// <summary>
    /// Creates typed service instances.
    /// </summary>
    public interface IServiceFactory<T>

    {
        /// <summary>
        /// Creates a service instance of type T specified by name.
        /// </summary>
        /// <param name="serviceName">The name of the service instance to create.</param>
        /// <returns>The service instance created by name.</returns>
        T CreateService(string serviceName);
    }
}
