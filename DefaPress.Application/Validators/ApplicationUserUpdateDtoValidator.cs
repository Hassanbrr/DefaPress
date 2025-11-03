using FluentValidation;
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Validators
{
    public class ApplicationUserUpdateDtoValidator : AbstractValidator<ApplicationUserUpdateDto>
    {
        public ApplicationUserUpdateDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("نام کامل الزامی است.")
                .MaximumLength(100).WithMessage("نام کامل نباید بیشتر از ۱۰۰ کاراکتر باشد.");

            RuleFor(x => x.ProfileImageUrl)
                .MaximumLength(500).WithMessage("آدرس تصویر نباید بیشتر از ۵۰۰ کاراکتر باشد.")
                .Must(url => string.IsNullOrEmpty(url) || Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                .WithMessage("آدرس تصویر معتبر نیست.");
        }
    }
}