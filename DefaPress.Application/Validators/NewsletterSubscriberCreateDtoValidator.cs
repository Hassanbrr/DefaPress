using FluentValidation;
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Validators
{
    public class NewsletterSubscriberCreateDtoValidator : AbstractValidator<NewsletterSubscriberCreateDto>
    {
        public NewsletterSubscriberCreateDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ایمیل الزامی است.")
                .EmailAddress().WithMessage("فرمت ایمیل معتبر نیست.")
                .MaximumLength(150).WithMessage("ایمیل نباید بیشتر از ۱۵۰ کاراکتر باشد.");
        }
    }
}