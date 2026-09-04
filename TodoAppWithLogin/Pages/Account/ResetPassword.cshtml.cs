using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TodoAppWithLogin.Models;

namespace TodoAppWithLogin.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<Users> _userManager;

        public ResetPasswordModel(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string UserId { get; set; } = string.Empty;

            [Required]
            public string Code { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public IActionResult OnGet(string? userId, string? code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("A valid reset link is required.");
            }

            Input = new InputModel { UserId = userId, Code = code };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.UserId);
            if (user == null)
            {
                // Don't reveal that the user doesn't exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Input.Code));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Invalid or expired reset link.");
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, Input.Password);

            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}