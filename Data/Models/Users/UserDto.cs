using System.ComponentModel.DataAnnotations;
using MCS.HomeSite.Common;
using MCS.HomeSite.Data.CustomValidators;

namespace MCS.HomeSite.Data.Models.Users
{
    [Auditing(true,fields:"Id,Name,Email,UserName")]
    public class UserDto
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, ValidateEmail]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
