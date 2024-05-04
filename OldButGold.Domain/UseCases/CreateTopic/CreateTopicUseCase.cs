using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;
using OldButGold.Domain.Exceptions;
using Topic = OldButGold.Domain.Models.Topic;

namespace OldButGold.Domain.UseCases.CreateTopic
{
    public class CreateTopicUseCase : ICreateTopicUseCase
    {
        private readonly IIntentionManager intentionManager;
        private readonly IIdentityProvider identityProvider;
        private readonly ICreateTopicStorage storage;

        public CreateTopicUseCase(
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            ICreateTopicStorage storage)
        {
            this.intentionManager = intentionManager;
            this.identityProvider = identityProvider;
            this.storage = storage;
        }
        public async Task<Topic> Execute(Guid forumId, string title, CancellationToken cancellationToken)
        {
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
