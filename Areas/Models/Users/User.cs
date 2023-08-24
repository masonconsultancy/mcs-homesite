using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mcs_homesite.Areas.Models.Users
{
    public class User
    {
        public long? Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required, PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

    }
}
