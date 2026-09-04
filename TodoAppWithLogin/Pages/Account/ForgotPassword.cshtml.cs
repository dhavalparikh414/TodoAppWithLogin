using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TodoAppWithLogin.Models;
using TodoAppWithLogin.Services;

namespace TodoAppWithLogin.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Users> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<Users> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);

            // Deliberately don't reveal whether the email exists — always show the same confirmation
            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { userId = user.Id, code = encodedToken },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset your TodoApp password",
                    $"<p>We received a request to reset your TodoApp password.</p>" +
                    $"<p><a href='{callbackUrl}'>Click here to reset your password</a></p>" +
                    $"<p>If you didn't request this, you can safely ignore this email.</p>");
            }

            return RedirectToPage("./ForgotPasswordConfirmation");
        }
    }
}