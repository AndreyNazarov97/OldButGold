using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Authorization;
using OldButGold.Forums.Domain.Models;
using OldButGold.Forums.Domain.UseCases.CreateComment;

namespace OldButGold.Forums.Domain.UseCases.CreateTopic
{
    internal class TopicIntentionResolver : 
        IIntentionResolver<TopicIntention>,
        IIntentionResolver<TopicIntention, Topic>
    {
        public bool IsAllowed(IIdentity subject, TopicIntention intention)
        {
            return intention switch
            {
                TopicIntention.Create => subject.isAuthenticated(),
                _ => false,
            };
        }


        public bool IsAllowed(IIdentity subject, TopicIntention intention, Topic target)
        {
            return intention switch
            {
                TopicIntention.CreateComment => subject.isAuthenticated(),
                _ => false
            };
        }
    }
}
