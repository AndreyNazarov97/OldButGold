using FluentValidation;
using OldButGold.Forums.Domain.UseCases.CreateForum;
using OldButGold.Forums.Domain.Exceptions;

namespace OldButGold.Forums.Domain.UseCases.SignIn
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
