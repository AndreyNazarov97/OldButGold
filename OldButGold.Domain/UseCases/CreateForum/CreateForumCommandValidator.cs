using FluentValidation;
using OldButGold.Domain.Exceptions;
using OldButGold.Domain.UseCases.CreateTopic;

namespace OldButGold.Domain.UseCases.CreateForum
{
    internal class CreateForumCommandValidator : AbstractValidator<CreateForumCommand>
    {
        public CreateForumCommandValidator()
        {
            RuleFor(c => c.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
                .MaximumLength(100).WithErrorCode(ValidationErrorCode.TooLong);
        }
    }
}
