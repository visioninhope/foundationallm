namespace FoundationaLLM.Authorization.Models.Configuration
{
    public record AuthorizationAPIServiceSettings
    {
        public required string APIUrl { get; set; }

        public required string APIScope { get; set; }
    }
}
