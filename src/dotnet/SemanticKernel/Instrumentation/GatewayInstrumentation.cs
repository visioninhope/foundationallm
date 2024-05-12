using FoundationaLLM.Common.Models.Gateway;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace FoundationaLLM.Gateway.Instrumentation
{
    public class GatewayInstrumentation : IDisposable
    {
        public const string ActivitySourceName = "FoundationaLLM.GatewayAPI";
        public const string MeterName = "FoundationaLLM.GatewayAPI.AzureOpenAI";
        public Meter Meter { get; }

        public Dictionary<string, CompletionModelContext> CompletionModels { get; set; }
        public Dictionary<string, EmbeddingModelContext> EmbeddingModels { get; set; }

        public ActivitySource ActivitySource { get; }

        public Counter<double> GatewayEmbeddingsRequests { get; }
        public Counter<double> GatewayCompletionRequests { get; }

        public Counter<double> GatewayEmbeddingsTokens { get; }
        public Counter<double> GatewayCompletionTokens { get; }

        public Dictionary<string, Counter<double>> EmbeddingsRequests { get; }
        public Dictionary<string, Counter<double>> CompletionRequests { get; }
        public Dictionary<string, Counter<double>> EmbeddingsTokens { get; }
        public Dictionary<string, Counter<double>> CompletionTokens { get; }


        public GatewayInstrumentation()
        {
            string? version = typeof(GatewayInstrumentation).Assembly.GetName().Version?.ToString();
            this.ActivitySource = new ActivitySource(ActivitySourceName, version);

            this.Meter = new Meter(MeterName, version);

            this.GatewayEmbeddingsRequests = this.Meter.CreateCounter<double>("embeddings.requests", description: "The number of embeddings requests");
            this.GatewayEmbeddingsTokens = this.Meter.CreateCounter<double>("embeddings.tokens", description: "The number of tokens used by embeddings requests");

            this.GatewayCompletionRequests = this.Meter.CreateCounter<double>("completions.requests", description: "The number of completion requests");
            this.GatewayCompletionTokens = this.Meter.CreateCounter<double>("completions.tokens", description: "The number of tokens used by completion requests");

            this.CompletionModels = new Dictionary<string, CompletionModelContext>();
            this.EmbeddingModels = new Dictionary<string, EmbeddingModelContext>();

            this.CompletionRequests = new Dictionary<string, Counter<double>>();
            this.CompletionTokens = new Dictionary<string, Counter<double>>();

            this.EmbeddingsRequests = new Dictionary<string, Counter<double>>();
            this.EmbeddingsTokens = new Dictionary<string, Counter<double>>();  
        }

        public void AddEmbeddingModel(EmbeddingModelContext em)
        {
            if (this.EmbeddingModels.ContainsKey(em.ModelName))
                this.EmbeddingModels.Remove(em.ModelName);

            this.EmbeddingModels.Add(em.ModelName, em);

            this.EmbeddingsRequests.Add(em.ModelName, this.Meter.CreateCounter<double>($"embeddings.requests.{em.ModelName}", description: "The number of embeddings requests"));

            this.EmbeddingsTokens.Add(em.ModelName, this.Meter.CreateCounter<double>($"embeddings.tokens.{em.ModelName}", description: "The number of tokens used by embeddings requests"));

            //this.Meter.CreateObservableCounter<long>("embeddings.models.requests", () => GatewayInstrumentation.GetRequests(em), "rpm", "Requests per minute");
            //this.Meter.CreateObservableCounter<long>("embeddings.models.tokens", () => GatewayInstrumentation.GetTokens(em), "tpm", "Tokens per minute");
        }

        public void AddCompletionModel(CompletionModelContext em)
        {
            if ( this.CompletionModels.ContainsKey(em.ModelName) )
                this.CompletionModels.Remove(em.ModelName);

            this.CompletionModels.Add(em.ModelName, em);

            this.CompletionRequests.Add(em.ModelName, this.Meter.CreateCounter<double>($"completions.requests.{em.ModelName}", description: "The number of completion requests"));

            this.CompletionTokens.Add(em.ModelName, this.Meter.CreateCounter<double>($"completions.tokens.{em.ModelName}", description: "The number of tokens used by completion requests"));

            //this.Meter.CreateObservableCounter<long>("embeddings.models.requests", () => GatewayInstrumentation.GetRequests(em), "rpm", "Requests per minute");
            //this.Meter.CreateObservableCounter<long>("embeddings.models.tokens", () => GatewayInstrumentation.GetTokens(em), "tpm", "Tokens per minute");
        }

        public static IEnumerable<Measurement<long>> GetRequests(ModelContext em)
        {
            long val = em.RequestCount.Current();
            yield return new(val, new("ModelName", em.ModelName), new("ModelName", em.ModelName));
        }

        public static IEnumerable<Measurement<long>> GetTokens(ModelContext em)
        {
            long val = em.TokenCount.Current();
            yield return new(val, new("ModelName", em.ModelName), new("ModelName", em.ModelName));
        }

        public void Dispose()
        {
            this.ActivitySource.Dispose();
            this.Meter.Dispose();
        }
    }
}
