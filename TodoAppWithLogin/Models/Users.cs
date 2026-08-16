using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TodoAppWithLogin.Models
{
    public class Users :IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        //[BindProperty(SupportsGet = true)]
        //public string Username { get; set; }

        //public string Password { get; set; }

        //// Navigation property for related Todos
        //public ICollection<Todos>? Todos { get; set; }
    }
}
