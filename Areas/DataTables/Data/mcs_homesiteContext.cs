using mcs_contracts.Users;
using Microsoft.EntityFrameworkCore;

namespace mcs_homesite.Areas.DataTables.Data
{
    public class mcs_homesiteContext : DbContext
    {
        public mcs_homesiteContext (DbContextOptions<mcs_homesiteContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("HomeSite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserDto> UserDto { get; set; } = default!;
    }
}
