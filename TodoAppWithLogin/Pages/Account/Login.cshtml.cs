using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TodoAppWithLogin.Helpers;
using TodoAppWithLogin.Models; // adjust namespace to match your project

namespace TodoAppWithLogin.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;

        public LoginModel(SignInManager<Users> signInManager, UserManager<Users> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser = await _userManager.FindByNameAsync(Input.Username);
            if (existingUser == null)
            {
                ModelState.AddModelError("", "Invalid username or password. Please, try again");
                return Page();
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(existingUser);
            if (!existingUserClaims.Any(c => c.Type == CustomClaim.FirstName))
                await _userManager.AddClaimAsync(existingUser, new Claim(CustomClaim.FirstName, existingUser.FirstName));

            // lockoutOnFailure: true enables account lockout after repeated failed attempts
            var result = await _signInManager.PasswordSignInAsync(
                Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "This account has been locked out. Please try again later.");
                return Page();
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        public IActionResult OnPostExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Page("./ExternalLoginCallback", pageHandler: null, values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

    }
}