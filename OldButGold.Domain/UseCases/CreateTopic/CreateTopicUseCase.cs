using FluentValidation;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Exceptions;
using Topic = OldButGold.Domain.Models.Topic;

namespace OldButGold.Domain.UseCases.CreateTopic
{
    internal class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly IValidator<CreateTopicCommand> validator;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentityProvider identityProvider;
        private readonly ICreateTopicStorage storage;

        public CreateTopicUseCase(
            IValidator<CreateTopicCommand> validator,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            ICreateTopicStorage storage)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.identityProvider = identityProvider;
            this.storage = storage;
        }
        public async Task<Topic> Execute(CreateTopicCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command, cancellationToken);

            var (forumId, title) = command;
            intentionManager.ThrowIfForbidden(TopicIntention.Create);

            var forumExist = await storage.ForumExist(forumId, cancellationToken);
            if (!forumExist)
            {
                throw new ForumNotFoundException(forumId);
            }

            return await storage.CreateTopic(forumId, identityProvider.Current.UserId, title, cancellationToken);
        }
    }
}
