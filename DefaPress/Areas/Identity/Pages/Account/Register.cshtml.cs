// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using DefaPress.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Helps;

namespace DefaPress.Presentation.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "نام کامل الزامی است.")]
            [Display(Name = "نام کامل")]
            [StringLength(100, ErrorMessage = "نام کامل باید بین 2 تا 100 کاراکتر باشد.", MinimumLength = 2)]
            public string FullName { get; set; }

            [Required(ErrorMessage = "شماره تلفن الزامی است.")]
            [Display(Name = "شماره تلفن")]
            [Phone(ErrorMessage = "شماره تلفن معتبر نیست.")]
            [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "فرمت شماره تلفن باید 09xxxxxxxxx باشد.")]
            public string PhoneNumber { get; set; }
            public string Role { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }

            [Required(ErrorMessage = "رمز عبور الزامی است.")]
            [StringLength(100, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "رمز عبور")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تکرار رمز عبور")]
            [Compare("Password", ErrorMessage = "رمز عبور و تکرار آن مطابقت ندارند.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            // ایجاد نقش‌ها در صورت عدم وجود
            await CreateRolesIfNotExist();
            Input = new InputModel
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // بررسی تکراری نبودن شماره تلفن
                var existingUser = await _userManager.FindByNameAsync(Input.PhoneNumber);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "شماره تلفن قبلاً ثبت شده است.");
                    return Page();
                }

                var user = CreateUser();

                user.FullName = Input.FullName;
                user.PhoneNumber = Input.PhoneNumber;
                user.UserName = Input.PhoneNumber; // Username = PhoneNumber
                user.Email = null; // ایمیل را null می‌کنیم
                user.EmailConfirmed = true; // چون ایمیل نداریم، confirmed در نظر می‌گیریم
                user.CreatedAt = DateTime.UtcNow;

                await _userStore.SetUserNameAsync(user, Input.PhoneNumber, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // اختصاص نقش پیش‌فرض User
                    if (!string.IsNullOrEmpty(Input.Role))
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, RoleConstants.User);

                    }
                    // لاگین مستقیم کاربر بعد از ثبت‌نام
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private async Task CreateRolesIfNotExist()
        {
            string[] roles = { "SuperAdmin", "Admin", "Editor", "Author", "Reporter", "Moderator", "User" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}