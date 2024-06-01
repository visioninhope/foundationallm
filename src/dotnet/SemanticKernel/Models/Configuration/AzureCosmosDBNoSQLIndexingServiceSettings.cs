using FoundationaLLM.Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoundationaLLM.SemanticKernel.Core.Models.Configuration
{
    /// <summary>
    /// Provides configuration settings for the Azure Cosmos DB NoSQL vector indexing service.
    /// </summary>
    public record AzureCosmosDBNoSQLIndexingServiceSettings
    {
        /// <summary>
        /// The connection string for the Azure Cosmos DB workspace.
        /// Please note, even though the default connection to the deployed Cosmos DB workspace
        /// uses RBAC, the connection string is required to use the vectorization capabilities
        /// due to the access level required to create workspace artifacts like containers.
        /// </summary>
        public required string ConnectionString { get; set; }

        /// <summary>
        /// The name of the Azure Cosmos DB vector database.
        /// Please note, this database should be different from the default one deployed with
        /// the FoundationaLLM platform, as it is used for vectorization purposes.
        /// </summary>
        public string? VectorDatabase { get; set; }

        /// <summary>
        /// Sets the maximum authoscale throughput for new containers automatically created
        /// for this vector database. The default value is 4000 RU/s.
        /// </summary>
        public string AutoscaleMaxThroughput { get; set; } = "4000";
    }
}
