using MediatR;
using OldButGold.Forums.Domain.Authorization;

namespace OldButGold.Forums.Domain.UseCases.CreateForum
{
    public class CreateForumUseCase(
        IIntentionManager intentionManager,
        ICreateForumStorage storage) : IRequestHandler<CreateForumCommand, Models.Forum>
    {
        public async Task<Models.Forum> Handle(CreateForumCommand command, CancellationToken cancellationToken)
        {
            intentionManager.ThrowIfForbidden(ForumIntention.Create);

            return await storage.CreateForum(command.Title, cancellationToken);
        }
    }
}
