using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Data.Models.Audit
{
    public class AuditLogDto
    {
        public long Id { get; set; }
        public long EntityKeyValue { get; set; }
        public long? ParentEntityKeyValue { get; set; }
        public string? EntityName { get; set; }
        public EntityState? EntityState { get; set; }
        public string? PropertyName { get; set; }
        public string? OriginalValue { get; set; }
        public string? CurrentValue { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
        public string? AdditionalData { get; set; }
    }
}
