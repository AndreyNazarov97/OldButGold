using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Options;
using OldButGold.Forums.Domain.Authentication;
using OldButGold.Forums.Domain.Exceptions;

namespace OldButGold.Forums.Domain.UseCases.SignIn
{
    internal class SignInUseCase(
        ISignInStorage storage,
        IPasswordManager passwordManager,
        ISymmetricEncryptor encryptor,
        IOptions<AuthenticationConfiguration> options) : IRequestHandler<SignInCommand, (IIdentity identity, string token)>
    {
        private readonly AuthenticationConfiguration configuration = options.Value;

        public async Task<(IIdentity identity, string token)> Handle(
            SignInCommand command, CancellationToken cancellationToken)
        {
            var recognisedUser = await storage.FindUser(command.Login, cancellationToken);
            if (recognisedUser is null)
            {
                throw new ValidationException(new ValidationFailure[]
                {
                    new()
                    {
                        PropertyName = nameof(command.Login),
                        ErrorCode = ValidationErrorCode.Invalid,
                        AttemptedValue = command.Login
                    }
                });
            }

            var passwordMatch = passwordManager.ComparePassword(command.Password, recognisedUser.Salt, recognisedUser.PasswordHash);
            if (!passwordMatch)
            {
                throw new ValidationException(new ValidationFailure[]
                {
                    new()
                    {
                        PropertyName = nameof(command.Password),
                        ErrorCode = ValidationErrorCode.Invalid,
                        AttemptedValue = command.Password
                    }
                });
            }

            // TODO: Expiration momnent generation is ugly
            var sessionId = await storage.CreateSession(recognisedUser.UserId, DateTimeOffset.UtcNow.AddMinutes(60), cancellationToken);
            var token = await encryptor.Encrypt(sessionId.ToString(), configuration.Key, cancellationToken);

            return (new User(recognisedUser.UserId, sessionId), token);
        }
    }
}
