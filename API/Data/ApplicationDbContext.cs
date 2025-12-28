using Microsoft.EntityFrameworkCore;

namespace Trabalho_ISI.Data
{
    /// <summary>
    /// Application database context.
    /// Manages access to the database and entity sets.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Constructor that receives database context options
        /// via dependency injection.
        /// </summary>
        /// <param name="options">Database context configuration</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Users table.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Movies table.
        /// </summary>
        public DbSet<Movie> Movies { get; set; }

        /// <summary>
        /// Books table.
        /// </summary>
        public DbSet<Book> Books { get; set; }
    }
}
