﻿using MediatR;
using OldButGold.Domain.Authentication;

namespace OldButGold.Domain.UseCases.SignOn
{
    internal class SignOnUseCase(
        IPasswordManager passwordManager,
        ISignOnStorage storage) : IRequestHandler<SignOnCommand, IIdentity>
    {
        public async Task<IIdentity> Handle(SignOnCommand command, CancellationToken cancellationToken)
        {
            var(salt, hash) = passwordManager.GeneratePasswordParts(command.Password);
            var userid = await storage.CreateUser(command.Login, salt, hash, cancellationToken);

            return new User(userid, Guid.Empty);
        }
    }
}
