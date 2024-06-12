using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Favorite>? Favorites { get; set; } = null!;
        public DbSet<FilmViewLater>? FilmViewLaters { get; set; } = null!;


        public ApplicationDbContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ticket_office.db");
        }
    }
}
