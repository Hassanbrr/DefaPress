using FluentValidation;
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Validators
{
    public class ArticleCategoryCreateDtoValidator : AbstractValidator<ArticleCategoryCreateDto>
    {
        public ArticleCategoryCreateDtoValidator()
        {
            // Name
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام دسته‌بندی الزامی است.")
                .MaximumLength(200).WithMessage("نام دسته‌بندی نباید بیشتر از ۲۰۰ کاراکتر باشد.");

            // Slug
            RuleFor(x => x.Slug)
                .MaximumLength(250).WithMessage("آدرس دسته‌بندی نباید بیشتر از ۲۵۰ کاراکتر باشد.");

            // DisplayOrder
            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("ترتیب نمایش باید عددی بزرگتر یا مساوی ۰ باشد.");

            // ParentCategoryId
            RuleFor(x => x.ParentCategoryId)
                .Must(parentId => parentId == null || parentId > 0)
                .WithMessage("شناسه والد معتبر نیست.");
        }
    }
}