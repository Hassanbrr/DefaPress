using FluentValidation;
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Validators
{
    public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
    {
        public CommentCreateDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("متن نظر الزامی است.")
                .MaximumLength(2000).WithMessage("متن نظر نباید بیشتر از ۲۰۰۰ کاراکتر باشد.");

            RuleFor(x => x.ArticleId)
                .GreaterThan(0).WithMessage("مقاله معتبر نیست.");

            RuleFor(x => x.ParentCommentId)
                .GreaterThan(0).When(x => x.ParentCommentId.HasValue)
                .WithMessage("شناسه نظر والد معتبر نیست.");
        }
    }
}