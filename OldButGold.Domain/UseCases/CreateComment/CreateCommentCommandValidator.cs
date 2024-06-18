using FluentValidation;
using OldButGold.Forums.Domain.Exceptions;

namespace OldButGold.Forums.Domain.UseCases.CreateComment
{
    internal class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(c => c.Text)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        }
    }
}
