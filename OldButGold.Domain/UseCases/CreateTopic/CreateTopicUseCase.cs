using MediatR;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.UseCases.CreateForum;
using OldButGold.Domain.UseCases.GetForums;
using Topic = OldButGold.Domain.Models.Topic;

namespace OldButGold.Domain.UseCases.CreateTopic
{
    internal class CreateTopicUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        IGetForumsStorage getForumsStorage,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateTopicCommand, Topic>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Topic> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
        {
            var (forumId, title) = command;
            intentionManager.ThrowIfForbidden(TopicIntention.Create);

            await getForumsStorage.ThrowIfFormNotExist(forumId, cancellationToken);

            await using var scope = await unitOfWork.CreateScope(cancellationToken);
            var topicStorage = scope.GetStorage<ICreateTopicStorage>();
            var domainEventStorage = scope.GetStorage<IDomainEventStorage>();
            var topic =  await topicStorage.CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
            await domainEventStorage.AddEvent(topic, cancellationToken);

            await scope.Commit(cancellationToken);

            return topic;
        }
    }
}
