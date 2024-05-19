namespace OldButGold.Forums.Domain.Authentication
{
    public class User(Guid userId, Guid sessionId) : IIdentity
    {
        public Guid UserId { get; } = userId;

        public static User Guest => new(Guid.Empty, Guid.Empty);

        public Guid SessionId { get; } = sessionId;
    }
}
