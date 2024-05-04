namespace OldButGold.Domain.Authentication
{
    public interface IIdentity
    {
        Guid UserId { get; }
    }

    public static class IIdentityExtensions
    {
        public static bool isAuthenticated(this IIdentity identity)
        {
            return identity.UserId != Guid.Empty;
        }
    }
}
