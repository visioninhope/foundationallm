using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace FoundationaLLM.Gateway.Instrumentation
{
    public class VectorizationInstrumentation : IDisposable
    {
        internal const string ActivitySourceName = "FoundationaLLM.VectorizationAPI";
        internal const string MeterName = "FoundationaLLM.VectorizationAPI.Operations";
        private readonly Meter meter;

        public VectorizationInstrumentation()
        {
            string? version = typeof(VectorizationInstrumentation).Assembly.GetName().Version?.ToString();
            this.ActivitySource = new ActivitySource(ActivitySourceName, version);

            this.meter = new Meter(MeterName, version);

            this.EmbedRequests = this.meter.CreateCounter<long>("embed.requests", description: "The number of embeddings requests");
            this.EmbedRequestsErrors = this.meter.CreateCounter<long>("embed.requests.errors", description: "The number of embeddings requests errored");

            this.PartitionRequests = this.meter.CreateCounter<long>("parition.requests", description: "The number of tokens used by embeddings requests");
            this.PartitionRequestsErrors = this.meter.CreateCounter<long>("parition.requests.errors", description: "The number of tokens used by embeddings requests");

            this.ExtractRequests = this.meter.CreateCounter<long>("extract.requests", description: "The number of tokens used by embeddings requests");
            this.ExtractRequestsErrors = this.meter.CreateCounter<long>("extract.requests.errors", description: "The number of tokens used by embeddings requests");

            this.IndexRequests = this.meter.CreateCounter<long>("index.requests", description: "The number of tokens used by embeddings requests");
            this.IndexRequestsErrors = this.meter.CreateCounter<long>("index.rqeuests.errors", description: "The number of tokens used by embeddings requests");
        }

        public ActivitySource ActivitySource { get; }

        public Counter<long> EmbedRequests { get; }
        public Counter<long> EmbedRequestsErrors { get; }

        public Counter<long> PartitionRequests { get; }
        public Counter<long> PartitionRequestsErrors { get; }

        public Counter<long> ExtractRequests { get; }
        public Counter<long> ExtractRequestsErrors { get; }

        public Counter<long> IndexRequests { get; }
        public Counter<long> IndexRequestsErrors { get; }



        public void Dispose()
        {
            this.ActivitySource.Dispose();
            this.meter.Dispose();
        }
    }
}
