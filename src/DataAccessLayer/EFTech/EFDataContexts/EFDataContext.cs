using DomainLayer.Entities.Color;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EFTech.EFDataContexts
{
    public partial class EFDataContext : DbContext
    {
        public DbSet<Color> Colors { get; set; }

        public EFDataContext(
            DbContextOptions<EFDataContext> options)
            : base(options)
        {
        }

        public EFDataContext(
            string connectionString) : this(
                new DbContextOptionsBuilder<EFDataContext>()
                .UseSqlServer(connectionString).Options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
