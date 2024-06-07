using FoundationaLLM.Common.Constants.Authentication;
using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Models.Authentication
{
    /// <summary>
    /// Stores security group account information.
    /// </summary>
    public class GroupAccount : AccountBase
    {
        /// <inheritdoc/>
        [JsonPropertyName("object_type")]
        public override string ObjectType => ObjectTypes.Group;
    }
}
