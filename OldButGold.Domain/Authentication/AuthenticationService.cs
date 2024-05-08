
using Microsoft.Extensions.Options;

namespace OldButGold.Domain.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationConfiguration configuration;
        private readonly ISymmetricDecryptor decryptor;

        public AuthenticationService(
            ISymmetricDecryptor decryptor,
            IOptions<AuthenticationConfiguration> options)
        {
            configuration = options.Value;
            this.decryptor = decryptor;
        }

        public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
        {
            var userIdString = await decryptor.Decrypt(authToken, configuration.Key, cancellationToken);
            // TODO : verify user identifier    
            return new User(Guid.Parse(userIdString));
        }

    }
}
