using OldButGold.Domain.Models;

namespace OldButGold.Domain.UseCases.CreateForum
{
    public interface ICreateForumStorage
    {
        Task<Forum> CreateForum(string title, CancellationToken cancellationToken);
    }
}
