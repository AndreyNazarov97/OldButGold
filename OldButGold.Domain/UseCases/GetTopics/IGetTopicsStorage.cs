using OldButGold.Domain.Models;

namespace OldButGold.Domain.UseCases.GetTopics
{
    public interface IGetTopicsStorage
    {
        Task<(IEnumerable<Topic> resources, int totalCount)> GetTopics(
            Guid forumId, int skip, int take, CancellationToken cancellationToken);
    }
}
