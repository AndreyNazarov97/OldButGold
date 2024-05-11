using OldButGold.Domain.Monitoring;
using Forum = OldButGold.Domain.Models.Forum;

namespace OldButGold.Domain.UseCases.GetForums
{
    internal class GetForumsUseCase : IGetForumsUseCase
    {
        private readonly IGetForumsStorage storage;
        private readonly DomainMetrics metrics;

        public GetForumsUseCase(
            IGetForumsStorage storage,
            DomainMetrics metrics)
        {
            this.storage = storage;
            this.metrics = metrics;
        }

        public async Task<IEnumerable<Forum>> Execute(CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<Forum> result = await storage.GetForums(cancellationToken);
                metrics.ForumsFetched(true);
                return result;
            }
            catch 
            {
                metrics.ForumsFetched(false);
                throw;
            }
        }
    }
}
