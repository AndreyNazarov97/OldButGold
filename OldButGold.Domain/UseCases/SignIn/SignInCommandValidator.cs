using FluentValidation;
using OldButGold.Domain.Exceptions;
using OldButGold.Domain.UseCases.CreateForum;

namespace OldButGold.Domain.UseCases.SignIn
{
    internal class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(c => c.Login).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
                .MaximumLength(30).WithErrorCode(ValidationErrorCode.TooLong);

            RuleFor(c => c.Password)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        }

    }
}
