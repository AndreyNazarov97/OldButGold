
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace OldButGold.Domain.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationConfiguration configuration;
        private readonly ISymmetricDecryptor decryptor;
        private readonly IAuthenticationStorage storage;
        private readonly ILogger<AuthenticationService> logger;

        public AuthenticationService(
            ISymmetricDecryptor decryptor,
            IAuthenticationStorage storage,
            ILogger<AuthenticationService> logger, 
            IOptions<AuthenticationConfiguration> options)
        {
            configuration = options.Value;
            this.decryptor = decryptor;
            this.storage = storage;
            this.logger = logger;
        }

        public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
        {
            string sessionIdString;

            try
            {
                sessionIdString = await decryptor.Decrypt(authToken, configuration.Key, cancellationToken);
               
            }
            catch (CryptographicException cryptographicException)
            {
                logger.LogWarning(cryptographicException, "Cannot decrypt auth token: {AuthToken}", authToken);
                return User.Guest;
            }

            if(!Guid.TryParse(sessionIdString, out var sessionId))
            {
                return User.Guest;
            }

            var session = await storage.FindSession(sessionId, cancellationToken);
            if(session is null)
            {
                return User.Guest;
            }

            if(session.ExpiresAt < DateTimeOffset.UtcNow)
            {
                return User.Guest;
            }
   
            return new User(session.UserId, sessionId);
        }

        
    }
}
