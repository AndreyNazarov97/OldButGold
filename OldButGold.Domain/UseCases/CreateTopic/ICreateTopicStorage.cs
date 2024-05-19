using OldButGold.Forums.Domain.Models;

namespace OldButGold.Forums.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicStorage : IStorage
    {
        Task<Topic> CreateTopic(Guid forumId, Guid userId, string title, CancellationToken cancellationToken);
    }
}
