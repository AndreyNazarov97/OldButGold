using FluentValidation;
using OldButGold.Forums.Domain.Exceptions;

namespace OldButGold.Forums.Domain.UseCases.SignOn
{
    internal class SignOnCommandValidator : AbstractValidator<SignOnCommand>
    {
        public SignOnCommandValidator()
        {
            RuleFor(c => c.Login).Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
                .MaximumLength(30).WithErrorCode(ValidationErrorCode.TooLong);
            RuleFor(c => c.Password)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        }
    }
}
