using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TodoAppWithLogin.Models;

namespace TodoAppWithLogin.Pages.Account
{
    public class ExternalLoginCallbackModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;

        public ExternalLoginCallbackModel(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login");
            }

            // Step 1: Try signing in directly if this external login is already linked to a user
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ErrorMessage = "This account has been locked out. Please try again later.";
                return RedirectToPage("./Login");
            }

            // Step 2: Not linked yet — read the email Google gave us
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                ErrorMessage = "Google did not provide an email address, so we can't sign you in.";
                return RedirectToPage("./Login");
            }

            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                // Step 3a: A user already exists with this email — link Google to that account
                var addLoginResult = await _userManager.AddLoginAsync(existingUser, info);
                if (!addLoginResult.Succeeded)
                {
                    ErrorMessage = "Could not link your Google account. Please try logging in with your password instead.";
                    return RedirectToPage("./Login");
                }

                await _signInManager.SignInAsync(existingUser, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            // Step 3b: No existing user — create one automatically from Google's profile info
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "New";
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "User";

            var newUser = new Users
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true // Google already verified this email on their end
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                ErrorMessage = "Could not create an account with your Google sign-in.";
                return RedirectToPage("./Login");
            }

            var linkResult = await _userManager.AddLoginAsync(newUser, info);
            if (!linkResult.Succeeded)
            {
                ErrorMessage = "Account created, but linking Google sign-in failed. Please try logging in with your password.";
                return RedirectToPage("./Login");
            }

            await _signInManager.SignInAsync(newUser, isPersistent: false);
            return LocalRedirect(returnUrl);
        }
    }
}