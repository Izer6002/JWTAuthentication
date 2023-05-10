namespace JWTAuthentication.Database.Models
{
    public class Response <T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
