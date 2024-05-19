namespace OldButGold.Forums.Domain.Authentication
{
    public interface IIdentity
    {
        Guid UserId { get; }
        Guid SessionId { get; }
    }

    internal static class IIdentityExtensions
    {
        public static bool isAuthenticated(this IIdentity identity)
        {
            return identity.UserId != Guid.Empty;
        }
    }
}
