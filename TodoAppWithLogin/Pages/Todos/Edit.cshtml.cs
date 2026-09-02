using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TodoAppWithLogin.Data;
using TodoAppWithLogin.Models;

namespace TodoAppWithLogin.Pages.Todos
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public EditModel(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Please enter a todo.")]
            [StringLength(200)]
            public string Description { get; set; } = string.Empty;

            public DateTime? DueDate { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            // If the todo doesn't exist, or doesn't belong to this user, don't reveal it
            if (todo == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Id = todo.Id,
                Description = todo.Description,
                DueDate = todo.DueDate
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Id == Input.Id && t.UserId == userId);

            if (todo == null)
            {
                return NotFound();
            }

            todo.Description = Input.Description;
            todo.DueDate = Input.DueDate.HasValue
    ? DateTime.SpecifyKind(Input.DueDate.Value, DateTimeKind.Utc)
    : null;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Todos/Index");
        }
    }
}