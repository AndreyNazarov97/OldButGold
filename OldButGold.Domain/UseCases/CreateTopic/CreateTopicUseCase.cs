using MediatR;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.UseCases.GetForums;
using Topic = OldButGold.Domain.Models.Topic;

namespace OldButGold.Domain.UseCases.CreateTopic
{
    internal class CreateTopicUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        IGetForumsStorage getForumsStorage,
        ICreateTopicStorage storage) : IRequestHandler<CreateTopicCommand, Topic>
    {
        public async Task<Topic> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
        {
            var (forumId, title) = command;
            intentionManager.ThrowIfForbidden(TopicIntention.Create);

            await getForumsStorage.ThrowIfFormNotExist(forumId, cancellationToken);

            return await storage.CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
        }
    }
}
