// DefaPress.Presentation.AdminPanel/Infrastructure/Extensions/IdentityExtensions.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DefaPress.Domain;

namespace DefaPress.Presentation.AdminPanel.Infrastructure.Extensions
{
    public static class IdentityExtensions
    {
        public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var routeGroup = endpoints.MapGroup("/admin");

            routeGroup.MapGet("/login", async (
                [FromQuery] string? returnUrl,
                HttpContext context,
                [FromServices] SignInManager<ApplicationUser> signInManager) =>
            {
                var redirectUrl = returnUrl ?? "/admin";
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    return Results.Redirect(redirectUrl);
                }

                return Results.Redirect($"/admin/login?returnUrl={redirectUrl}");
            });

            routeGroup.MapPost("/logout", async (
                HttpContext context,
                [FromServices] SignInManager<ApplicationUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Redirect("/admin/login");
            });

            return routeGroup;
        }
    }
}