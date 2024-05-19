using FluentValidation;
using OldButGold.Forums.Domain.Exceptions;

namespace OldButGold.Forums.Domain.UseCases.CreateForum
{
    internal class CreateForumCommandValidator : AbstractValidator<CreateForumCommand>
    {
        public CreateForumCommandValidator()
        {
            RuleFor(c => c.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
                .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong);
        }
    }
}
