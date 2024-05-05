using OldButGold.Domain.Authentication;

namespace OldButGold.Domain.Authorization
{
    internal class IntentionManager : IIntentionManager
    {
        private readonly IEnumerable<IIntentionResolver> resolvers;
        private readonly IIdentityProvider identityProvider;

        public IntentionManager(
            IEnumerable<IIntentionResolver> resolvers,
            IIdentityProvider identityProvider)
        {
            this.resolvers = resolvers;
            this.identityProvider = identityProvider;
        }

        public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
        {
            var mathcingResolver = resolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();
            return mathcingResolver?.isAllowed(identityProvider.Current, intention) ?? false;
        }

        public bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct
        {
            throw new NotImplementedException();
        }
    }
}
