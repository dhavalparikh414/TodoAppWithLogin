using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TodoAppWithLogin.Models;
using TodoAppWithLogin.Data; 

namespace TodoAppWithLogin.Pages.Todos
{
    [Authorize] // blocks access unless logged in — redirects to Login automatically
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public IndexModel(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<TodoAppWithLogin.Models.Todos> Todos { get; set; } = new();

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Please enter a todo.")]
            [StringLength(200)]
            public string Text { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            await LoadTodosAsync();
        }

        // Add a new todo
        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadTodosAsync();
                return Page();
            }

            var userId = _userManager.GetUserId(User);

            var todo = new TodoAppWithLogin.Models.Todos
            {
                Description = Input.Text,
                IsComplete = false,
                UserId = userId!
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        // Toggle complete/incomplete
        public async Task<IActionResult> OnPostToggleAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo != null)
            {
                todo.IsComplete = !todo.IsComplete;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        // Edit todo text
        public async Task<IActionResult> OnPostEditAsync(int id, string editedText)
        {
            var userId = _userManager.GetUserId(User);
            var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo != null && !string.IsNullOrWhiteSpace(editedText))
            {
                todo.Description = editedText;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        // Delete a todo
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        private async Task LoadTodosAsync()
        {
            var userId = _userManager.GetUserId(User);
            Todos = await _context.Todos
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }
    }
}