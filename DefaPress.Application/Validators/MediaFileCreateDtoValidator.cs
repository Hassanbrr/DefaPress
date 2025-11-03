using FluentValidation;
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Validators
{
    public class MediaFileCreateDtoValidator : AbstractValidator<MediaFileCreateDto>
    {
        public MediaFileCreateDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("فایل الزامی است.")
                .Must(file => file.Length > 0).WithMessage("فایل نمی‌تواند خالی باشد.")
                .Must(file => file.Length <= 50 * 1024 * 1024) // 50MB
                .WithMessage("حجم فایل نمی‌تواند بیشتر از ۵۰ مگابایت باشد.")
                .Must(file =>
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".avi", ".pdf", ".doc", ".docx" };
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    return allowedExtensions.Contains(extension);
                }).WithMessage("فرمت فایل مجاز نیست.");

            RuleFor(x => x.ArticleId)
                .GreaterThan(0).When(x => x.ArticleId.HasValue)
                .WithMessage("شناسه مقاله معتبر نیست.");
        }
    }
}