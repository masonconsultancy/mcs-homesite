using System.ComponentModel.DataAnnotations;

namespace MCS.HomeSite.Data.Models.Migration
{
    public class MigrationLogDto
    {
        [Key]
        public long Id { get; set; }
        public string TableName { get; set; }
        public string Index { get; set; }
        public string Error { get; set; }
    }
}
