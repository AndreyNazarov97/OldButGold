using OldButGold.Domain.Authentication;

namespace OldButGold.Storage.Storages
{
    internal class AuthenticationStorage : IAuthenticationStorage
    {
        public Task<Session> FindSession(Guid sessionId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
