using DefaPress.Application.Base;
using DefaPress.Application.Profiles;
using DefaPress.Application.Validators;
using DefaPress.Infrastructure.Base;
using DefaPress.Infrastructure.Context;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DefaPress.Presentation.Web.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
       
        public static void AddCustomServicesToContainer(this IServiceCollection services, IConfiguration configuration)
        {


            services
                .AddControllersWithViews()
                .AddFluentValidation(fv =>
                {
                    // اسمبلی‌ای که Validator ها داخلش هستن رو معرفی کن
                    fv.RegisterValidatorsFromAssemblyContaining<ArticleCategoryCreateDtoValidator>();
                });
            services.AddRazorPages(); 
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))); 
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();
            services.InstallServices();
            services.InstallRepositories();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ArticleProfile>();
                cfg.AddProfile<ArticleCategoryProfile>(); // اگر پروفایل‌های دیگه داری
            });
        }

    }
}
