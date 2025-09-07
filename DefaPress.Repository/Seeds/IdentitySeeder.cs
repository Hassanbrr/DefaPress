using DefaPress.Domain;
using Microsoft.AspNetCore.Identity;

namespace DefaPress.Infrastructure.Seeds
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // نقش‌ها
            string[] roles = { "Admin", "Editor", "Reporter", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // ادمین پیش‌فرض
            var adminEmail = "admin@defapress.ir";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "مدیر سیستم",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@12345"); // رمز پیش‌فرض

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}