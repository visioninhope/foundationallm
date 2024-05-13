using System.Text.Json.Serialization;

namespace FoundationaLLM.Upgrade.Models._040
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(MultipartPrompt040), "multipart")]
    public class Prompt040 : ResourceBase040
    {
        [JsonIgnore]
        public override string? Type { get; set; }
    }
}
