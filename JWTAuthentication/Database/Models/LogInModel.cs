﻿using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Database.Models
{
    public class LogInModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
