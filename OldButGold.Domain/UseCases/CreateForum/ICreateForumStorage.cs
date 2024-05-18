using OldButGold.Domain.Models;

namespace OldButGold.Domain.UseCases.CreateForum
{
    public interface ICreateForumStorage : IStorage
    {
        Task<Forum> CreateForum(string title, CancellationToken cancellationToken);
    }
}
