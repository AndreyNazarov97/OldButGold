using OldButGold.Forums.Domain;

namespace OldButGold.Forums.Domain.UseCases.CreateForum
{
    public interface ICreateCommentStorage : IStorage
    {
        Task<Models.Forum> CreateForum(string title, CancellationToken cancellationToken);
    }
}
