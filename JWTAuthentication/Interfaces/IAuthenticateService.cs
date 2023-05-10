using JWTAuthentication.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthentication.Interfaces
{
    public interface IAuthenticateService
    {
        Task<Response<LogInResponse>> LogIn([FromBody] LogInModel logInModel);
        Task<Response<bool>> Register([FromBody] RegisterModel registerModel);
        Task<Response<bool>> RegisterAdmin([FromBody] RegisterModel registerModel); 
    }
}
