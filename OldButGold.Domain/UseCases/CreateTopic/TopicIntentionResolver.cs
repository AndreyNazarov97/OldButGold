using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Authorization;

namespace OldButGold.Forums.Domain.UseCases.CreateTopic
{
    internal class TopicIntentionResolver : IIntentionResolver<TopicIntention>
    {
        public bool IsAllowed(IIdentity subject, TopicIntention intention)
        {
            return intention switch
            {
                TopicIntention.Create => subject.isAuthenticated(),
                _ => false,
            };
        }


    }
}
