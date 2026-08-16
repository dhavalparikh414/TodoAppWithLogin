using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TodoAppWithLogin.Pages
{
    public class IndexModel : PageModel
    {

        // Redirect logged-in users straight to their todos
        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToPage("/Todos/Index");
            }

            return Page();
        }

    }
}
