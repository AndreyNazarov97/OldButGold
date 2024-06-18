using MediatR;
using OldButGold.Forums.Domain;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Authorization;
using OldButGold.Forums.Domain.DomainEvents;
using OldButGold.Forums.Domain.UseCases.CreateComment;
using OldButGold.Forums.Domain.UseCases.GetForums;
using Topic = OldButGold.Forums.Domain.Models.Topic;

namespace OldButGold.Forums.Domain.UseCases.CreateTopic
{
    internal class CreateTopicUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        IGetForumsStorage getForumsStorage,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateTopicCommand, Topic>
    {
        public async Task<Topic> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
        {
            var (forumId, title) = command;
            intentionManager.ThrowIfForbidden(TopicIntention.Create);

            await getForumsStorage.ThrowIfFormNotExist(forumId, cancellationToken);

            await using var scope = await unitOfWork.CreateScope(cancellationToken);
            var topicStorage = scope.GetStorage<ICreateTopicStorage>();
            var domainEventStorage = scope.GetStorage<IDomainEventStorage>();


            var topic = await topicStorage
                .CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
            await domainEventStorage
                .AddEvent(ForumDomainEvent.TopicCreated(topic), cancellationToken);

            await scope.Commit(cancellationToken);

            return topic;
        }
    }
}
