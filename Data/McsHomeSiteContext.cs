using MCS.HomeSite.Data.Models.Audit;
using MCS.HomeSite.Data.Models.Users;
using MCS.HomeSite.Data.SampleData;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Data
{
    public class McsHomeSiteContext : BaseDbContext<McsHomeSiteContext>
    {
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("MCSHomeSiteSource");
            //optionsBuilder.UseSqlServer("Server=localhost;Database=MCSHomeSiteSource;Trusted_Connection=True;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Users.UserDto>().HasData(SampleUserData.Data());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserDto> Users { get; set; } = default!;
        public DbSet<AuditLogDto> AuditLogs { get; set; } = default!;

        public McsHomeSiteContext(DbContextOptions<McsHomeSiteContext> options, IConfiguration config, IHttpContextAccessor accessor) : base(options, config, accessor)
        {
        }

        public McsHomeSiteContext(string connectionString, int commandTimeout, int enableRetryOnFailure, bool canAudit) : base(connectionString, commandTimeout, enableRetryOnFailure, canAudit)
        {
        }
    }
}
