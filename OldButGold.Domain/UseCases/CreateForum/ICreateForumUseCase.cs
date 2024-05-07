using OldButGold.Domain.Models;
using OldButGold.Domain.UseCases.CreateTopic;

namespace OldButGold.Domain.UseCases.CreateForum
{
    public interface ICreateForumUseCase
    {
        Task<Forum> Execute(CreateForumCommand command, CancellationToken cancellationToken);
    }
}
