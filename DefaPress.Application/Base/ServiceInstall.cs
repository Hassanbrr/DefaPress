using DefaPress.Application.Interfaces;
using DefaPress.Application.Profiles;
using DefaPress.Application.Services;
using DefaPress.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DefaPress.Application.Base
{
    public static class ServiceInstall
    {
        public static void InstallServices(this IServiceCollection services)
        {
            services.AddScoped<IArticleCategoryService, ArticleCategoryService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<ArticleCategoryCreateDtoValidator>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
} 