using MCS.HomeSite.Data.Models.Audit;
using MCS.HomeSite.Data.Models.Migration;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Data
{
    public class McsHomeSiteDestContext : BaseDbContext<McsHomeSiteDestContext>
    {
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=localhost;Database=MCSHomeSite;Trusted_Connection=True;TrustServerCertificate=True");
            optionsBuilder.UseInMemoryDatabase("MCSHomeSite");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Models.Users.UserDto> Users { get; set; } = default!;
        public DbSet<MigrationLogDto> MigrationLogs { get; set; } = default!;
        public DbSet<AuditLogDto> AuditLogs { get; set; } = default!;

        public McsHomeSiteDestContext(DbContextOptions<McsHomeSiteDestContext> options, IConfiguration config, IHttpContextAccessor accessor) : base(options, config, accessor)
        {
        }

        public McsHomeSiteDestContext(string connectionString, int commandTimeout, int enableRetryOnFailure, bool canAudit) : base(connectionString, commandTimeout, enableRetryOnFailure, canAudit)
        {
        }
    }
}
