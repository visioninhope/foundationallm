using System.Text.Json.Serialization;

namespace FoundationaLLM.Utility.Upgrade.Models._050
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(MultipartPrompt050), "multipart")]
    public class Prompt050 : ResourceBase050
    {
        [JsonIgnore]
        public override string? Type { get; set; }
    }
}
