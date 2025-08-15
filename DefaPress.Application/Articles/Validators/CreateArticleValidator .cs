using DefaPress.Application.Articles.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace DefaPress.Application.Articles.Validators
{

    public class CreateArticleValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Summary).MaximumLength(2000);
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.ArticleCategoryId).GreaterThan(0);
            RuleFor(x => x.AuthorId).NotEmpty();
        }
    }
}
