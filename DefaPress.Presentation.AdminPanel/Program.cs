using DefaPress.Presentation.AdminPanel.Components;
using DefaPress.Presentation.AdminPanel.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using DefaPress.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DefaPress.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCustomServicesToContainer(builder.Configuration);

// پیکربندی احراز هویت
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddCookie(IdentityConstants.ApplicationScheme, options =>
{
    options.LoginPath = "/admin/login";
    options.AccessDeniedPath = "/admin/access-denied";
    options.Cookie.Name = "DefaPress.Admin.Auth";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// پیکربندی Authorization
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Administrator"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// ایجاد نقش و کاربر ادمین
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // اعمال migration‌های pending
        await context.Database.MigrateAsync();

        // ایجاد نقش ادمین
        if (!await roleManager.RoleExistsAsync("Administrator"))
        {
            await roleManager.CreateAsync(new IdentityRole("Administrator"));
            Console.WriteLine("✅ نقش Administrator ایجاد شد");
        }

        // ایجاد نقش‌های دیگر اگر نیاز باشد
        var roles = new[] { "Editor", "Author", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"✅ نقش {role} ایجاد شد");
            }
        }

        // ایجاد کاربر ادمین
        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@defapress.ir",
                FullName = "مدیر سیستم",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrator");
                Console.WriteLine("✅ کاربر ادمین ایجاد شد");
                Console.WriteLine("📧 ایمیل: admin@defapress.ir");
                Console.WriteLine("🔑 رمز عبور: Admin123!");
            }
            else
            {
                Console.WriteLine("❌ خطا در ایجاد کاربر ادمین:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine("ℹ️ کاربر ادمین از قبل وجود دارد");
        }

        // ایجاد یک کاربر تستی
        var testUser = await userManager.FindByNameAsync("editor");
        if (testUser == null)
        {
            testUser = new ApplicationUser
            {
                UserName = "editor",
                Email = "editor@defapress.ir",
                FullName = "ویرایشگر تست",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(testUser, "Editor123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(testUser, "Editor");
                Console.WriteLine("✅ کاربر ویرایشگر ایجاد شد");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ خطا در ایجاد نقش/کاربر: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"📋 Inner Exception: {ex.InnerException.Message}");
        }
    }
}

// مسیرهای Blazor
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Fallback برای پنل ادمین
app.MapFallbackToPage("/admin/{*path:nonfile}", "/_Host");

// اضافه کردن مسیرهای Identity
app.MapGet("/admin/login", async (HttpContext context, string? returnUrl) =>
{
    returnUrl ??= "/admin";
    if (context.User.Identity?.IsAuthenticated == true)
    {
        context.Response.Redirect(returnUrl);
        return;
    }

    context.Response.Redirect($"/admin/login?returnUrl={returnUrl}");
});

app.MapPost("/admin/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Redirect("/admin/login");
});

app.Run();