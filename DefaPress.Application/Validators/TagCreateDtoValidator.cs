using FluentValidation;
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Validators
{
    public class TagCreateDtoValidator : AbstractValidator<TagCreateDto>
    {
        public TagCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام تگ الزامی است.")
                .MaximumLength(100).WithMessage("نام تگ نباید بیشتر از ۱۰۰ کاراکتر باشد.")
                .Matches("^[a-zA-Zآ-ی0-9\\s]+$").WithMessage("نام تگ فقط می‌تواند شامل حروف و اعداد باشد.");
        }
    }

    public class TagUpdateDtoValidator : AbstractValidator<TagUpdateDto>
    {
        public TagUpdateDtoValidator()
        {
            RuleFor(x => x.TagId)
                .GreaterThan(0).WithMessage("شناسه تگ معتبر نیست.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("نام تگ الزامی است.")
                .MaximumLength(100).WithMessage("نام تگ نباید بیشتر از ۱۰۰ کاراکتر باشد.")
                .Matches("^[a-zA-Zآ-ی0-9\\s]+$").WithMessage("نام تگ فقط می‌تواند شامل حروف و اعداد باشد.");
        }
    }
}