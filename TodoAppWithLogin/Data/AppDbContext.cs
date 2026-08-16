using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoAppWithLogin.Models;

namespace TodoAppWithLogin.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
     : base(options)
        {

        }


        public DbSet<Todos> Todos { get; set; }

    }
}
