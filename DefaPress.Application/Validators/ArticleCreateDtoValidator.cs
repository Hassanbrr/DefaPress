using FluentValidation;
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Validators
{
    public class ArticleCreateDtoValidator : AbstractValidator<ArticleCreateDto>
    {
        public ArticleCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان مقاله الزامی است.")
                .MaximumLength(500).WithMessage("عنوان مقاله نباید بیشتر از ۵۰۰ کاراکتر باشد.");

 
            RuleFor(x => x.Summary)
                .MaximumLength(1000).WithMessage("خلاصه مقاله نباید بیشتر از ۱۰۰۰ کاراکتر باشد.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("محتوای مقاله الزامی است.");

            RuleFor(x => x.ArticleCategoryId)
                .GreaterThan(0).WithMessage("دسته‌بندی مقاله الزامی است.");

            RuleFor(x => x.PublishedAt)
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddMinutes(-5))
                .When(x => x.PublishedAt.HasValue)
                .WithMessage("تاریخ انتشار نمی‌تواند در گذشته باشد.");
        }
    }

    public class ArticleUpdateDtoValidator : AbstractValidator<ArticleUpdateDto>
    {
        public ArticleUpdateDtoValidator()
        {
            Include(new ArticleCreateDtoValidator());

            RuleFor(x => x.ArticleId)
                .GreaterThan(0).WithMessage("شناسه مقاله معتبر نیست.");
        }
    }
}