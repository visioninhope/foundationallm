using System.Text.Json.Serialization;

namespace FoundationaLLM.Common.Upgrade.Models._060
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(MultipartPrompt060), "multipart")]
    public class Prompt060 : ResourceBase060
    {
        [JsonIgnore]
        public override string? Type { get; set; }
    }
}
