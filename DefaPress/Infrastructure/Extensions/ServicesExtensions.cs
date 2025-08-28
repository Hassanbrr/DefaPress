using DefaPress.Infrastructure.Context;
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
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ApplicationDbContext>();


        }

    }
}
