namespace JWTAuthentication.Database.Models
{
    public class LogInResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
