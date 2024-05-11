using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using OldButGold.Domain.Authentication;
using OldButGold.Domain.Exceptions;

namespace OldButGold.Domain.UseCases.SignIn
{
    internal class SignInUseCase : ISignInUseCase
    {
        private readonly IValidator<SignInCommand> validator;
        private readonly ISignInStorage storage;
        private readonly IPasswordManager passwordManager;
        private readonly ISymmetricEncryptor encryptor;
        private readonly AuthenticationConfiguration configuration;

        public SignInUseCase(
            IValidator<SignInCommand> validator,
            ISignInStorage storage,
            IPasswordManager passwordManager,
            ISymmetricEncryptor encryptor,
            IOptions<AuthenticationConfiguration> options)
        {
            this.validator = validator;
            this.storage = storage;
            this.passwordManager = passwordManager;
            this.encryptor = encryptor;
            configuration = options.Value;
        }

        public async Task<(IIdentity identity, string token)> Execute(
            SignInCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command, cancellationToken);

            var recognisedUser = await storage.FindUser(command.Login, cancellationToken);
            if(recognisedUser is null)
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
