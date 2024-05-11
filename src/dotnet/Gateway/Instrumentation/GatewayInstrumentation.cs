using FoundationaLLM.Gateway.Models;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace FoundationaLLM.Gateway.Instrumentation
{
    public class GatewayInstrumentation : IDisposable
    {
        public const string ActivitySourceName = "FoundationaLLM.GatewayAPI";
        public const string MeterName = "FoundationaLLM.GatewayAPI.AzureOpenAI";
        public Meter Meter { get; }

        public Dictionary<string, EmbeddingModelContext> EmbeddingModels { get; set; }


        public GatewayInstrumentation()
        {
            string? version = typeof(GatewayInstrumentation).Assembly.GetName().Version?.ToString();
            this.ActivitySource = new ActivitySource(ActivitySourceName, version);

            this.Meter = new Meter(MeterName, version);

            this.EmbeddingsRequests = this.Meter.CreateCounter<double>("embeddings.requests", description: "The number of embeddings requests");

            this.EmbeddingsTokens = this.Meter.CreateCounter<double>("embeddings.tokens", description: "The number of tokens used by embeddings requests");

            this.EmbeddingModels = new Dictionary<string, EmbeddingModelContext>();
        }

        public void AddEmbeddingModel(EmbeddingModelContext em)
        {
            this.EmbeddingModels.Add(em.ModelName, em);

            this.Meter.CreateCounter<long>($"embeddings.requests.{em.ModelName}", description: "The number of embeddings requests");

            this.Meter.CreateCounter<long>($"embeddings.tokens.{em.ModelName}", description: "The number of tokens used by embeddings requests");

            this.Meter.CreateObservableCounter<long>("embeddings.models.requests", () => GatewayInstrumentation.GetEmbeddingRequests(em), "rpm", "Requests per minute");

            this.Meter.CreateObservableCounter<long>("embeddings.models.tokens", () => GatewayInstrumentation.GetTokens(em), "tpm", "Tokens per minute");
        }

        public static IEnumerable<Measurement<long>> GetEmbeddingRequests(EmbeddingModelContext em)
        {
            long val = em.RequestCount.Current();
            yield return new(val, new("ModelName", em.ModelName), new("ModelName", em.ModelName));
        }

        public static IEnumerable<Measurement<long>> GetTokens(EmbeddingModelContext em)
        {
            long val = em.TokenCount.Current();
            yield return new(val, new("ModelName", em.ModelName), new("ModelName", em.ModelName));
        }

        public ActivitySource ActivitySource { get; }

        public Counter<double> EmbeddingsRequests { get; }

        public Counter<double> CompletiongRequests { get; }


        public Counter<double> EmbeddingsTokens { get; }

        public Counter<double> CompletionTokens { get; }

        public void Dispose()
        {
            this.ActivitySource.Dispose();
            this.Meter.Dispose();
        }
    }
}
