using FluentValidation;
using MediatR;
using OldButGold.Domain.Authentication;

namespace OldButGold.Domain.UseCases.SignOn
{
    internal class SignOnUseCase : IRequestHandler<SignOnCommand, IIdentity>
    {
        private readonly IValidator<SignOnCommand> validator;
        private readonly IPasswordManager passwordManager;
        private readonly ISignOnStorage storage;

        public SignOnUseCase(
            IValidator<SignOnCommand> validator,
            IPasswordManager passwordManager,
            ISignOnStorage storage)
        {
            this.validator = validator;
            this.passwordManager = passwordManager;
            this.storage = storage;
        }
        public async Task<IIdentity> Handle(SignOnCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command, cancellationToken);

            var(salt, hash) = passwordManager.GeneratePasswordParts(command.Password);
            var userid = await storage.CreateUser(command.Login, salt, hash, cancellationToken);

            return new User(userid, Guid.Empty);
        }
    }
}
