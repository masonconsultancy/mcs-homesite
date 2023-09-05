using MCS.HomeSite.Data.Models.Audit;

namespace MCS.HomeSite.Data.Models
{
    public class BaseResponse
    {
        public int StateEntriesWritten { get; set; }
        public IEnumerable<AuditLogDto> AuditLogs { get; set; }
    }
}
