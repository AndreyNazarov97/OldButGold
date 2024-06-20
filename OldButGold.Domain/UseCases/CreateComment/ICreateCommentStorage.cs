using OldButGold.Forums.Domain.Models;

namespace OldButGold.Forums.Domain.UseCases.CreateComment
{
    public interface ICreateCommentStorage : IStorage
    {
        Task<Topic?> FindTopic(Guid topicId, CancellationToken cancellationToken);
        Task<Comment> CreateComment(Guid topicId, Guid userId, string text, CancellationToken cancellationToken);
    }
}
