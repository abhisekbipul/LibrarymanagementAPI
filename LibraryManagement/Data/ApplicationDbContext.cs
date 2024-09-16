using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        
        public DbSet<Book> books { get; set; }
        public DbSet<LibraryCard> libraryCards { get; set; }
        public DbSet<User> users { get; set; }
    }
}
