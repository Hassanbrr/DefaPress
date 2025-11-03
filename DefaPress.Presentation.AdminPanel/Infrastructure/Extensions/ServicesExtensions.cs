// DefaPress.Presentation.AdminPanel/Infrastructure/Extensions/ServicesExtensions.cs
using DefaPress.Application.Base;
using DefaPress.Application.Profiles;
using DefaPress.Domain;
using DefaPress.Infrastructure.Base;
using DefaPress.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DefaPress.Presentation.AdminPanel.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddCustomServicesToContainer(this IServiceCollection services, IConfiguration configuration)
        {
            // ابتدا DbContext را اضافه کنید
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // سپس Identity را اضافه کنید
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // سرویس‌های اضافی
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.InstallServices();
            services.InstallRepositories();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ArticleProfile>();
                cfg.AddProfile<ArticleCategoryProfile>();
            });

            // برای دسترسی به HttpContext در Blazor Server
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(UserManager<>));
        }
    }
}