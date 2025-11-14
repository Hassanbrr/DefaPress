using DefaPress.Application.Profiles;
using DefaPress.Application.Services.Implements;
using DefaPress.Application.Services.Interfaces;
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
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPollService, PollService>();

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<ArticleCategoryCreateDtoValidator>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
} 