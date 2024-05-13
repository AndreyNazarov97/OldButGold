using FluentValidation;
using MediatR;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Monitoring;
using OldButGold.Domain.UseCases.GetForums;
using Topic = OldButGold.Domain.Models.Topic;

namespace OldButGold.Domain.UseCases.CreateTopic
{
    internal class CreateTopicUseCase : IRequestHandler<CreateTopicCommand, Topic>
    {
        private readonly IValidator<CreateTopicCommand> validator;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentityProvider identityProvider;
        private readonly IGetForumsStorage getForumsStorage;
        private readonly ICreateTopicStorage storage;

        public CreateTopicUseCase(
            IValidator<CreateTopicCommand> validator,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            IGetForumsStorage getForumsStorage,
            ICreateTopicStorage storage)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.identityProvider = identityProvider;
            this.getForumsStorage = getForumsStorage;
            this.storage = storage;
        }

        public async Task<Topic> Handle(CreateTopicCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command, cancellationToken);

            var (forumId, title) = command;
            intentionManager.ThrowIfForbidden(TopicIntention.Create);

            await getForumsStorage.ThrowIfFormNotExist(forumId, cancellationToken);

            return await storage.CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
        }
    }
}
