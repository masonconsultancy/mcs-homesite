using System.Text.Json.Nodes;
using mcs_homesite.Areas.Models.Users;
using Microsoft.EntityFrameworkCore;
using UserDto = mcs_homesite.Areas.Models.Users.UserDto;

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
            var userData = Extensions.Extensions.ReadAsync<UserDto[]>("users.json").ConfigureAwait(false).GetAwaiter().GetResult();
            modelBuilder.Entity<UserDto>().HasData(userData ?? Array.Empty<UserDto>());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserDto> UserDto { get; set; } = default!;
    }
}
