using FluentValidation;
using DefaPress.Application.DTOs;

namespace DefaPress.Application.Validators
{
    public class ContactMessageCreateDtoValidator : AbstractValidator<ContactMessageCreateDto>
    {
        public ContactMessageCreateDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("نام کامل الزامی است.")
                .MaximumLength(100).WithMessage("نام کامل نباید بیشتر از ۱۰۰ کاراکتر باشد.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ایمیل الزامی است.")
                .EmailAddress().WithMessage("فرمت ایمیل معتبر نیست.")
                .MaximumLength(150).WithMessage("ایمیل نباید بیشتر از ۱۵۰ کاراکتر باشد.");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("شماره تلفن نباید بیشتر از ۲۰ کاراکتر باشد.")
                .Matches(@"^[\d\s\-\+\(\)]+$").When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage("فرمت شماره تلفن معتبر نیست.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("متن پیام الزامی است.")
                .MaximumLength(5000).WithMessage("متن پیام نباید بیشتر از ۵۰۰۰ کاراکتر باشد.");
        }
    }
}