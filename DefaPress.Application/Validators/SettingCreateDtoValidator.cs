using FluentValidation;
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Validators
{
    public class SettingCreateDtoValidator : AbstractValidator<SettingCreateDto>
    {
        public SettingCreateDtoValidator()
        {
            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("دسته‌بندی تنظیمات الزامی است.")
                .MaximumLength(50).WithMessage("دسته‌بندی نباید بیشتر از ۵۰ کاراکتر باشد.");

            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("کلید تنظیمات الزامی است.")
                .MaximumLength(100).WithMessage("کلید نباید بیشتر از ۱۰۰ کاراکتر باشد.")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("کلید فقط می‌تواند شامل حروف انگلیسی، اعداد و زیرخط باشد.");

            RuleFor(x => x.Value)
                .MaximumLength(4000).WithMessage("مقدار نباید بیشتر از ۴۰۰۰ کاراکتر باشد.");
        }
    }

    public class SettingUpdateDtoValidator : AbstractValidator<SettingUpdateDto>
    {
        public SettingUpdateDtoValidator()
        {
            RuleFor(x => x.Value)
                .MaximumLength(4000).WithMessage("مقدار نباید بیشتر از ۴۰۰۰ کاراکتر باشد.");
        }
    }
}