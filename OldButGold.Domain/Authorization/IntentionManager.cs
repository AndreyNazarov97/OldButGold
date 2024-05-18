using OldButGold.Domain.Authentication;

namespace OldButGold.Domain.Authorization
{
    internal class IntentionManager(
        IEnumerable<IIntentionResolver> resolvers,
        IIdentityProvider identityProvider) : IIntentionManager
    {
        public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
        {
            var matchingResolver = resolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();
            return matchingResolver?.IsAllowed(identityProvider.Current, intention) ?? false;
        }
        public bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct
        {
            var matchingResolver = resolvers.OfType<IIntentionResolver<TIntention, TObject>>().FirstOrDefault();
            return matchingResolver?.IsAllowed(identityProvider.Current, intention, target) ?? false;
        }
    }
}
