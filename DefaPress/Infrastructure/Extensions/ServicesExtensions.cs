using DefaPress.Application.Base;
using DefaPress.Application.Profiles;
using DefaPress.Domain;
using DefaPress.Infrastructure.Base;
using DefaPress.Infrastructure.Context;
using Helps;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;


namespace DefaPress.Presentation.Web.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {

        public static void AddCustomServicesToContainer(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddControllersWithViews();
       

            services.AddRazorPages();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            // ثبت سرویس‌های Identity - بدون نیاز به تایید ایمیل
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    // تنظیمات پسورد
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;

                    // تنظیمات کاربر
                    options.User.RequireUniqueEmail = false; // ایمیل اجباری نیست
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
            services.InstallServices();
            services.InstallRepositories();
            services.AddScoped<IEmailSender, EmailSender>();
            // اگر نیاز به ثبت به عنوان سرویس بود
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ArticleProfile>();
                cfg.AddProfile<ArticleCategoryProfile>(); // اگر پروفایل‌های دیگه داری
            });
        }

    }
}
