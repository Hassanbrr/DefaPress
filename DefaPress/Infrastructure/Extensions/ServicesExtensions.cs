using DefaPress.Application.Base;
using DefaPress.Application.Profiles; 
using DefaPress.Infrastructure.Base;
using DefaPress.Infrastructure.Context;
 
using Microsoft.AspNetCore.Identity;
 
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
            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
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
