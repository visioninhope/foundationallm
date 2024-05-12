using FoundationaLLM.Common.Models.Gateway;

namespace FoundationaLLM.Common.Instrumentation
{
    public class SingleCount
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
    public class SlidingWindowRateLimiter
    {
        private readonly string name;
        private readonly int capacity;
        private readonly int windowInSeconds;
        private readonly Queue<SingleCount> window;
        private readonly ModelContext parent;

        public SlidingWindowRateLimiter(int capacity, int windowInSeconds, string name, ModelContext parent)
        {
            this.name = name;
            this.capacity = capacity;
            this.windowInSeconds = windowInSeconds;
            this.window = new Queue<SingleCount>();
            this.parent = parent;
        }

        public bool TryConsume(int val)
        {
            SingleCount sc = new SingleCount { Date = DateTime.Now, Count = val };

            lock (window)
            {
                CleanExpiredTokens();

                if (Current() < capacity)
                {
                    window.Enqueue(sc);
                    return true;
                }

                return false;
            }
        }

        public int Current()
        {
            lock (window)
            {
                CleanExpiredTokens();

                int total = 0;

                foreach(SingleCount cs in window)
                {
                    total+= cs.Count;
                }

                Console.WriteLine($"{name} : Current total: {total}");

                return total;
            }
        }

        private void CleanExpiredTokens()
        {
            var now = DateTime.Now;

            while (window.Count > 0 && (now - window.Peek().Date).TotalSeconds >= this.windowInSeconds)
            {
                string log = $"Removing token with date {window.Peek().Date} and count {window.Peek().Count}";

                window.Dequeue();
            }
        }
    }
}
