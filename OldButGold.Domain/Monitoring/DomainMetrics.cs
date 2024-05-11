using System.Collections.Concurrent;
using System.Diagnostics.Metrics;

namespace OldButGold.Domain.Monitoring
{
    internal class DomainMetrics
    {
        private readonly Meter meter = new Meter("OldButGold.Domain");
        private readonly ConcurrentDictionary<string, Counter<int>> counters = new();


        public void ForumsFetched(bool success)
        {
            IncrementCount("forums.fetched", 1, new Dictionary<string, object?>
            {
                ["succes"] = success,
            });
        }

        private void IncrementCount(string name, int value, IDictionary<string, object?>? additionalTags = null)
        {
            var counter = counters.GetOrAdd(name, _ => meter.CreateCounter<int>(name));
            counter.Add(value, additionalTags?.ToArray() ?? ReadOnlySpan<KeyValuePair<string, object?>>.Empty);
        }
    }
}
