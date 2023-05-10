using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Database.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string EmailAdress { get; internal set; }
    }
}
