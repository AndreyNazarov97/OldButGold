
namespace OldButGold.Domain.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationStorage storage;

        public AuthenticationService(
            IAuthenticationStorage storage)
        {
            this.storage = storage;
        }

        public async Task<(bool succes, string authToken)> SignIn(BasicSignInCredentials credentials, CancellationToken cancellationToken)
        {
            var recognisedUser = await storage.FindUser(credentials.Login, cancellationToken) ?? throw new Exception("User not found");

            var succes = credentials.Password + recognisedUser.Salt == recognisedUser.PasswordHash;
            return (succes, credentials.Login);
        }

        public Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}
