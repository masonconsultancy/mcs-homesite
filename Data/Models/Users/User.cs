﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MCS.HomeSite.Data.CustomValidators;

namespace MCS.HomeSite.Data.Models.Users
{
    public class User
    {
        public long? Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [ValidateEmail]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required, PasswordPropertyText]
        public string Password { get; set; } = string.Empty;
    }
}
