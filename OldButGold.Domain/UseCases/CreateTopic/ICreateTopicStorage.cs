using OldButGold.Domain.Models;

namespace OldButGold.Domain.UseCases.CreateTopic
{
    public interface ICreateTopicStorage : IStorage
    {
        Task<Topic> CreateTopic(Guid forumId, Guid userId, string title ,CancellationToken cancellationToken);
    }
}
