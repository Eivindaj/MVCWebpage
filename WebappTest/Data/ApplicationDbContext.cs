using Microsoft.EntityFrameworkCore;
using WebappTest.Models;

namespace WebappTest.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Oppgaver> oppgavers { get; set; }
        public DbSet<Stilling> stilling { get; set; }
        public DbSet<Ansatt> ansatts { get; set; }

    }
}
