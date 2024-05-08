﻿using FluentValidation;
using OldButGold.Domain.Exceptions;
using OldButGold.Domain.UseCases.SignIn;

namespace OldButGold.Domain.UseCases.SignOn
{
    internal class SignOnCommandValidator : AbstractValidator<SignOnCommand>
    {
        public SignOnCommandValidator()
        {
            RuleFor(c => c.Login).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
            RuleFor(c => c.Password)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        }
    }
}
