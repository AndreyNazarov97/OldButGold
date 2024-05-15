using MediatR;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Models;

namespace OldButGold.Domain.UseCases.CreateForum
{
    public class CreateForumUseCase(
        IIntentionManager intentionManager,
        ICreateForumStorage storage) : IRequestHandler<CreateForumCommand, Forum>
    {
        public async Task<Forum> Handle(CreateForumCommand command, CancellationToken cancellationToken)
        {
            intentionManager.ThrowIfForbidden(ForumIntention.Create);

            return await storage.CreateForum(command.Title, cancellationToken);
        }
    }
}
