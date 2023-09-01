using MCS.HomeSite.Areas.DataTables.Data.SampleData;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Areas.DataTables.Data
{
    public class McsHomeSiteContext : DbContext
    {
        public McsHomeSiteContext (DbContextOptions<McsHomeSiteContext> options)
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
            modelBuilder.Entity<Models.Users.UserDto>().HasData(SampleUserData.Data());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Models.Users.UserDto> UserDto { get; set; } = default!;
    }
}
