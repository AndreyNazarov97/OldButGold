using System.Diagnostics;

namespace OldButGold.Search.ForumConsumer.Monitoring
{
    public class Metrics
    {
        public const string ApplicationName = "TFA.Search.ForumConsumer";
        internal static readonly ActivitySource ActivitySource = new(ApplicationName);
    }
}
