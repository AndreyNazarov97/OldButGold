using OldButGold.Domain.Authentication;
using OldButGold.Domain.Authorization;

namespace OldButGold.Domain.UseCases.CreateTopic
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
